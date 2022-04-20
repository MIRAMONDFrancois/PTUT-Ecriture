using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AideBuilder : MonoBehaviour
{
    public GameObject Tuto;

    void Start()
    {
        Tuto.transform.localScale = Vector3.zero;
    }

    public void Show()
    {
        Tuto.transform.localScale = new Vector2(1f,1f);
    }

    public void Hide()
    {
        Tuto.transform.localScale = Vector3.zero;
    }
}
