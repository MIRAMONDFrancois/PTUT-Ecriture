using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class scrGlobal : MonoBehaviour
{
    [Header("General Informations")]
    public string playerName;
    public int levelNum;

    [Header("Unlocked Levels")]
    public List<bool> levelunlocked = new List<bool>();
    
    [Header("Base Text File")]
    public TextAsset file;
    
    [HideInInspector]
    public bool isSpecial;
    public TextAsset specialFile;

    [Header("Position Text (en pourcentage)")]
    public float debut_text_gauche = 10f;//10% à gauche
    public float marge_text_droite = 10f;//10% à droite
    public float taille_police = 4f;//1920(taille écran) * .04f = 76. -> taille avec laquelle on a conçu le jeu
    public float debut_hauteur_dual = 53f;//ça commence en bas
    public float debut_hauteur_normal = 97f;
    public float hauteur_table = 46f;//.43 +.03 de marge pour les trucs chiants -> pq, dépassement pot

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

    [Header("Phases de tuto et fin de jeu")]
    public bool tutoChecked;
    public int nbIndices = 5;

    public void Start()
    {
        setLevelUnlocked();

        //Creation du Dossier RESULTATS
        if (!Directory.Exists("Assets/Resources/RESULTATS")) {
            Directory.CreateDirectory ("Assets/Resources/RESULTATS");
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

    public void setLevelUnlocked()
    {
        levelunlocked.Add(true);
        for (int i = 1; i <= 15; i++)
        {
            levelunlocked.Add(false);
        }
    }
}
