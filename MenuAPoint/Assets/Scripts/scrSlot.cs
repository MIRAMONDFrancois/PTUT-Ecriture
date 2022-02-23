using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSlot : MonoBehaviour
{
    public GameObject txtManager;
    public int INDEX;
    public bool isUsed = false;
    public float ligne;
    //public bool fromSlotsOne = true;

    public void SendPonct(string ponct)
    {
        txtManager.GetComponent<scrTextManager>().separators[INDEX] = ponct;
        txtManager.GetComponent<scrTextManager>().RefreshText();
        //Debug.Log("Sent ponct [" + ponct + "] to list " + fromSlotsOne + " (true=1;false=2)");
    }
}
