using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QLearning : MonoBehaviour
{

    #region QLearning
    public float[,] q { get; set; }
    
    public bool[] state { get; set; }


    [SerializeField]
    private float a = 0.1f;
    [SerializeField]
    private float y = 0.9f;

    public float[] sensorOutput { get; set; }
    public float ultimoValor = 0f;
    private static string SEPARATOR = "\t";
    private string directory = "QLearning";
    #endregion

    public void Init(){
        state = new bool[]{false, false, false, false, false};
        int columnas = ((int)Mathf.Pow(2,state.Length));
        q = new float[columnas, 3];
        for(int i = 0; i < columnas; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                q[i, j] = 0f;
            }
        }

        this.InitFiles();
    }

    private void InitFiles()
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        string header = "Generation" + SEPARATOR +
        "Time" + SEPARATOR +
        "Position x" + SEPARATOR +
        "Position y" + SEPARATOR +
        "Reward" + SEPARATOR +
        "Sensors" + Environment.NewLine;
        File.AppendAllText(directory + "\\" + Car.Instance.gameObject.name + "-summary.txt", header);
    }

    private int Calcstate()
    {
        int st = 0;
        for(int n = 0; n < this.state.Length; n++)
        {
            st = st + (Convert.ToInt32(Mathf.Pow(2, n))) * (Convert.ToInt32(this.state[n]));
        }
        //print("st: " + st);
        return st;
    }

    private int SelectAction(int state, bool rand)
    {
        int selected_action = 0;
        float val = Mathf.NegativeInfinity;
        for(int n = 0; n < 3; n++)
        {
            if(q[state, n] > val)
            {
                val = q[state, n];
                selected_action = n;
            }
        }
        if(rand && MathHelper.Random(0.0f,100.0f) >= 99f)
        {
            selected_action = Convert.ToInt32(Mathf.Floor(MathHelper.Random(0.0f, 3.0f)));
        }
        //print("Nombre: " + this.controller.gameObject.name + "Selected action: " + selected_action);
        return selected_action;
    }

    private void PerformAction(int action)
    {
        if (action == 1)
        {
            //GIRAR A LA IZQUIERDA
            Car.Instance.GetMovement().TurnCar((int)CarController.Directions.izquierda);
        }
        if (action == 2)
        {
            //GIRAR A LA DERECHA
            Car.Instance.GetMovement().TurnCar((int)CarController.Directions.derecha);
        }
        //ACTUALIZAR POSICION
        this.UpdateStates();
    }

    public void Learn() {
        int current_state = this.Calcstate();
        int action = SelectAction(current_state, true);
        this.PerformAction(action);
        
        //LEARNING
        float reward = Car.Instance.GetPoints().GetReward(action, state, sensorOutput);
        //print("Nombre: " + this.controller.gameObject.name + "Reward: " + reward);
        int fstate = Calcstate();
        int faction = SelectAction(fstate, false);
        float current_state_value = q[current_state, action];
        q[current_state, action] = (1f - a) * q[current_state, action] + a * (reward + y * q[fstate, faction]);
        this.ultimoValor = current_state_value;
        /*string data = "[";
        for(int i = 0; i < 3; i++)
        {
            data = data + q[current_state, i] + ",";
        }
        data += "]";*/
        
        //print("Nombre: " + this.controller.gameObject.name  + "ESTADO :" + current_state + " Q= " + current_state_value);
        //print("--------------------");
    }

    public void UpdateStates()
    {
        for(int i = 0; i < this.sensorOutput.Length; i++)
        {
            this.state[i] = (this.sensorOutput[i] < 7f) ? true : false;

            /*if (this.sensorOutput[i] > 0f)
            {
                this.state[i] = false;
            } else
            {
                this.state[i] = true;
            }*/
        }
    }
    public void EscribirArchivo(int GenerationCount, float timeGeneration)
    {
        DateTime now = DateTime.Now;
        string fecha = now.ToString("F");

        string matrix = "";
        int columnas = ((int)Mathf.Pow(2, this.state.Length));
        for (int i = 0; i < columnas; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                matrix = matrix + "\t" + this.q[i, j] + "\t";

            }
            matrix += Environment.NewLine;
        }

        string data = "[" + fecha + "]" + " Car " + Car.Instance.gameObject.name + ": " + Environment.NewLine +
            "\tGeneracion: " + GenerationCount + Environment.NewLine +
            "\tTiempo Transcurrido" + timeGeneration + Environment.NewLine +
            "\tQ Matrix: " + Environment.NewLine +
            matrix + Environment.NewLine +
            "\tReward: " + this.ultimoValor + Environment.NewLine +
            "_____________________________________________________" + Environment.NewLine;
        
        File.AppendAllText(directory + "\\" + Car.Instance.gameObject.name + ".txt", data);

        string text = "";
        text += GenerationCount + SEPARATOR +
        Car.Instance.GetComponent<Transform>().position.x + SEPARATOR +
        Car.Instance.GetComponent<Transform>().position.y + SEPARATOR +
        this.ultimoValor + SEPARATOR;
        foreach (float sensorData in this.sensorOutput)
        {
            text += sensorData + SEPARATOR;
        }
        text = Persistence.replaceCharOnString(text);
        text += Environment.NewLine;
        File.AppendAllText(directory + "\\" + Car.Instance.gameObject.name + "-possensors.txt", text);

    }

    public void EscribirArchivoSec(int GenerationCount, float timeGeneration)
    {
        string text = "";
        text += GenerationCount + SEPARATOR +
        timeGeneration + SEPARATOR +
        Car.Instance.GetComponent<Transform>().position.x + SEPARATOR +
        Car.Instance.GetComponent<Transform>().position.y + SEPARATOR +
        this.ultimoValor + SEPARATOR;
        foreach (float sensorData in this.sensorOutput)
        {
            text += sensorData + SEPARATOR;
        }
        text = Persistence.replaceCharOnString(text);
        text += Environment.NewLine;
        File.AppendAllText(directory + "\\" + Car.Instance.gameObject.name + "-summary.txt", text);
    }

}
