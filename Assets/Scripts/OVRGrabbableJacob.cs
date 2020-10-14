using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CircularBuffer;

public class OVRGrabbableJacob : OVRGrabbable
{
    CircularBuffer<Vector3> positionBuffer = new CircularBuffer<Vector3>(60);
    CircularBuffer<float> timeBuffer = new CircularBuffer<float>(60);

    public void Update()
    {
        velocityPosBuffer();
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity, Vector3 cross)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;
        Vector3 fullVelocity = linearVelocity + cross;

        rb.velocity = findVelocity(); //USED TO SAY FULLVELOCITY, TEMP CHANGE TO NEW METHOD
        rb.angularVelocity = angularVelocity;
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }

    public void velocityPosBuffer()
    {
        positionBuffer.PushBack(gameObject.transform.position);
        timeBuffer.PushBack(Time.time);
    }
    private Vector3 findVelocity()
    {
        Vector3 farthestBackPos = positionBuffer[0];
        int indexKeeper = 0;
        float timeAtFurthest = timeBuffer[0];

        /* This looks for the furthest back x position within the past 60 frames.  This should be at the point when the arm was cocked back the furthest.
         * I am also looking for the time at which that happened relative to the start of the program */
        for (int i = 1; i < 60; i++)
        {
            if (farthestBackPos.x > positionBuffer[i].x) 
            {
                farthestBackPos = positionBuffer[i];
                indexKeeper = i;
            }
        }
        timeAtFurthest = timeBuffer[indexKeeper];
        float deltaTime = Time.time - timeAtFurthest;

        Vector3 finalVelocity = (positionBuffer.Back() - farthestBackPos) / deltaTime;
        Debug.Log("Shooting ball away with the following velocity:" + finalVelocity.magnitude);
        return finalVelocity;
    }
}
