using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollisionHandler : MonoBehaviour
{

    Drive driveScript;

    Rigidbody rb;
    public int collisionLayer = 6;

    [SerializeField]
    float hitangle;

    [SerializeField]
    float hitdirection;

    [SerializeField]
    Vector3 vlctytmpbffr;
        

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        driveScript = GetComponent<Drive>();
    }


    void OnCollisionEnter(Collision other)
    {
        

        if(other.gameObject.layer == collisionLayer)
        {
            hitangle = Vector3.Dot(transform.forward, other.contacts[0].normal);
            hitdirection = Vector3.Dot(transform.right, other.contacts[0].normal);

            if(hitangle > 0)
            {
                print("Back");
            }

            if(hitangle < 0)
            {
                print("Front");
            }
        }

        
    }
}
