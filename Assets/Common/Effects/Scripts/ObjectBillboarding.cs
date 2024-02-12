using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBillboarding : MonoBehaviour
{

    Transform cam;
    
    void Start()
    {
        cam = Camera.main.transform;
    }

    
    void Update()
    {
        Vector3 newRot;
        newRot.x = -90f;
        newRot.y = cam.transform.position.y;
        newRot.z = 0f;

        transform.rotation = Quaternion.Euler(newRot);
    }
}
