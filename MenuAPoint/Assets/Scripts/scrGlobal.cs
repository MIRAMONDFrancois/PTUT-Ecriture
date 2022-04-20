using System.Text;
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
    private DonneesBonus dataBonus;
    private Joueurs player;
    private string chemin_json;
    private string chemin_jsonBonus;
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
        chemin_jsonBonus = chemin + "/NiveauxBonus.json";
        chemin_txt = chemin + "/Resultats";
        chemin_bonus = chemin + "/NiveauxBonus";

        Directory.CreateDirectory(chemin_bonus);

        if(!File.Exists(chemin_json))
        {
            File.WriteAllText(chemin_json, "{}");
        }

        if(!File.Exists(chemin_jsonBonus))
        {
            File.WriteAllText(chemin_jsonBonus, "{}");
        }

        string jsonFile = File.ReadAllText(chemin_json);
        string jsonFileBonus = File.ReadAllText(chemin_jsonBonus);

        data = JsonConvert.DeserializeObject<Donnees>(jsonFile);
        dataBonus = JsonConvert.DeserializeObject<DonneesBonus>(jsonFileBonus);
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
        if(data.joueurs.ContainsKey(playerName))
        {
            return data.joueurs[playerName];
        }
        
        return CreationJoueur();
    }

    //Création Dossier + Ajout dans .json . Connexion [scrMenu]
    public Joueurs CreationJoueur()
    {
        Joueurs j = new Joueurs();

        j.joueur = playerName;
        
        data.joueurs.Add(playerName,j);

        WriteInJson(true);
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
            nbEssaiTab[a] = player.essaies[a];
            levelunlocked[a+1] = player.niveauxFinis[a];
        }
    }
    

    public void SetIntro()
    {
        player.intro = true;
        WriteInJson(true);
    }

    public void SetTuto()
    {
        player.tuto = true;
        WriteInJson(true);
        
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
                
        WriteInJson(true);
    }

    //Data en .txt dans Resultats. SceneTest [scrTextManager]
    public void SetTexteFichier(string recap)
    {
        if(FromGameBuilder)return;

        string chemin = chemin_txt+"/"+playerName;

        if(FromBonusLevel)
        {
            chemin += "/NiveauxBonus";
            Directory.CreateDirectory(chemin);
            chemin += "/"+NameBuilderText;

            if(File.Exists(chemin))
            {
                StringBuilder sb =  new StringBuilder(File.ReadAllText(chemin));
                sb.Append("\n");
                sb.Append(recap);
                File.WriteAllText(chemin,sb.ToString());
                return;
            }

            File.WriteAllText(chemin, recap);
            return;
        }


        chemin += "/NiveauxClassiques";
        Directory.CreateDirectory(chemin);
        chemin += "/Niveau_"+levelNum;

        if(File.Exists(chemin))
        {
            StringBuilder sb =  new StringBuilder(File.ReadAllText(chemin));
            sb.Append("\n");
            sb.Append(recap);
            File.WriteAllText(chemin,sb.ToString());
            return;
        }

        File.WriteAllText(chemin, recap);
        return;
    }

    public NiveauxBonus GetBonusLevel()
    {
        if(dataBonus.niveaux.ContainsKey(NameBuilderText))
        {
            return dataBonus.niveaux[NameBuilderText];
        }

        //new niveau
        string[] trial = NameBuilderText.Split('.');

        if(!trial[trial.Length-1].Equals("txt"))
        {
            NameBuilderText += ".txt";
        }
        NiveauxBonus niveau = new NiveauxBonus(NameBuilderText);

        if(NameBuilderText.Equals("NomDuTexte.txt"))return niveau;
        
        dataBonus.niveaux.Add(NameBuilderText,niveau);
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

        WriteInJson(false);

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

        WriteInJson(true);
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

        WriteInJson(true);
    }

    public Donnees GetData()
    {
        return data;
    }

    public DonneesBonus GetDataBonus()
    {
        return dataBonus;
    }

    public void NiveauBonusSelected(TextAsset text)
    {
        GameBuilderText = text;
    }

    public void RefreshNiveauxBonus()
    {
        string [] files = System.IO.Directory.GetFiles(chemin_bonus);
        
        //List<NiveauxBonus> newBonus = new List<NiveauxBonus>();
        Dictionary<string, NiveauxBonus> newBonus = new Dictionary<string, NiveauxBonus>();

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
                newBonus.Add(NameBuilderText,GetBonusLevel());
            }
        }
        dataBonus.niveaux = newBonus;

        if(FromGameBuilder)return;
        JoueursNiveauxBonus();
    }

    private void JoueursNiveauxBonus()
    {
        Joueurs j = GetPlayer();

        foreach(string keys in dataBonus.niveaux.Keys)
        {
            if(!j.niveauxBonusFinis.ContainsKey(keys))
            {
                j.niveauxBonusFinis.Add(keys,false);
                j.indiceBonus.Add(keys,false);
                j.essaiesBonus.Add(keys,0);
                j.chronoNiveauBonus.Add(keys,0);
            }
        }
    }

    public void WriteInJson(bool pourJoueur)//true = joueur // false = bonus 
    {
        string json;

        if(pourJoueur)
        {
            json = JsonConvert.SerializeObject(data);
            File.WriteAllText(chemin_json, json);
        }else
        {
            json = JsonConvert.SerializeObject(dataBonus);
            File.WriteAllText(chemin_jsonBonus, json);
        }
    }
}
