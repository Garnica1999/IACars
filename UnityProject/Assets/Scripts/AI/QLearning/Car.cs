using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Points Points;
    private CarMovement Movement;
    public bool IsAlive;
    public static Car Instance;
    private Sensor[] sensors;
    private Vector3 VectorPosition;
    private Quaternion InitialRotation;
    private QLearning Learning;
    private int GenerationCount = 0;
    private float timeGeneration = 0f;
    private bool PermitirGuardar = false;
    private bool autoChocado = false;
    private float TiempoGuardado = 0.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        sensors = GetComponentsInChildren<Sensor>();

        VectorPosition = transform.position;
        InitialRotation = transform.rotation;
        this.Instantiate();
    }
    void Start()
    {
        
        
        
    }

    void Update()
    {
        this.timeGeneration += Time.deltaTime;
        this.TiempoGuardado += Time.deltaTime;
        if (TiempoGuardado >= 10)
        {
            this.Learning.EscribirArchivo(this.GenerationCount, this.timeGeneration);
            //PermitirGuardar = false;
            TiempoGuardado = 0.0f;
        }
    }

    void FixedUpdate()
    {
        float[] sensorsOutput = new float[this.sensors.Length];
        for (int i = 0; i < this.sensors.Length; i++)
        {
            //print("Sensor: " + sensors[i].gameObject.name + ", Value: " + sensors[i].Output);
            sensorsOutput[i] = sensors[i].Output;//Probability
        }
        this.Learning.sensorOutput = sensorsOutput;
        this.Learning.Learn();

    }

    void ShowSensors()
    {
        foreach (Sensor sensor in sensors)
        {
            sensor.Show();
        }
    }

    void HideSensors()
    {
        foreach (Sensor sensor in sensors)
        {
            sensor.Show();
        }
    }

    void Instantiate()
    {
        this.Movement = GetComponent<CarMovement>();
        sensors = GetComponentsInChildren<Sensor>();
        Learning = new QLearning();
        Points = new Points();
        this.ShowSensors();
        Init();
    }

    void Init()
    {
        this.Learning.Init();
        this.Points.Init();
        this.IsAlive = true;
    }

    void Reset()
    {
        transform.position = VectorPosition;
        transform.rotation = InitialRotation;
        this.IsAlive = true;
        this.ShowSensors();
    }

    IEnumerator Die(float waitTime)
    {
        this.IsAlive = false;
        this.HideSensors();
        this.Stop();
        yield return new WaitForSeconds(waitTime);
        
        Reset();
    }

    private void Stop()
    {
        this.Movement.Stop();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.gameObject.tag.Equals("Wall"))
        {
            PermitirGuardar = true;
            StartCoroutine(Die(0.7f));
            this.GenerationCount++;
            this.timeGeneration = 0f;
        }
    }

    public Points GetPoints()
    {
        return this.Points;
    }

    public CarMovement GetMovement()
    {
        return this.Movement;
    }

}
