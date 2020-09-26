using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Quaternion Rotation;
    [SerializeField]
    private float horizontalInput;
    [SerializeField]
    private int TURN_SPEED = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotation = transform.rotation;
        Rotation *= Quaternion.AngleAxis((float)-horizontalInput * TURN_SPEED * Time.deltaTime, new Vector3(0, 0, 1));
        transform.rotation = Rotation;
    }
}
