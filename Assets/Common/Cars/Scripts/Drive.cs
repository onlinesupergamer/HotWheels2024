using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class Drive : MonoBehaviour
{
    public Transform[] wheels = new Transform[4];
    public Transform[] wheelModels = new Transform[4];
    public float distance;
    public float currentMultiplier;


    public Rigidbody rb;
    public float maxLength, springStrength, restDistance, frontWheelOffset, rearWheelOffset, dampingFactor, gravityAmount = 2500f, torqueMultiplier = 1000f, turnForce = 500f, normalSlideAmount = 15, driftSlideMount = 500, topSpeed = 35f;
    public float currentSpeed;

    bool[] bIsWheelGrounded = new bool[4];
    bool bIsGrounded;

    public AnimationCurve torqueCurve, steeringCurve;
    
    public PID pidController;


    RaycastHit hit;
    
    public float inAirTimer = 0f;
    public int turnDirection;
    public float sidewaysMomentum;
    public Transform boostStart;
    bool bCanRebound;

    Vector3 susForce;

    
    
    

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Accelerate");
        bool bIsBraking = Input.GetButton("Brake");
        bool bIsBoosting = Input.GetButton("Boost");
        
        
        for(int i = 0; i < wheels.Length; i++)
        {

               
            

                if (Physics.Raycast(wheels[i].transform.position, -wheels[i].up, out RaycastHit m_hit, maxLength))
                {
                    Vector3 springDir = m_hit.normal;
                    Vector3 tireWorldVel = rb.GetPointVelocity(wheels[i].transform.position) * (Time.deltaTime * 10);
                    float offset = restDistance - m_hit.distance;
                    
                    float vel = Vector3.Dot(springDir, tireWorldVel);
                    float force = (offset * springStrength) - (vel * dampingFactor);


                    Vector3 newWheelPos = wheelModels[i].transform.localPosition;
                    
                    
                   
                    hit = m_hit;

                    
                    rb.AddForceAtPosition(springDir * force, wheels[i].position);
                    bIsWheelGrounded[i] = true;

                   

                    

                    if(i == 0 || i == 1)
                    {
                        newWheelPos.y = -m_hit.distance + frontWheelOffset;
                        newWheelPos.y = Mathf.Clamp(newWheelPos.y, -0.25f, -0.15f);
                        wheelModels[i].transform.localPosition = newWheelPos;
                    }
                    

                    if(i == 2 || i == 3)
                    {
                        newWheelPos.y = -m_hit.distance + rearWheelOffset;
                        newWheelPos.y = Mathf.Clamp(newWheelPos.y, -0.25f, -0.05f);
                        wheelModels[i].transform.localPosition = newWheelPos;
                    
                    }
  
                }

               
                else

                {
                    bIsWheelGrounded[i] = false;
                    
                }

                 if(!bIsWheelGrounded[i])
                {
                    Vector3 newWheelPos = wheelModels[i].transform.localPosition;
                    newWheelPos.y = -maxLength / 2;
                    wheelModels[i].transform.localPosition = newWheelPos;


                }


                Debug.DrawRay(wheels[i].transform.position, -wheels[i].up * maxLength, Color.green);

                if(i == 0 || i == 1)
                {
                    Quaternion rot = wheels[i].localRotation;
                    rot.y = x / 2;
                    wheels[i].localRotation = rot;
                }



        }

        HandleGravity();



        sidewaysMomentum = Mathf.Clamp(Vector3.Dot(rb.transform.right, rb.velocity) / topSpeed, -1, 1);
        
        currentSpeed = Vector3.Dot(rb.transform.forward, rb.velocity);
        Vector3 projectedDirection = Vector3.ProjectOnPlane(transform.forward, hit.normal);
        

        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(currentSpeed) / topSpeed);
        float carTorque = torqueCurve.Evaluate(normalizedSpeed) * y;


        if(currentSpeed < topSpeed)
        {
            if(bIsGrounded)
                rb.AddForce(projectedDirection * (carTorque * torqueMultiplier));
        }

        float normalizedSteering = Mathf.Clamp01(Mathf.Abs(currentSpeed) / topSpeed);
        float steeringValue = steeringCurve.Evaluate(normalizedSteering) * x;

        if(bIsGrounded)
            rb.AddTorque(rb.transform.up * (steeringValue * turnForce), ForceMode.Acceleration);



        float currentSlideValue;

        //This can be used as a ternary operator
        if(bIsBraking && turnDirection != 0)
        {
            currentSlideValue = driftSlideMount;
        }

        else
        {
            currentSlideValue = normalSlideAmount;
        }


        

        float sideSpeed = Vector3.Dot(rb.velocity, transform.right);
        Vector3 SideFriction = -transform.right * (sideSpeed / Time.fixedDeltaTime / currentSlideValue); //Slideamount ranges from 1-500 with 1 being more grip and 500 being super ultra mega drift

        if(currentSlideValue < 1)
        {
            FileLog.CrashErrorHandler("Current slide value is too small!", true);
        }

        if(bIsGrounded)
            rb.AddForce(SideFriction, ForceMode.Acceleration);

        if(x < 0)
        {
            turnDirection = -1;
        }

        if(x > 0)
        {
            turnDirection = 1;
        }
        if(x == 0)

        {
            turnDirection = 0;
        }


        rb.angularVelocity = new Vector3(rb.angularVelocity.x, Mathf.Lerp(rb.angularVelocity.y, 0, Time.deltaTime * 2), rb.angularVelocity.z);

        RaycastHit boostHit;

        if(Physics.Raycast(boostStart.transform.position, -boostStart.transform.up, out boostHit, 0.8f))
        {
            float boostHeight = 1f;
            float forcePercent = pidController.Seek(boostHeight, boostHit.distance);
            float boostLiftForce = 150f;

            Vector3 Force = boostHit.normal * boostLiftForce * forcePercent;

            if(bIsBoosting)
            {
                //rb.AddTorque(-rb.transform.right * (boostLiftForce * forcePercent), ForceMode.Acceleration);
            }

                        

        }

       

        if(!bIsGrounded)
        {
            inAirTimer += Time.deltaTime;
        }
        
        else
        {
            if(inAirTimer >=  1f)
            {
                //Bounce(500f);
            }

            inAirTimer = 0f;

        }


    }

    void HandleGravity()
    {
        if(bIsWheelGrounded[0] == true || bIsWheelGrounded[1] == true || bIsWheelGrounded[2] == true || bIsWheelGrounded[3] == true)
        {
            rb.AddForce(-rb.transform.up * gravityAmount);
            bIsGrounded = true;
            //print("Is Grounded");
        }
        else
        {
            rb.AddForce(-Vector3.up * gravityAmount * 1.25f);
            //print("Is In Air");
            bIsGrounded = false;

        }

    }

    

    void Bounce(float bounceForce)
    {

        rb.AddForce(hit.normal * bounceForce, ForceMode.Acceleration);
        print("Bounce");
    }

 
}
