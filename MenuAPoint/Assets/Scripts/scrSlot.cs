using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSlot : MonoBehaviour
{
    public GameObject txtManager;
    public int INDEX;
    public bool isUsed = false;
    public bool indice = false;
    public float ligne;
    public string ponctuation;
    public Vector3 pos_origine;

    public void showIndice()
    {
        indice = true;
        if(!isUsed)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        
    }
}
