using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrGlobal : MonoBehaviour
{
    public string playerName;
    public int levelNum;
    public  List<bool> levelunlocked = new List<bool>();
    
    
    public TextAsset file;
    
    public TextAsset specialFile;
    public bool isSpecial = false;
    public bool nivAntiOubli = false;
    public int pointLimit = 0;
    public int virguleLimit = 0;

    public void Start(){
        levelunlocked.Add(true);
        for (int i = 1; i<= 14; i++){
            levelunlocked.Add(false);
        }
        
    }
    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Global");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
