using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrWiggle : MonoBehaviour
{
    private float x = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        x += 2*Time.deltaTime;
        Quaternion newRot = gameObject.transform.localRotation;
        newRot.z = Mathf.Cos(x) / 40f;
        gameObject.transform.localRotation = newRot;
    }
}
