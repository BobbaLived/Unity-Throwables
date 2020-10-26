using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CircularBuffer;

public class OVRGrabbableJacob : OVRGrabbable
{
    
    public GameObject floorReference; //To reference floor pos.y
    float velocity; 
    public Vector3 objectDir;
    GameObject ball;
    Vector3 velocityPerm;
    Vector3 angularVelocityPerm;
    Vector3 accelerationPerm;

    float density = 1.112f; //Average air density at 1000 feet above sea level.
    float dragCoefficient = 0.3f; //According to Nasa! https://www.grc.nasa.gov/WWW/K-12/airplane/balldrag.html
    float crossSectionalArea = 0.00426f; //According to RIT http://spiff.rit.edu/richmond/baseball/traj/traj.html


    bool thrown = false;
    float Initx;
    float Inity;
    float Initz;

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity, Vector3 acceleration)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;

        velocityPerm = linearVelocity;
        angularVelocityPerm = angularVelocity;
        accelerationPerm = acceleration;

        Initx = acceleration.x;
        Inity = acceleration.y;
        Initz = acceleration.z;

        //rb.AddForce(acceleration.x, 20, acceleration.z);
        //transform.Translate(acceleration.x * Time.deltaTime, 0, acceleration.z * Time.deltaTime);
        rb.angularVelocity = angularVelocity;
        thrown = true;
        
      
        m_grabbedBy = null;
        m_grabbedCollider = null;
        //InvokeRepeating("GravityNoRb", 0, 0.01f);
        //InvokeRepeating("AirResistanceNoRb", 0, 0.01f);

    }

    public void Update()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (thrown == true)
        {
            Debug.Log("Calling the deltas!");
            Inity -= 9.81f * Time.deltaTime;

            float deltaX = 0.5f * Initx * Time.deltaTime;
            Debug.Log("DeltaX = " + deltaX);
            float deltaY = 0.5f * Inity * Time.deltaTime;
            Debug.Log("DeltaY = " + deltaY);
            float deltaZ = 0.5f * Initz * Time.deltaTime;
            Debug.Log("DeltaZ = " + deltaZ);
            Vector3 Movepos = new Vector3(rb.position.x + deltaX, rb.position.y + deltaY, rb.position.z + deltaZ);
            rb.MovePosition(Movepos);
 
        }
    }

   /* public void TestMethod()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (x > 0.01)
        {
            Debug.Log("Calling this!");
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y + 0.5f * -9.81f * 0.02f, rb.position.z));
            x = 0;
        }
        else
        {
            x += Time.deltaTime;
        }

    } */

    /*public void FlightForces()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb.position.y > -9.06)
        {
            rb.AddForce(0, -9.81f, 0, ForceMode.Acceleration); //Gravity!

            objectDir = -rb.velocity.normalized; //Gives vector of size one with opposite signs of velocity
                                                 //Air resistance will always be applied negative to the object's 3D motion vector
            velocity = rb.velocity.magnitude;

            rb.AddForce(objectDir * ((density * velocity * velocity * dragCoefficient * crossSectionalArea) / 2)); //Air resistance!
        }
        
    }

   
    public void GravityNoRb()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Debug.Log("Got Here! #1");
        float gravityMultiple = 0.5f * -9.81f * Time.deltaTime;
        Debug.Log("Got here! #2: " + gravityMultiple);
        if (rb.position.y > 0)
        {
            Debug.Log("Transforming by: " + (0.5f * -9.81f * Time.deltaTime));
            //transform.Translate(0, gravityMultiple, 0, Space.World);
            //transform.transform += new Vector3(0, gravityMultiple, 0);
        }
        else
        {
            CancelInvoke(); //After the ball has gone past the floor, stop calling this method repeatedly
            
        } 
        
    }

    public void AirResistanceNoRb()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb.position.y > -9.06)
        {

            objectDir = -rb.velocity.normalized; //Gives vector of size one with opposite signs of velocity
                                                 //Air resistance will always be applied negative to the object's 3D motion vector
            velocity = rb.velocity.magnitude;

            transform.Translate(0.5f * ((objectDir * ((density * velocity * velocity * dragCoefficient * crossSectionalArea) / 2))/rb.mass) * Time.deltaTime * Time.deltaTime, Space.World);
        }
        else
        {
            CancelInvoke(); //After the ball has gone past the floor, stop calling this method repeatedly
        }
    }*/

}
