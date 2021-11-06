using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSlot : MonoBehaviour
{
    public GameObject txtManager;
    public int INDEX;
    public bool isUsed = false;
    public bool fromSlotsOne = true;

    public void SendPonct(string ponct)
    {
        if (fromSlotsOne) txtManager.GetComponent<scrTextManager>().separators[INDEX] = ponct;
        else txtManager.GetComponent<scrTextManager>().separators2[INDEX] = ponct;
        txtManager.GetComponent<scrTextManager>().RefreshText();
        Debug.Log("Sent ponct [" + ponct + "] to list " + fromSlotsOne + " (true=1;false=2)");
    }
}
