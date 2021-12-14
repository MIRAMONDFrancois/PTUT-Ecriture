using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrGlobal : MonoBehaviour
{
    public string playerName;
    public int levelNum;
    public bool level2unlocked = false;
    public TextAsset file;


    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Global");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
