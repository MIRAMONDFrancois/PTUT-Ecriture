using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class scrGlobal : MonoBehaviour
{
    [Header("General Informations")]
    public static scrGlobal Instance;
    public string playerName;//Connexion
    public int levelNum;//Connexion
    private Donnees data;
    private Joueurs player;
    private string chemin_json;
    private string chemin_txt;
    public string chemin_bonus;
    public TextMeshProUGUI debug;

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
    public int nbIndices = 0;//Map pour Jeu


    //GameBuilder
    public bool FromGameBuilder;
    public bool FromBonusLevel;
    public TextAsset GameBuilderText;
    public string NameBuilderText;

    void Start()
    {
        #if UNITY_EDITOR
            chemin_json = Application.streamingAssetsPath + "/donnees.json";
            chemin_txt = "./Resultats";
        #elif UNITY_ANDROID
            chemin_json = Application.persistentDataPath + "/donnees.json";
            chemin_txt = Application.persistentDataPath + "/Resultats";
        
        #else
            chemin_json = Application.streamingAssetsPath + "/donnees.json";
            chemin_txt = "./Resultats";
        #endif

        chemin_bonus = Application.persistentDataPath + "/NiveauxBonus";
        Directory.CreateDirectory (chemin_bonus);

        if(!File.Exists(chemin_json))
        {
            File.WriteAllText(chemin_json, "{\"donnees\": [],\"niveauxBonus\": []}");
        }

        debug.text = Application.persistentDataPath;
        
        SetNiveauxBonus();
        setLevelUnlocked();
    }

    void Awake() {
        /*GameObject[] objs = GameObject.FindGameObjectsWithTag("Global");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);*/
        if(Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void setLevelUnlocked()
    {
        levelunlocked.Add(true);
        for (int i = 1; i <= 15; i++)
        {
            levelunlocked.Add(false);
        }
    }

    public Joueurs GetPlayer()
    {
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

        for(int a=0;a<data.niveauxBonus.Count;a++)
        {
            j.niveauxBonusFinis.Add(data.niveauxBonus[a].nom,false);
            j.indiceBonus.Add(data.niveauxBonus[a].nom,false);
            j.essaiesBonus.Add(data.niveauxBonus[a].nom,1);
            j.chronoNiveauBonus.Add(data.niveauxBonus[a].nom,0);
        }
        
        data.donnees.Add(j);
        WriteInJson();
        Directory.CreateDirectory (chemin_txt + "/" +playerName);

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
        WriteInJson();
    }

    public void SetTuto()
    {
        player.tuto = true;
        WriteInJson();
        
    }
    //Pour Jeu. SceneTest [scrTextManager]
    public int GetChrono()
    {
        if(FromGameBuilder)return 0;

        return player.chronoNiveau[levelNum-1];
    }

    //Pour Indice. SceneTest [scrIndice]
    public bool GetIndice()
    {
        if(FromGameBuilder)return false;

        return player.indiceNiveau[levelNum-1];
    }

    //Pour Indice. SceneTest [scrIndice]
    public void SetIndice()
    {
        if(FromGameBuilder)return;
        
        player.indiceNiveau[levelNum-1]=true;
        player.indiceRestant = nbIndices;
                
        WriteInJson();
    }

    //Data en .txt dans Resultats. SceneTest [scrTextManager]
    public void SetTexteFichier(string recap)
    {
        if(FromGameBuilder)return;

        string chemin = chemin_txt+"/"+playerName+"/Niveau_"+levelNum;
        Directory.CreateDirectory(chemin);

        File.WriteAllText(chemin+"/Essaie_"+player.essaies[levelNum-1]+".txt",recap);
    }

    public void CreateTexteBuilder()
    {
        string[] trial = NameBuilderText.Split('.');

        for(int a=0;a<trial.Length;a++)
        {
            print("trial "+trial[a]);
        }

        if(trial[trial.Length-1].Equals("txt"))
        {
            File.WriteAllText(chemin_bonus+"/"+NameBuilderText,GameBuilderText.text);
        }else
        {
            File.WriteAllText(chemin_bonus+"/"+NameBuilderText+".txt",GameBuilderText.text);
        }

        
    }

    public void SetReussite(int frame)
    {
        if(FromGameBuilder)return;

        player.niveauxFinis[levelNum-1] = true;
        player.chronoNiveau[levelNum-1] = frame;
        player.essaies[levelNum-1]++;
        
        WriteInJson();
    }

    public void SetRetour(int frame)
    {
        player.chronoNiveau[levelNum-1] += frame;
        player.essaies[levelNum-1]++;
        
        WriteInJson();
    }

    public Donnees GetData()
    {
        return data;
    }

    public void SetNiveauxBonus()
    {
        chemin_bonus = Application.persistentDataPath + "/NiveauxBonus";
        string jsonFile = File.ReadAllText(chemin_json);
        data = JsonConvert.DeserializeObject<Donnees>(jsonFile);
        
        RefreshNiveauxBonus();

        WriteInJson();
    }

    public void NiveauBonusSelected(TextAsset text)
    {
        GameBuilderText = text;
    }

    public void RefreshNiveauxBonus()
    {
        string [] files = System.IO.Directory.GetFiles(chemin_bonus);
        data.niveauxBonus = new List<NiveauxBonus>();

        foreach (string file in files)
        {
            if(file.Contains("txt"))
            {  
                string[] arr = file.Split('\\');
                data.niveauxBonus.Add(new NiveauxBonus(arr[1]));
            }
        }

        JoueursNiveauxBonus();

        WriteInJson();
    }

    private void JoueursNiveauxBonus()
    {
        Joueurs j = GetPlayer();

        foreach(NiveauxBonus niveaux in data.niveauxBonus)
        {
            if(j.niveauxBonusFinis.ContainsKey(niveaux.nom))break;

            j.niveauxBonusFinis.Add(niveaux.nom,false);
            j.indiceBonus.Add(niveaux.nom,false);
            j.essaiesBonus.Add(niveaux.nom,1);
            j.chronoNiveauBonus.Add(niveaux.nom,0);
        }
    }

    public void WriteInJson()
    {
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(chemin_json, json);
    }
}
