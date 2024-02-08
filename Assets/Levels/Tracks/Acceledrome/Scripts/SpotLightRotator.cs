using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpotLightRotator : MonoBehaviour
{
   
    public float rotateSpeed;
    float speedRandom;

    void Start()
    {
        speedRandom = Random.Range(0.75f, 1.25f);
    }
    void FixedUpdate()
    {
        transform.Rotate(transform.right * (rotateSpeed * Time.deltaTime));
    }
}
