using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrIndice : MonoBehaviour
{
    public GameObject Indice;

    void Start()
    {
        level3unlocked();
    }

    public void decrementIndice()
    {
        if (GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices > 0)
            GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices--;
    }

    public void level3unlocked()
    {
        if(GameObject.Find("Global").GetComponent<scrGlobal>().levelNum > 3)
        {
            
            Indice.SetActive(true);
        }
    }
}
