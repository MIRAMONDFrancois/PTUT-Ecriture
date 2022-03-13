using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class scrGlobal : MonoBehaviour
{
    [Header("General Informations")]
    public string playerName;//Connexion
    public int levelNum;//Connexion
    private Donnees data;
    private Joueurs player;

    [Header("Unlocked Levels")]
    public List<bool> levelunlocked = new List<bool>();//Map
    
    
    [Header("Base Text File")]
    public TextAsset file;//Map pour Jeu{

    [Header("Taille Police Texte Base")]
    public float taille_police = 76.7f;

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
    public bool canBeDeleted;//}

    [Header("Phases de tuto et fin de jeu")]
    public bool tutoChecked = false;//Tuto
    public bool intro = false;//Synopsis
    public int nbIndices = 2;//Map pour Jeu

    public void Start()
    {
        setLevelUnlocked();
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

    private Joueurs GetPlayer()
    {
        TextAsset jsonFile = Resources.Load("donnees") as TextAsset;
        data = JsonUtility.FromJson<Donnees>(jsonFile.text);

        foreach(Joueurs j in data.donnees)
        {
            if(j.joueur.Equals(playerName))return j;
        }

        return CreationJoueur();
    }

    //Création Dossier + Ajout dans .json . Connexion [scrMenu]
    public Joueurs CreationJoueur()
    {
        Joueurs j = new Joueurs();

        j.joueur = playerName;

        data.donnees.Add(j);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("Assets/Resources/donnees.json", json);

        Directory.CreateDirectory ("./RESULTATS/"+playerName);

        return j;
    }

    //Charger variables pour Global. Connexion [scrMenu] 
    public void ChargeJoueur()
    {
        player = GetPlayer();

        nbIndices = player.indiceRestant;
        intro = player.intro;
        tutoChecked = player.tuto;

        for(int a=0;a<15;a++)
        {
            levelunlocked[a+1] = player.niveauxFinis[a];
        }
    }
    

    public void SetIntro()
    {
        player.intro = true;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("Assets/Resources/donnees.json", json);
    }

    public void SetTuto()
    {
        player.tuto = true;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("Assets/Resources/donnees.json", json);
    }
    //Pour Jeu. SceneTest [scrTextManager]
    public int GetChrono()
    {
        return player.chronoNiveau[levelNum-1];
    }

    //Pour Indice. SceneTest [scrIndice]
    public bool GetIndice()
    { 
        return player.indiceNiveau[levelNum-1];
    }

    //Pour Indice. SceneTest [scrIndice]
    public void SetIndice()
    {
        player.indiceNiveau[levelNum-1]=true;
        player.indiceRestant = nbIndices;
                
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("Assets/Resources/donnees.json", json);
    }

    //Data en .txt dans Resultats. SceneTest [scrTextManager]
    public void SetTexteFichier(string recap)
    {
        string chemin = "./RESULTATS/"+playerName+"/"+"Niveau_"+levelNum;
        Directory.CreateDirectory (chemin);

        File.WriteAllText(chemin+"/Essaie_"+player.essaies[levelNum-1]+".txt",recap);
    }

    public void SetReussite(int frame)
    {
        player.niveauxFinis[levelNum-1] = true;
        player.chronoNiveau[levelNum-1] = frame;
        player.essaies[levelNum-1]++;
        
        string json = JsonUtility.ToJson(data);

        File.WriteAllText("Assets/Resources/donnees.json", json);
    }

    public void SetRetour(int frame)
    {
        player.chronoNiveau[levelNum-1] += frame;
        player.essaies[levelNum-1]++;
        
        string json = JsonUtility.ToJson(data);

        File.WriteAllText("Assets/Resources/donnees.json", json);
    }
}
