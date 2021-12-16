using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrWiggle : MonoBehaviour
{
    private float goingRight = 1f;
    private float goingDown = 1f;
    private float rotSpeed = 0.05f;
    private float x = 0f;


    // Update is called once per frame
    void FixedUpdate()
    {
        x += 2*Time.deltaTime;
        var y = Mathf.Cos(x);
        //Debug.Log(y);
        /*
        Quaternion newRot = gameObject.transform.localRotation;
        if (Mathf.Abs(newRot.z) > 5f) {
            goingRight *= -1f;
        } else {
            newRot.z += goingRight * rotSpeed;
        }
        gameObject.transform.localRotation = newRot;
        */
        Quaternion newRot = gameObject.transform.localRotation;
        Debug.Log(newRot.z);
        newRot.z = y / 40f;
        gameObject.transform.localRotation = newRot;
    }
}
