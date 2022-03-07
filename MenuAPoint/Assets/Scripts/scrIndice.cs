using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrIndice : MonoBehaviour
{
    private bool used = false;
    public GameObject Indice;

    void Start()
    {
        level3unlocked();
    }

    public void decrementIndice()
    {
        if (GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices > 0 && !used)
        {
            GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices--;

            used = true;
            GameObject.Find("GameManager").GetComponent<scrTextManager>().showIndice();
        }
    }

    public void level3unlocked()
    {
        if(GameObject.Find("Global").GetComponent<scrGlobal>().levelNum > 3)
        {
            
            Indice.SetActive(true);
        }
    }
}
