using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrSound : MonoBehaviour
{
    public GameObject toggle;
    public void MuteAllSound()
    {
        if (!toggle.GetComponent<Toggle>().isOn)
            AudioListener.volume = 0;
        else
            AudioListener.volume = 1;

    }
}
