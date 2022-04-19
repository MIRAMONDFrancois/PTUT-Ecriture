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
    public List<int> nbEssaiTab = new List<int>();

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

    [Header("Scene de fin")]
    public int nbrDrag = 0;//Check nbr elements qui ont été drag & drop.
    public List<bool> endSceneItemsCanMove = new List<bool>();//List pour savoir si l'item peut ou non être drag & drop

    //GameBuilder
    public bool FromGameBuilder;
    public bool FromBonusLevel;
    public TextAsset GameBuilderText;
    public string NameBuilderText;

    void Start()
    {  
        InitJson();
        setLevelUnlocked();
        setEndSceneItemsCanMove();
        setNbEssaiTab();
        nbrDrag = 0;
    }

    void Awake() {
        if(Instance != null)
        {
            Destroy(this);
        }
        
        Instance = this;
    }

    private void InitJson()
    {

        string chemin = Application.persistentDataPath;

        #if UNITY_STANDALONE_OSX
            chemin = Application.streamingAssetsPath;
        #endif

        chemin_json = chemin + "/donnees.json";
        chemin_txt = chemin + "/Resultats";
        chemin_bonus = chemin + "/NiveauxBonus";

        Directory.CreateDirectory(chemin_bonus);

        if(!File.Exists(chemin_json))
        {
            File.WriteAllText(chemin_json, "{\"donnees\": [],\"niveauxBonus\": []}");
        }

        string jsonFile = File.ReadAllText(chemin_json);

        data = JsonConvert.DeserializeObject<Donnees>(jsonFile);
    }

    public void setLevelUnlocked()
    {
        levelunlocked.Add(true);
        for (int i = 1; i <= 15; i++)
        {
            levelunlocked.Add(false);
        }
    }

    public void setNbEssaiTab()
    {
        nbEssaiTab.Add(0);
        for(int i = 1; i < 15; i++)
        {
            nbEssaiTab.Add(0);
        }
    }

    public void setEndSceneItemsCanMove()
    {
        endSceneItemsCanMove.Add(true);
        for (int i = 1; i <= 10; i++)
            endSceneItemsCanMove.Add(true);
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

        if(FromBonusLevel)
        {
            return player.chronoNiveauBonus[NameBuilderText];
        }

        return player.chronoNiveau[levelNum-1];
    }

    //Pour Indice. SceneTest [scrIndice]
    public bool GetIndice()
    {
        if(FromGameBuilder || FromBonusLevel)return false;
        return player.indiceNiveau[levelNum-1];
    }

    //Pour Indice. SceneTest [scrIndice]
    public void SetIndice()
    {
        if(FromGameBuilder)return;
        
        if(FromBonusLevel)
        {
            player.indiceBonus[NameBuilderText] = true;
            return;
        }

        player.indiceNiveau[levelNum-1]=true;
        player.indiceRestant = nbIndices;
                
        WriteInJson();
    }

    //Data en .txt dans Resultats. SceneTest [scrTextManager]
    public void SetTexteFichier(string recap)
    {
        if(FromGameBuilder)return;

        string chemin = chemin_txt+"/"+playerName;

        if(FromBonusLevel)
        {
            chemin += "/NiveauxBonus/"+NameBuilderText.Split('.')[0];
            Directory.CreateDirectory(chemin);

            File.WriteAllText(chemin+"/Essaie_"+player.essaiesBonus[NameBuilderText]+".txt",recap);
            return;
        }


        
        chemin += "/Niveau_"+levelNum;
        Directory.CreateDirectory(chemin);

        File.WriteAllText(chemin+"/Essaie_"+player.essaies[levelNum-1]+".txt",recap);
    }

    public NiveauxBonus GetBonusLevel()
    {
        foreach(NiveauxBonus niveaux in data.niveauxBonus)
        {
            
            if(niveaux.nom.Equals(NameBuilderText))
            {
                print("existe");
                return niveaux;
            }
        }
        string[] trial = NameBuilderText.Split('.');

        if(!trial[trial.Length-1].Equals("txt"))
        {
            NameBuilderText += ".txt";
        }
        NiveauxBonus niveau = new NiveauxBonus(NameBuilderText);

        if(NameBuilderText.Equals("NomDuTexte.txt"))return niveau;
        
        data.niveauxBonus.Add(niveau);
        print("new");
        return niveau;
    }

    public void CreateTexteBuilder()
    {
        NiveauxBonus niveauxBonus = GetBonusLevel();
        

        niveauxBonus.totalPonct[0] = pointLimit;
        niveauxBonus.totalPonct[1] = virguleLimit;
        niveauxBonus.totalPonct[2] = exclamationLimit;
        niveauxBonus.totalPonct[3] = interrogationLimit;
        niveauxBonus.totalPonct[4] = deuxpointsLimit;
        niveauxBonus.totalPonct[5] = pointvirguleLimit;

        WriteInJson();

        //create .txt
        File.WriteAllText(chemin_bonus+"/"+NameBuilderText,GameBuilderText.text);
    }

    public void SetReussite(int frame)
    {
        if(FromGameBuilder)return;

        if(FromBonusLevel)
        {
            player.niveauxBonusFinis[NameBuilderText] = true;
            player.chronoNiveauBonus[NameBuilderText] = frame;
            player.essaiesBonus[NameBuilderText]++;
        }else
        {
            player.niveauxFinis[levelNum-1] = true;
            player.chronoNiveau[levelNum-1] = frame;
            player.essaies[levelNum-1]++;
        }

        WriteInJson();
    }

    public void SetRetour(int frame)
    {
        if(FromBonusLevel)
        {
            player.chronoNiveauBonus[NameBuilderText] += frame;
            player.essaiesBonus[NameBuilderText]++;
        }else
        {
            player.chronoNiveau[levelNum-1] += frame;
            player.essaies[levelNum-1]++;
        }

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
        
        List<NiveauxBonus> newBonus = new List<NiveauxBonus>();
        foreach (string file in files)
        {
            //only read txt
            if(file.Contains("txt"))
            {  
                #if (UNITY_STANDALONE_WIN || UNITY_EDITOR)
                    string[] arr = file.Split('\\');    
                #else
                    string[] arr = file.Split('/');
                #endif
            
                NameBuilderText = arr[arr.Length-1]; 
                newBonus.Add(GetBonusLevel());
            }
        }
        data.niveauxBonus = newBonus;

        if(FromGameBuilder)return;
        JoueursNiveauxBonus();
    }

    private void JoueursNiveauxBonus()
    {
        Joueurs j = GetPlayer();

        foreach(NiveauxBonus niveaux in data.niveauxBonus)
        {
            if(!j.niveauxBonusFinis.ContainsKey(niveaux.nom))
            {
                j.niveauxBonusFinis.Add(niveaux.nom,false);
                j.indiceBonus.Add(niveaux.nom,false);
                j.essaiesBonus.Add(niveaux.nom,1);
                j.chronoNiveauBonus.Add(niveaux.nom,0);
            }
        }
    }

    public void WriteInJson()
    {
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(chemin_json, json);
    }
}
