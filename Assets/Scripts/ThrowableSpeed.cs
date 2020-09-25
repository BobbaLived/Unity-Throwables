using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSpeed : MonoBehaviour
{
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OutputVelocity", 1f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OutputVelocity()
    {
        Debug.Log("Velocity of throwable:" + GetComponent<Rigidbody>().velocity.magnitude);
    }
}
