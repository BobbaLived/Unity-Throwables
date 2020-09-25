using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRelease : MonoBehaviour
{
    public GameObject CollidingObject;
    public GameObject objectInHand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            CollidingObject = other.gameObject;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        CollidingObject = null;
    }
}
