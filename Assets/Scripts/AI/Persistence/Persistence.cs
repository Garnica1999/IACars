using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Persistence : MonoBehaviour
{
    private CarController target;
    /// <summary>
    /// The Car to fill the GUI data with.
    /// </summary>
    public CarController Target
    {
        get { return target; }
        set
        {
            if (target != value)
            {
                target = value;

                //if (target != null)
                    //NeuralNetPanel.Display(target.Agent.FNN);
            }
        }
    }
    public static Persistence Instance;
    private float ActualTime;
    private static string SEPARATOR = "\t";
    private string directory;
    private string nameFile;
    public string actualDate;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        this.actualDate = this.GetDateNow();
        this.createFile();
    }

    private void createFile()
    {
        string header = "";
        this.directory = "FNN-GA";
        header = "Fecha" + SEPARATOR +
        "Nombre" + SEPARATOR +
        "Generacion" + SEPARATOR +
        "Evaluation" + SEPARATOR +
        "Fitness" + SEPARATOR +
        "position x" + SEPARATOR +
        "position y" + SEPARATOR +
        "Inputs FNN" + SEPARATOR +
        "Outputs FNN" + Environment.NewLine;
        
        
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        if (!Directory.Exists(directory + "\\" + actualDate))
        {
            Directory.CreateDirectory(directory + "\\" + actualDate);
        }

        this.nameFile = "Report.txt";


        

        File.AppendAllText(this.directory + "\\" + actualDate + "\\" + this.nameFile, header);
    }

    // Update is called once per frame
    void Update()
    {
        ActualTime += Time.deltaTime;
        if (ActualTime >= 0.5f)
        {
            this.SaveFileFNN();
            
            ActualTime = 0f;
        }
    }

    private void SaveFileFNN()
    {
        if(Target != null)
        {
            if (Target.Agent.IsAlive)
            {
                uint ActualGeneration = EvolutionManager.Instance.GenerationCount;
                float Evaluation = Target.Agent.Genotype.Evaluation;
                float fitness = Target.Agent.Genotype.Fitness;
                Vector3 carPosition = Target.GetComponent<Transform>().position;
                double[] inputs = Target.FNNInputs;
                double[] outputs = Target.FNNOutputs;
                string nameCar = Target.GetComponent<Transform>().gameObject.name;

                
                string cadFile = "";
                cadFile += this.GetDateNow() + SEPARATOR +
                    nameCar + SEPARATOR +
                    ActualGeneration + SEPARATOR +
                    Evaluation + SEPARATOR +
                    fitness + SEPARATOR +
                    carPosition.x + SEPARATOR +
                    carPosition.y + SEPARATOR;

                foreach (double input in inputs)
                {
                    cadFile += input + "\t";
                }
                cadFile = Persistence.replaceCharOnString(cadFile);

                cadFile += SEPARATOR;
                foreach (double output in outputs)
                {
                    cadFile += output + "\t";
                }
                cadFile = Persistence.replaceCharOnString(cadFile);

                cadFile += SEPARATOR + Environment.NewLine;

                File.AppendAllText(this.directory + "\\" + actualDate + "\\" + this.nameFile, cadFile);
            }
        }
    }

    public static string replaceCharOnString(string cadFile)
    {
        StringBuilder sb = new StringBuilder(cadFile);
        sb[cadFile.Length - 1] = '\0';
        return sb.ToString();
    }

    private string GetDateNow()
    {
        DateTime datenow = DateTime.Now;
        string fechaActual = datenow.ToString();
        return fechaActual.Split(' ')[0].Replace('/','-');
    }
}
