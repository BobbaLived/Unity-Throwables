using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CircularBuffer;

public class OVRGrabberJacob : OVRGrabber
{
    public override void Update()
    {
        VelocityCounter();
        if (m_operatingWithoutOVRCameraRig)
        {
            OnUpdatedAnchors();
        }
    }

    CircularBuffer<Vector3> velocityBuffer = new CircularBuffer<Vector3>(5);
    CircularBuffer<Vector3> angularBuffer = new CircularBuffer<Vector3>(3);
    CircularBuffer<Vector3> accelerationBuffer = new CircularBuffer<Vector3>(15);

    

    public override void GrabEnd()
    {
        if (m_grabbedObj != null)
        {
            
            Vector3 averageAngular = Vector3.zero;
            Vector3 averageLinear = Vector3.zero;
            Vector3 averageAcceleration = Vector3.zero;
            
            foreach (Vector3 x in velocityBuffer) //Calculate the AVERAGE 3d vector for velocity
            {
                averageLinear += x;
            }
            averageLinear = averageLinear / velocityBuffer.Size;

            foreach (Vector3 y in angularBuffer) //Calculate the AVERAGE 3d vector for angular velocity
            {
                averageAngular += y;
            }
            averageAngular = averageAngular / angularBuffer.Size;

            foreach (Vector3 z in accelerationBuffer)
            {
                averageAcceleration += z;
            }
            averageAcceleration = averageAcceleration / accelerationBuffer.Size;
            averageAcceleration.y = Mathf.Abs(averageAcceleration.y);
       
           
            Debug.Log("\nAverage linear: " + averageLinear.magnitude + ".  Average angular:" + averageAngular.magnitude);
            GrabbableRelease(averageLinear, averageAngular, averageAcceleration);
        }

        // Re-enable grab volumes to allow overlap events
        GrabVolumeEnable(true);
    }

    public void VelocityCounter()
    {
        OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) }; //What is controller pos/rot
        OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation }; //What is original pos/rot
        localPose = localPose * offsetPose; //Multiply both vectors & quaternion's
        OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
        //All that these lines do is take the velocity of the hand (which happens to be the velocity of our ball) and 
        //Removes the local velocity and instead puts it into global space.

        velocityBuffer.PushFront(trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller));
        angularBuffer.PushFront(trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller));
        accelerationBuffer.PushFront(trackingSpace.orientation * OVRInput.GetLocalControllerAcceleration(m_controller));
        //Takes these data points and puts them into world space
    }

}
