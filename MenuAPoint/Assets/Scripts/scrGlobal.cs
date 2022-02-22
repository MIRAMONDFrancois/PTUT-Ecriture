using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrGlobal : MonoBehaviour
{
    [Header("General Informations")]
    public string playerName;
    public int levelNum;

    [Header("Unlocked Levels")]
    public  List<bool> levelunlocked = new List<bool>();
    
    [Header("Base Text File")]
    public TextAsset file;
    
    [Header("Fixed Separators")]
    public bool isSpecial;
    public TextAsset specialFile;

    [Header("Limited Generators")]
    public int pointLimit = 0;
    public int virguleLimit = 0;
    public int exclamationLimit = 0;
    public int interrogationLimit = 0;
    public int deuxpointsLimit = 0;
    public int pointvirguleLimit = 0;

    [Header("Animation Levels")]
    public bool nivAntiOubli;
    public TextAsset animTextFile;
    public bool canBeMoved;
    public bool canBeDeleted;
    public bool tutoChecked;

    public void Start(){
        levelunlocked.Add(true);
        for (int i = 1; i<= 15; i++){
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
