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
    CircularBuffer<Vector3> angularBuffer = new CircularBuffer<Vector3>(5);
    Vector3 controllerCenterOfMassLocal;

    public override void GrabEnd()
    {
        if (m_grabbedObj != null)
        {
            controllerCenterOfMassLocal = GetComponent<Rigidbody>().centerOfMass; //Rename so parent and child class do not have same pub. var.
            Vector3 averageAngular = Vector3.zero;
            Vector3 averageLinear = Vector3.zero;
            foreach (Vector3 x in velocityBuffer) //Calculate the AVERAGE 3d vector for velocity
            {
                averageLinear += x;
            }
            foreach (Vector3 y in angularBuffer)
            {
                averageAngular += y;
            }
            averageLinear = averageLinear / velocityBuffer.Size;
            averageAngular = averageAngular / angularBuffer.Size;
            Vector3 controllerVelocityCross = Vector3.Cross(averageLinear, m_grabbedObjectPosOff - controllerCenterOfMassLocal);
            /*TRYING OUT CONTROLLERVELOCITYCROSS IN THIRD ARG OF GRAB RELEASE -> USED TO BE CONTROLLERCENTER OFMASS
             * 
             */

            GrabbableRelease(averageLinear, averageAngular, controllerVelocityCross); //By including this center of mass, we take into account
            //The controller's center of mass, not the object.
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
        Vector3 oculusFactor = new Vector3(2.5f, 2.5f, 2.5f);

        velocityBuffer.PushFront(trackingSpace.orientation * Vector3.Scale(OVRInput.GetLocalControllerVelocity(m_controller), oculusFactor)); //multiply everything by 2.5x
        angularBuffer.PushFront(trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller));
        //Take the velocity from the controller and orient it correctly for the ball being thrown
    }
}
