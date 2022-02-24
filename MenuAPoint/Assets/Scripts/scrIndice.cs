using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrIndice : MonoBehaviour
{
    public void decrementIndice()
    {
        if (GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices > 0)
            GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices--;
    }
}
