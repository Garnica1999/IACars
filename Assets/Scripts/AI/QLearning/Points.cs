using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField]
    public float points;
    public static Points Instance;
    public CarController Controller { get; set; }


    public void Init()
    {
        this.points = 0.0f;
        if (!Instance){
            Instance = this;
        }
    }

    public float GetReward(int action, bool[] state, float[] sensorOutputs)
    {
        this.points = 10.0f;
        bool sensorTrigged = false;
        if (Car.Instance.IsAlive)
        {
            for(int i = 0; i < state.Length; i++)
            {
                //print(state[i] + "-" + sensorOutputs[i]);
                if (state[i])
                {
                    //points = 5.0f - (10f - sensorOutputs[i]) * 60f; 
                    points = -5f;
                    sensorTrigged = true;
                }
            }
            if(sensorTrigged == false)
            {
                if(action == 0)
                {
                    points = 20f;
                }
            }
        } else
        {
            points = -100f;
            //print("Choco el auto");
        }
        return points;
    }
}
