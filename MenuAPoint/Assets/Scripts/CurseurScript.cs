using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseurScript : MonoBehaviour
{
    public AudioSource AudioManger;
    
    public void change_taille()
    {
        scrTextManager GameManagerScript = GameObject.Find("GameManager").GetComponent<scrTextManager>();
        Vector2 taillecurseur = GetComponent<RectTransform>().sizeDelta;
        
        GetComponent<RectTransform>().sizeDelta = new Vector2(taillecurseur[0]*GameManagerScript.scaler_x,taillecurseur[1]*GameManagerScript.scaler_y);
    }
}
