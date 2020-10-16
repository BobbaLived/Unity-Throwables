using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CircularBuffer;

public class OVRGrabbableJacob : OVRGrabbable
{
    
    CircularBuffer<Vector3> accelerationBuffer = new CircularBuffer<Vector3>(60);

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity, Vector3 acceleration)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;

        rb.AddForce(acceleration);
        rb.angularVelocity = angularVelocity;
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }

    
}
