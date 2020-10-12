using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CircularBuffer;

public class OVRGrabbableJacob : OVRGrabbable
{
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity, Vector3 cross)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;
        Vector3 fullVelocity = linearVelocity + cross;

        rb.velocity = fullVelocity;
        rb.angularVelocity = angularVelocity;
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }
}
