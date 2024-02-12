using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSkyRotate : MonoBehaviour
{
    public float rotateSpeed;
    float speedRandom;

    void Start()
    {
        speedRandom = Random.Range(0.75f, 1.25f);
    }
    void FixedUpdate()
    {
        transform.Rotate(transform.forward * (rotateSpeed * Time.deltaTime));
    }
}
