using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class scrTextManager : MonoBehaviour
{
    [Header("Text File")]
    public TextAsset TextFile;

    [Header("Prefabs")]
    public GameObject WordPrefab;
    public GameObject SlotPrefab;

    [Header("Canvas")]
    public GameObject canvas;
    public GameObject ButtonLayer;
    public GameObject cursor;
    public GameObject fondu;
    public GameObject clients;
    public GameObject client_virgule;
    public bool showLog;
    public Text animationLog;

    [Header("Position Text (en pourcentage)")]
    public float debut_text_gauche;
    public float marge_text_droite; 
    public float taille_police;
    public float debut_hauteur_dual;
    public float debut_hauteur_normal;
    public float hauteur_table;
    


    // Cursor
    private bool movingCursor;
    [HideInInspector]
    public bool canTouchPonct;
    private int cursorSpeed = 20;
    private int lineCursor = 0;
    private float lineToStop = 0;
    private float lineNumber = 0;
    private float posToStop;
    private Vector3 cursorStart;

    // Words
    private List<string> s; // list of words and separators
    private List<string> words; // list of words
    [HideInInspector]
    public GameObject[] slots; // list of slots
    [HideInInspector]
    public string[] separators; // list of the content of the slots
    [HideInInspector]
    public GameObject[] wordsObj; // list of the objects of the words

    private string correctText; // correct string
    private string currentText; // string composed of the words and separators

    [Header("Limited Ponct (-1 = infinite)")]
    public int pointLimit;
    public int virguleLimit;
    public int exclamationLimit;
    public int interrogationLimit;
    public int deuxpointsLimit;
    public int pointvirguleLimit;

    [Header("Dual Animation mode")]
    public bool dualAnim;
    public TextAsset CorrectFile;
    public bool canBeMoved;
    public bool canBeDeleted;
    public GameObject animationObj;


    // Text pos
    public GameObject text_scaler;
    public float scaler_x;//pour affichage slot
    public float scaler_y;//pour affichage slot
    private float lineWidth;
    private float textFloor;
    private float spaceSize;
    private float lineJump;
    private float taillePolice;
    private float text_sol;

    //list mots et ponctuations
    private List<string> vrai_nomspropres = new List<string>();//a mettre dans le globale //du détail pour vérifier nom
    private List<string> vrai_separators;//pour la vérification
    private List<List<string>> liste_separators;
    private List<string> vrai_mots;//pour écrire
    private List<string> vrai_slot;//pour vérification
    //liste gameobject
    private List<GameObject> vrai_slots_GO;//pour le jeu
    private List<GameObject> vrai_mots_GO;//pour le jeu

    //fin de phrase
    private bool pointTropTot = false;
    private bool manquePoint = false;
    private bool mauvaisPoint = false;//mauvaise fin ponctuation

    //mileu de phrase
    private bool tropVirgule = false;
    private bool pasAssezVirgule = false;
    private bool mauvaiseVirgule = false;//mauvaise position
    private bool pasbonneVirgule = false;//mauvaise mid ponctuation

    private bool textreussite = false; //pour pas faire "!bool && !bool" à chaque fois
    private bool point_reussite = false; 
    private bool virgule_reussite = false;

    // Data Export
    private string fullFolderName;
    private string recapContent;
    private int frames;
    private int errorNum;


    // Start is called before the first frame update
    void Start()
    {
        vrai_nomspropres.Add("Tokki");
        vrai_nomspropres.Add("Pesto");

        // DATA IMPORT
        scrGlobal globalScript = GameObject.Find("Global").GetComponent<scrGlobal>();

        TextFile = globalScript.file;
        CorrectFile = globalScript.animTextFile;
        dualAnim = globalScript.nivAntiOubli;
        pointLimit = globalScript.pointLimit;
        virguleLimit = globalScript.virguleLimit;
        exclamationLimit = globalScript.exclamationLimit;
        interrogationLimit = globalScript.interrogationLimit;
        deuxpointsLimit = globalScript.deuxpointsLimit;
        pointvirguleLimit = globalScript.pointvirguleLimit;

        debut_text_gauche = globalScript.debut_text_gauche/100f;
        marge_text_droite = globalScript.marge_text_droite/100f;
        taille_police = globalScript.taille_police/100f;
        debut_hauteur_dual = globalScript.debut_hauteur_dual/100f;
        debut_hauteur_normal = globalScript.debut_hauteur_normal/100f;
        hauteur_table = globalScript.hauteur_table/100f;

        init_taille_texte(); 

        canTouchPonct = true;
        s = new List<string>();
        words = new List<string>();        
        cursor.gameObject.SetActive(!dualAnim);

        if(dualAnim)
        {
            CutsWordsDual(CorrectFile);
        }else
        {
            CutsWordsDual(TextFile);
            cursorStart = new Vector3(Screen.width*debut_text_gauche-cursor.GetComponent<RectTransform>().sizeDelta.x/2,textFloor,0);
            cursor.transform.position = cursorStart;
        }
        
        

        // creates separators list
        separators = new string[words.Count];
        for (int i = 0; i < separators.Length; i++) separators[i] = "";
        wordsObj = new GameObject[words.Count];
        slots = new GameObject[words.Count];
        
        placesWords();
        if(dualAnim)
        {
            depart_block(TextFile);
        }

        if (!dualAnim) {

            animationLog.gameObject.SetActive(showLog);
            animationLog.text = "Les clients ont hâte de manger votre plat !";

        } else {
            animationObj.gameObject.SetActive(true);
            animationObj.GetComponent<Animator>().SetInteger("Niveau",globalScript.levelNum);
            animationLog.gameObject.SetActive(false);
        } 

        // DATA EXPORT
        string pn = globalScript.playerName;
        int ln = globalScript.levelNum;
        string folderName = System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year;
        fullFolderName = "./RESULTATS/" + folderName + "/" + pn;

        if (!Directory.Exists ("./RESULTATS")) {
            Directory.CreateDirectory ("./RESULTATS");
            Debug.Log("created result folder");
        }
        if (!Directory.Exists ("./RESULTATS/" + folderName)) {
            Directory.CreateDirectory ("./RESULTATS/" + folderName);
            Debug.Log("created date folder");
        }
        if (!Directory.Exists (fullFolderName)) {
            Directory.CreateDirectory (fullFolderName);
            Debug.Log("created player folder");
        }
        recapContent = "Fichier de " + pn + " sur le niveau " + ln + ".\nLe texte correct est:\n" + correctText + "\n";
        recapContent += "\n----------------------------------------------\n";
        //System.IO.File.WriteAllText(fullFolderName + "/NiveauX.txt", pn + " est un.e gentil.le élève.");
        // way to get the text:
        //Debug.Log(System.IO.File.ReadAllText("./DOSSIER/" + pn + ".txt"));


        HideSlots();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (movingCursor)
        {
            
            Vector3 trans = cursor.transform.position;
            trans.x += cursorSpeed;

            if ( trans.x > Screen.width*(1f-marge_text_droite))
            {   
                trans.x = cursorStart.x;
                trans.y -= lineJump;
                lineCursor++;
            }

            if (lineCursor >= lineToStop)
            {
                // last line
                if (trans.x > posToStop)
                {
                    // play animations
                    movingCursor = false;
                    
                    //prhase correcte
                    if (textreussite)
                    {
                        switch (Random.Range(1, 4))
                        {
                            case 1:
                                animationLog.text = "Les clients ont trouvé ça parfait !";
                                break;
                            case 2:
                                animationLog.text = "Tout le monde a trouvé ça délicieux !";
                                break;
                            case 3:
                                animationLog.text = "Ce plat était vraiment au point !";
                                break;
                        }
                        // writes on the .txt
                        
                        
                        // Hides all buttons and shows the "continue" one, which is the first child
                        ButtonLayer.SetActive(true);
                        ButtonLayer.transform.GetChild(0).gameObject.SetActive(true);
                        for (int i = 1; i < ButtonLayer.transform.childCount; i++) {
                            ButtonLayer.transform.GetChild(i).gameObject.SetActive(false);
                        }

                        // Unlocks next level
                        scrGlobal globalScript = GameObject.Find("Global").GetComponent<scrGlobal>();
                        globalScript.levelunlocked[globalScript.levelNum] = true;
                    }
                    // recap phrase for the animation recall
                    else
                    {
                        string recap = "";
                        switch (Random.Range(1, 4))
                        {
                            case 1:
                                recap = "Les clients ont trouvé ça";
                                break;
                            case 2:
                                recap = "Ils pensent que c'est";
                                break;
                            case 3:
                                recap = "Votre plat est peut-être";
                                break;
                        }
                        recapContent += "\nErreur à " + frames/60 + " secondes :\n";

                        if (tropVirgule) { recap += " trop épicé"; recapContent += "- trop de virgule\n"; }
                        if (pasAssezVirgule) { recap += " un peu fade"; recapContent += "- pas assez de virgule\n"; }
                        if (mauvaiseVirgule) { recap += " bizarrement épicé"; recapContent += "- mauvais placement de virgule\n"; }
                        if (pasbonneVirgule) { recap += " différent"; recapContent += "- mauvaise virgule\n"; }
                        if ((!virgule_reussite) && (!point_reussite)) { recap += " et"; }
                        if (pointTropTot) { recap += " trop léger."; recapContent += "- point trop tôt\n"; }
                        if (manquePoint) { recap += " trop lourd."; recapContent += "- manque d'un point\n"; }
                        if (mauvaisPoint) { recap += " étrange."; recapContent += "- mauvais point\n"; }
                        animationLog.text = recap;

                        // data recap
                        ButtonLayer.SetActive(true);

                        canTouchPonct = true;
                    }
                    texte_data();
                    //Début animation après fin validation
                    AnimationFondu();
                }
            }
            // cursor gets back to original position
            cursor.transform.position = trans;

        } else {
            // counts frames (for the timer) when the cursor isn't moving, to be fair
            frames++;
        }
        //anim curseur miam
        cursor.GetComponent<Animator>().SetBool("Validation",movingCursor);
    }

     private void CutsWordsDual(TextAsset TF)
    {
        vrai_separators = new List<string>();
        vrai_mots = new List<string>();
        vrai_slot = new List<string>();
        
        string v_mots = "";
        bool v_skip = false;
        bool v_tiret = false;
        bool toLower = false;
        //condition autre possibilité pour le texte
        bool cond1 = false;
        bool cond2 = false;
        bool cond3 = false;
        bool cond4 = false;

        for (int i = 0; i < TF.text.Length; i++)
        {
            switch (TF.text[i])
            {
                case ','://lettre_ponct_espace
                    vrai_separators.Add(",");
                    vrai_slot.Add("");
                    vrai_mots.Add(v_mots);
                    v_skip = true;
                    v_mots="";
                    break;
                case '.'://lettre_ponct_espace ou lettre_ponct_retourligne
                    vrai_separators.Add(".");
                    vrai_slot.Add("");
                    vrai_mots.Add(v_mots);
                    v_skip = true;
                    v_mots="";
                    toLower = true;
                    break;
                case '!'://espace_ponct_espace ou espace_ponct_retourligne
                    vrai_separators[vrai_separators.Count-1]="!";
                    v_skip = true;
                    toLower = true;
                    break;
                case '?'://espace_ponct_espace ou espace_ponct_retourligne
                    vrai_separators[vrai_separators.Count-1]="?";
                    v_skip = true;
                    toLower = true;
                    break;
                case ':'://espace_ponct_espace ou espace_ponct_retourligne
                    vrai_separators[vrai_separators.Count-1]=":";
                    v_skip = true;
                    break;
                case ';'://espace_ponct_espace
                    vrai_separators[vrai_separators.Count-1]=";";
                    v_skip = true;
                    break;
                case '-'://lettre_tiret_lettre ou tiret_espace_lettre
                    v_mots += "-";
                    v_tiret = true;
                    cond1 = false;
                    break;
                case '\n'://ponct_retourligne
                    //vrai_mots[vrai_mots.Count-1]+="\n";
                    v_skip = false;
                    v_mots="\n";
                    cond1 = true;
                    if(cond3)
                    {
                        v_skip = true;
                        cond3=false;
                        cond4=true;
                    }
                    break;
                case '/':
                    if(cond1){
                        cond1 = false;
                        cond2 = true;
                        break;
                    }
                    if(cond2)
                    {
                        cond2 = false;
                        cond3 = true;
                        break;
                    }
                    //lettre
                    Debug.Log("coucou");
                    v_mots += TF.text[i];

                    if(toLower)
                    {
                        v_mots = v_mots.ToLower();
                        toLower = false;
                    }
                    v_tiret = false;
                break;
                case ' '://espace_ponct_espace ou lettre_espace ou ponct_espace
                    
                    if(v_tiret)
                    {
                        v_tiret = false;
                        v_mots += " ";
                    }
                    else if(v_skip)
                    {
                        v_skip=false;
                    }
                    else
                    {
                        vrai_separators.Add("");
                        vrai_slot.Add("");
                        vrai_mots.Add(v_mots);
                        v_mots = "";
                    }
                    
                    break;
                default://lettre
                    cond1 = false;
                    v_mots += TF.text[i];

                    if(toLower)
                    {
                        v_mots = v_mots.ToLower();
                        toLower = false;
                    }
                    v_tiret = false;
                    break;
            }
            if(cond4)break;
        }
        if(!v_skip)
        {
            vrai_mots.Add(v_mots);
        }

        //verif noms
        for(int a=0;a<vrai_mots.Count;a++)
        {
            string verif_mot = vrai_mots[a].ToLower();
            for(int b=0;b<vrai_nomspropres.Count;b++)
            {
                string verif_nom = vrai_nomspropres[b].ToLower();
                if(verif_mot.Equals(verif_nom))vrai_mots[a] = verif_mot.Substring(0, 1).ToUpper() + verif_mot.Substring(1, verif_mot.Length - 1);
            }
        }

        autre_solution(TF);
    }

    private void autre_solution(TextAsset TF)
    {
        List<string> liste_ponct = new List<string>();
        liste_separators = new List<List<string>>();

        bool v_skip = false;
        bool v_tiret = false;
        //autre solution
        bool cond1 = false;
        bool cond2 = false;
        bool cond3 = false;
        bool cond4 = false;

        //recherche ponct à mettre de base
        for(int a=0;a<TF.text.Length;a++)
        {
            switch(TF.text[a])
            {
                case '.':
                    liste_ponct.Add(".");
                    v_skip = true;
                break;
                case '!':
                    liste_ponct[liste_ponct.Count-1]="!";
                    v_skip = true;
                break;
                case '?':
                    liste_ponct[liste_ponct.Count-1]="?";
                    v_skip = true;
                break;
                case ',':
                    liste_ponct.Add(",");
                    v_skip = true;
                break;
                case ';':
                    liste_ponct[liste_ponct.Count-1]=";";
                    v_skip = true;
                break;
                case ':':
                    liste_ponct[liste_ponct.Count-1]=":";
                    v_skip = true;
                break;
                case '-':
                    v_tiret = true;
                    cond1 = false;
                break;
                case '\n':
                    v_skip = false;
                    cond1 = true;
                    if(cond3)
                    {
                        v_skip = true;
                        cond3 = false;
                        cond4=true;
                    }
                break;
                case '/':
                    if(cond1){
                        cond1 = false;
                        cond2 = true;
                        break;
                    }
                    if(cond2)
                    {
                        cond2 = false;
                        cond3 = true;
                        break;
                    }
                    v_tiret = false;
                break;
                case ' ':
                    if(v_tiret)
                    {
                        v_tiret = false;
                    }
                    else if(v_skip)
                    {
                        v_skip=false;
                    }
                    else
                    {
                        liste_ponct.Add("");
                    }
                break;
                default:
                    v_tiret = false;
                break;

            }
            if(cond4)
            {
                liste_separators.Add(liste_ponct);
                liste_ponct = new List<string>();

                v_skip = false;
                v_tiret = false;
                cond4 = false;
            }
        }
        liste_separators.Add(liste_ponct);
    }

    private void depart_block(TextAsset TF)//Switch Land
    {
        List<string> liste_ponct = new List<string>();
        
        bool v_skip = false;
        bool v_tiret = false;

        //recherche ponct à mettre de base
        for(int a=0;a<TF.text.Length;a++)
        {
            switch(TF.text[a])
            {
                case '.':
                    liste_ponct.Add(".");
                    v_skip = true;
                break;
                case '!':
                    liste_ponct[liste_ponct.Count-1]="!";
                    v_skip = true;
                break;
                case '?':
                    liste_ponct[liste_ponct.Count-1]="?";
                    v_skip = true;
                break;
                case ',':
                    liste_ponct.Add(",");
                    v_skip = true;
                break;
                case ';':
                    liste_ponct[liste_ponct.Count-1]=";";
                    v_skip = true;
                break;
                case ':':
                    liste_ponct[liste_ponct.Count-1]=":";
                    v_skip = true;
                break;
                case '-':
                    v_tiret = true;
                break;
                case '\n':
                    v_skip = false;
                break;
                case ' ':
                    if(v_tiret)
                    {
                        v_tiret = false;
                    }
                    else if(v_skip)
                    {
                        v_skip=false;
                    }
                    else
                    {
                        liste_ponct.Add("");
                    }
                break;
                default:
                    v_tiret = false;
                break;

            }
        }

        for(int a=0;a<liste_ponct.Count;a++)
        {
            switch(liste_ponct[a])
            {
                case ".":
                    GameObject.Find("Point Gen").GetComponent<scrBlockGenerator>().demarrageBlock(vrai_slots_GO[a]);
                break;
                case "!":
                    GameObject.Find("Exclamation Gen").GetComponent<scrBlockGenerator>().demarrageBlock(vrai_slots_GO[a]);
                break;
                case "?":
                    GameObject.Find("Interrogation Gen").GetComponent<scrBlockGenerator>().demarrageBlock(vrai_slots_GO[a]);
                break;
                case ",":
                    GameObject.Find("Virgule Gen").GetComponent<scrBlockGenerator>().demarrageBlock(vrai_slots_GO[a]);
                break;
                case ":":
                    GameObject.Find("Deux Points Gen").GetComponent<scrBlockGenerator>().demarrageBlock(vrai_slots_GO[a]);
                break;
                case ";":
                    GameObject.Find("Point Virgule Gen").GetComponent<scrBlockGenerator>().demarrageBlock(vrai_slots_GO[a]);
                break;
                default:
                break;
            }
        }
    }

    private void placesWords()
    {

        vrai_slots_GO = new List<GameObject>();
        vrai_mots_GO = new List<GameObject>();
        

        //taille police
        WordPrefab.GetComponentInChildren<TextMeshProUGUI>().fontSize=taillePolice;
        WordPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().fontSize=taillePolice;

        //var
        bool alaligne = false;
        float total_width = 0;
        float total_height = 0;
        //boucle positionner chaque mots    
        for (int i = 0; i < vrai_mots.Count; i++)
        {
            
            //init chaque mot
            GameObject slot = Instantiate(SlotPrefab);
            GameObject wordObj = Instantiate(WordPrefab);
            wordObj.GetComponentInChildren<TextMeshProUGUI>().text = vrai_mots[i];
            wordObj.GetComponent<MotsGO>().nom_propre = true;

            //maj position 
            for(int a=0;a<vrai_mots[i].Length;a++)
            {
                if(!vrai_mots[i][a].Equals('-') && !vrai_mots[i][a].Equals(' '))
                {
                    wordObj.GetComponent<MotsGO>().maj_pos = a;
                    break;
                }
            }

            //verif nom propre et mot en maj
            if(vrai_mots[i].Equals(vrai_mots[i].ToLower()))wordObj.GetComponent<MotsGO>().nom_propre = false;
            
            float pw = wordObj.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;
            
            alaligne = vrai_mots[i][0].Equals('\n');

            if (total_width + pw + spaceSize > lineWidth || alaligne)
            {
                if(alaligne)
                {
                    wordObj.GetComponentInChildren<TextMeshProUGUI>().text =  vrai_mots[i].Substring(1, vrai_mots[i].Length - 1);
                }
                total_width = 0f;
                total_height -= lineJump;
                
                lineNumber++;
            }

            //positions des mots et slots
            

            total_width+=pw/2;
            wordObj.transform.position = new Vector3(Screen.width*debut_text_gauche+total_width, textFloor + total_height, 0);
            total_width+=spaceSize/2+pw/2;
            slot.transform.position = new Vector3(Screen.width*debut_text_gauche+total_width, textFloor + total_height, 0);
            slot.transform.GetComponent<scrSlot>().ligne=lineNumber;
            slot.transform.GetComponent<scrSlot>().INDEX=i;
            slot.transform.GetComponent<scrSlot>().pos_origine=slot.transform.position;
            total_width+=spaceSize/2;

            //afficher sur le canvas
            wordObj.transform.SetParent(canvas.transform);
            slot.transform.SetParent(canvas.transform);

            vrai_slots_GO.Add(slot);
            vrai_mots_GO.Add(wordObj);
            
            

        } // end of word placement

        if(vrai_mots_GO[vrai_mots_GO.Count-1].transform.position.y < text_sol)
        {
            replaceWords();
        }
    }

    public void vrai_valider()
    {
        bool flag_debug = true;
        //var de vérification
        int nb_midponct_joueur = 0; //pour savoir si trop ou pas assez
        int nb_midponct_verif = 0;

        //reset valeur pour animation
        pointTropTot = false;
        manquePoint = false;
        mauvaisPoint = false;

        pasAssezVirgule = false;
        tropVirgule = false;
        mauvaiseVirgule = false;
        pasbonneVirgule = false;

        //verification
        List<bool> autre_reussite = new List<bool>();//liste flag

        
        for(int a=0;a<liste_separators.Count;a++)
        {
            bool reussite = true;//flag

            for(int b=0;b<liste_separators[a].Count;b++)
            {
                if(!vrai_slots_GO[b].GetComponent<scrSlot>().ponctuation.Equals(liste_separators[a][b]))reussite=false;
            }
            
            autre_reussite.Add(reussite);
        }

        for(int a=0;a<autre_reussite.Count;a++)
        {
            if(autre_reussite[a])textreussite=true;
        }

        if(textreussite)
        {
            deplacement_cursor(vrai_slots_GO.Count-1);
            return;
        }
        //boucle de vérification
        for(int a=0;a<vrai_separators.Count;a++)
        {
            string slot_joueur = vrai_slots_GO[a].GetComponent<scrSlot>().ponctuation;
            string slot_verif = vrai_separators[a];

            
            if(!slot_joueur.Equals(slot_verif))
            {
                //slot sans ponctuation 
                if(slot_joueur.Equals(""))
                {
                    //au lieu de mid ponct
                    if(slot_verif.Equals(",") || slot_verif.Equals(":") || slot_verif.Equals(";"))
                    {
                        nb_midponct_verif++;
                    }
                    //au lieu de fin ponct
                    else if(slot_verif.Equals(".") || slot_verif.Equals("?") || slot_verif.Equals("!"))
                    {

                        manquePoint = true;

                        if(nb_midponct_verif>nb_midponct_joueur)
                        {
                            pasAssezVirgule = true;
                        }
                        else if(nb_midponct_verif<nb_midponct_joueur)
                        {
                            tropVirgule = true;
                        }
                    }
                }
                //slot mid ponctuation
                else if(slot_joueur.Equals(",") || slot_joueur.Equals(":") || slot_joueur.Equals(";"))
                {
                    nb_midponct_joueur++;
                    //au lieu d'un vide
                    if(slot_verif.Equals(""))
                    {
                        mauvaiseVirgule = true;
                    }
                    //au lieu de fin ponct
                    else if(slot_verif.Equals(".") || slot_verif.Equals("?") || slot_verif.Equals("!"))
                    {
                        manquePoint = true;
                        mauvaiseVirgule = true;
                        
                        if(nb_midponct_verif>nb_midponct_joueur)
                        {
                            pasAssezVirgule = true;
                        }
                        else if(nb_midponct_verif<nb_midponct_joueur)
                        {
                            tropVirgule = true;
                        }
                    }
                    //mais mauvaise mid ponctuation
                    else
                    {
                        pasbonneVirgule = true;
                        nb_midponct_verif++;
                    }
                }

                //slot fin ponctuation
                else
                {
                    //au lieu d'un vide
                    if(slot_verif.Equals(""))
                    {
                        pointTropTot = (true != manquePoint);
                    }
                    //au lieu de mid ponct
                    else if(slot_verif.Equals(",") || slot_verif.Equals(":") || slot_verif.Equals(";"))
                    {
                        pointTropTot = (true != manquePoint);
                        nb_midponct_verif++;
                    }
                    else
                    {
                        mauvaisPoint = true;
                    }

                    //verification nb mid ponct
                    if(nb_midponct_verif>nb_midponct_joueur)
                    {
                        pasAssezVirgule = true;
                    }
                    else if(nb_midponct_verif<nb_midponct_joueur)
                    {
                        tropVirgule = true;
                    }

                    
                    deplacement_cursor(a);

                    return;
                }
            }

            pasAssezVirgule = nb_midponct_joueur < nb_midponct_verif;
            tropVirgule = nb_midponct_joueur > nb_midponct_verif;
        }

        deplacement_cursor(vrai_slots_GO.Count-1);
    }

    private void deplacement_cursor(int pos)
    {
        point_reussite = !pointTropTot && !manquePoint && !mauvaisPoint;
        virgule_reussite = !tropVirgule && !pasAssezVirgule && !mauvaiseVirgule && !pasbonneVirgule;
        textreussite = (point_reussite && virgule_reussite) || textreussite;

        animationLog.text = "Les clients sont en train de tester votre plat...";

        lineToStop = vrai_slots_GO[pos].GetComponent<scrSlot>().ligne;
        posToStop = vrai_slots_GO[pos].transform.position.x-(cursor.transform.GetComponent<RectTransform>().sizeDelta.x/4);
        

        movingCursor = true;
        canTouchPonct = false;
        cursor.transform.position = cursorStart;
        lineCursor = 0;
        cursor.transform.SetAsLastSibling();
        ButtonLayer.SetActive(false);
    }

    public void replaceWords()
    {
        //globale
        taillePolice -= 5;
        lineNumber = 0;
        //fonction
        bool alaligne = false;
        float total_width = 0;
        float total_height = 0;

        text_scaler.SetActive(true);

        float scale_x = text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;
        float scale_y = text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredHeight;
        text_scaler.GetComponentInChildren<TextMeshProUGUI>().fontSize=taillePolice;
        scaler_x = text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredWidth / scale_x;
        scaler_y = text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredHeight / scale_y;

        if(dualAnim)
        {
            textFloor = Screen.height*debut_hauteur_dual - text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredHeight / 2;
        }
        else
        {
            textFloor = Screen.height*debut_hauteur_normal - text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredHeight / 2;
        }
        
        spaceSize = 1.8f*text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;
        lineJump = 1.2f*text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredHeight;

        text_scaler.SetActive(false);

        for(int a=0;a<vrai_mots_GO.Count;a++)
        {
            vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().fontSize=taillePolice;
            vrai_mots_GO[a].transform.GetChild(0).GetChild(1).GetComponentInChildren<TextMeshProUGUI>().fontSize=taillePolice;

            float pw = vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

            alaligne = vrai_mots[a][0].Equals('\n');

            if (total_width + pw > lineWidth || alaligne)
            {
                if(alaligne)
                {
                    vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().text =  vrai_mots[a].Substring(1, vrai_mots[a].Length - 1);
                }

                total_width = 0f;
                total_height -= lineJump;
                
                lineNumber++;
            }

            total_width+=pw/2;
            vrai_mots_GO[a].transform.position = new Vector3(Screen.width*debut_text_gauche+total_width, textFloor + total_height, 0);
            total_width+=spaceSize/2+pw/2;
            vrai_slots_GO[a].transform.position = new Vector3(Screen.width*debut_text_gauche+total_width, textFloor + total_height, 0);
            vrai_slots_GO[a].transform.GetComponent<scrSlot>().ligne=lineNumber;
            vrai_slots_GO[a].transform.GetComponent<scrSlot>().pos_origine=vrai_slots_GO[a].transform.position;
            total_width+=spaceSize/2;
            
        }

        if(vrai_mots_GO[vrai_mots_GO.Count-1].transform.position.y < text_sol)
        {
            replaceWords();
        }
    }

    public void majuscule(string tag, int slot_pos)
    {
        if(slot_pos==vrai_mots_GO.Count-1)return;

        
        if(tag.Equals("Point") || tag.Equals("Exclamation") || tag.Equals("Interrogation"))
        {
            if(!vrai_mots_GO[slot_pos+1].GetComponent<MotsGO>().nom_propre)
            {
                //ajout majuscule
                string mot = vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                int maj_pos = vrai_mots_GO[slot_pos+1].GetComponent<MotsGO>().maj_pos+1;
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mot.Substring(0, maj_pos).ToUpper() + mot.Substring(maj_pos, mot.Length - maj_pos);
                
                

                //triche majuscule animation
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = mot.Substring(0, maj_pos).ToUpper();
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

                //var raccourci
                float maj_width = vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().preferredWidth;
                float maj_height = vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().preferredHeight;
                float maj_max = Mathf.Max(maj_width,maj_height);
                
                //partie texte
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(maj_width,maj_height);
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(maj_width/2,0);
                
                
                //partie anim
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maj_max, maj_max);
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(maj_width/2,0);
                
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            
        }
        
        
    }

    public void demajuscule(string tag, int slot_pos)
    {
        if(slot_pos==vrai_mots_GO.Count-1)return;

        
        if(tag.Equals("Point") || tag.Equals("Exclamation") || tag.Equals("Interrogation"))
        {
            if(!vrai_mots_GO[slot_pos+1].GetComponent<MotsGO>().nom_propre)
            {
                string mot = vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mot.ToLower();

                //partie anim
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                vrai_mots_GO[slot_pos+1].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            
        }     
    }

    public void ValiderClick()
    {
        if (!dualAnim) vrai_valider();
        else ValiderDual();
    }

    public void ValiderDual() {

        bool dual_reussite = false;
        List<bool> autre_reussite = new List<bool>();//liste flag

        //verification
        for(int a=0;a<liste_separators.Count;a++)
        {
            bool reussite = true;//flag

            for(int b=0;b<liste_separators[a].Count;b++)
            {
                if(!vrai_slots_GO[b].GetComponent<scrSlot>().ponctuation.Equals(liste_separators[a][b]))reussite=false;
            }
            
            autre_reussite.Add(reussite);
        }

        for(int a=0;a<autre_reussite.Count;a++)
        {
            if(autre_reussite[a])dual_reussite=true;
        }

        animationObj.GetComponent<Animator>().SetBool("Validation",true);
        animationObj.GetComponent<Animator>().SetBool("Reussite",dual_reussite);
        
        if(dual_reussite)
        {
            ButtonLayer.SetActive(true);
            ButtonLayer.transform.GetChild(0).gameObject.SetActive(true);
            for (int i = 1; i < ButtonLayer.transform.childCount; i++) {
                ButtonLayer.transform.GetChild(i).gameObject.SetActive(false);
            }

            GameObject.Find("Global").GetComponent<scrGlobal>().levelunlocked[GameObject.Find("Global").GetComponent<scrGlobal>().levelNum] = true;
        }
        Invoke("dual_anim_reset",1f);
    }

    public void showIndice()
    {
        for(int a=0;a<vrai_separators.Count;a++)
        {
            if(!vrai_separators[a].Equals(""))
            {
                vrai_slots_GO[a].GetComponent<scrSlot>().showIndice();
            }
        }
    }

    public void ShowSlots(Vector2 taillePot, string pot)
    {
        Vector2 slot_pos = quel_pot(pot);

        for (int i = 0; i < vrai_slots_GO.Count; i++)
        {
            //pas afficher s'il y a déjà une ponctuation
            if(!vrai_slots_GO[i].GetComponentInChildren<scrSlot>().isUsed)
            {
                vrai_slots_GO[i].GetComponentInChildren<Image>().enabled = true;

                if(vrai_slots_GO[i].GetComponentInChildren<scrSlot>().indice)
                {
                    vrai_slots_GO[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                }
                

            }else
            {
                vrai_slots_GO[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }

            vrai_slots_GO[i].GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta = taillePot;
            vrai_slots_GO[i].GetComponentInChildren<BoxCollider2D>().size = taillePot;
            
            Vector3 V3_slot = vrai_slots_GO[i].GetComponentInChildren<RectTransform>().position;
            vrai_slots_GO[i].GetComponentInChildren<RectTransform>().position = new Vector3(V3_slot.x+slot_pos.x,V3_slot.y+slot_pos.y,V3_slot.z);
            
        }

        
    }

    public void HideSlots()
    {   
        for (int i = 0; i < vrai_slots_GO.Count; i++)
        {

            vrai_slots_GO[i].GetComponentInChildren<Image>().enabled = false;

            if(vrai_slots_GO[i].GetComponentInChildren<scrSlot>().isUsed && vrai_slots_GO[i].GetComponentInChildren<scrSlot>().indice)
            {
                vrai_slots_GO[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }

            vrai_slots_GO[i].GetComponentInChildren<RectTransform>().position = vrai_slots_GO[i].GetComponent<scrSlot>().pos_origine;
        }
    }

    public void whole_text()
    {
        currentText = "";
        string text_or_GO = "";
        for(int a=0;a<vrai_mots_GO.Count;a++)
        {

            if(vrai_mots[a].ToLower().Equals(vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().text.ToLower()))
            {
                text_or_GO = vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().text;
            }else
            {
                text_or_GO = vrai_mots[a];
            }

            if(!(vrai_slots_GO[a].GetComponent<scrSlot>().ponctuation.Equals(",") || vrai_slots_GO[a].GetComponent<scrSlot>().ponctuation.Equals(".")))
            {
                text_or_GO += " ";
            }
            currentText +=  text_or_GO + vrai_slots_GO[a].GetComponent<scrSlot>().ponctuation + " ";
        }
    }

    public void texte_data()
    {
        if(textreussite)
        {
            recapContent += "\nTerminé en " + frames/60 + " secondes avec " + errorNum + " erreur(s).";
            scrGlobal globalScript = GameObject.Find("Global").GetComponent<scrGlobal>();
            System.IO.File.WriteAllText(fullFolderName + "/Niveau"+globalScript.levelNum+".txt", recapContent);

        }else
        {
            whole_text();
            recapContent += currentText + "\n";
            recapContent += "\n----------------------------------------------\n";
            errorNum++;

        }
    }

    public void GoToMap() {
        SceneManager.LoadScene("MapScene");
    }

    public void AnimationFondu()
    {
        //affichage premier plan
        fondu.transform.SetAsLastSibling();
        

        //début fond noir
        fondu.SetActive(true);
        fondu.GetComponent<Animator>().SetBool("Actif",true);

        if(point_reussite && !virgule_reussite)
        {
            //saute l'animation curseur
            
            animation_virgule();
        }
        else
        {
            
            //animation curseur ->  .2 sec
            Invoke("animation_point",.2f);
        }

        
    }
    
    public void animation_point()
    {
        cursor.transform.SetAsLastSibling();

        cursor.GetComponent<Animator>().SetBool("Reussite",textreussite);
        cursor.GetComponent<Animator>().SetBool("Maigre",pointTropTot);
        cursor.GetComponent<Animator>().SetBool("Gros",manquePoint);
        cursor.GetComponent<Animator>().SetBool("Mauvaise",mauvaisPoint);
        
        //delai avant passage en fond et suite de l'animation des clients
        if(!point_reussite && virgule_reussite){
             Invoke("fondu_fin",1);
        }
        else
        {
             Invoke("animation_virgule",1);
        }
       
    }


    public void animation_virgule()
    {
        
        fondu.transform.SetAsLastSibling();
        clients.transform.SetAsLastSibling();

        client_virgule.SetActive(true);       

        client_virgule.GetComponent<Animator>().SetBool("Actif",true);
        client_virgule.GetComponent<Animator>().SetBool("Feu",tropVirgule);
        client_virgule.GetComponent<Animator>().SetBool("Berk",pasAssezVirgule);
        client_virgule.GetComponent<Animator>().SetBool("Confu",mauvaiseVirgule);
        client_virgule.GetComponent<Animator>().SetBool("Mauvaise",pasbonneVirgule);
        client_virgule.GetComponent<Animator>().SetBool("Reussite",textreussite);

        
        //Invoke("fin_animation",2);
        Invoke("fondu_fin",1);
    }

    public void fondu_fin()
    {  
        fondu.GetComponent<Animator>().SetBool("Actif",false);
        Invoke("fin_animation",1);
    }

    public void fin_animation()
    {   
        cursor.GetComponent<Animator>().SetBool("Maigre",false);
        cursor.GetComponent<Animator>().SetBool("Gros",false);
        cursor.GetComponent<Animator>().SetBool("Mauvaise",false);
        fondu.GetComponent<Animator>().SetBool("Actif",false);

        cursor.transform.position = cursorStart;
        
        //reset
        client_virgule.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0);
        client_virgule.GetComponent<RectTransform>().sizeDelta = new Vector2(478,844);
        client_virgule.GetComponent<Image>().color = new Color(1f,1f,1f);
        client_virgule.SetActive(false);
        fondu.SetActive(false); 
    }

    public void dual_anim_reset()
    {
        animationObj.GetComponent<Animator>().SetBool("Validation",false);
        animationObj.GetComponent<Animator>().SetBool("Reussite",false);
    }

    private Vector2 quel_pot(string pot)
    {
        float ratio_x = Screen.width / 1920f*scaler_x;
        float ratio_y = Screen.height / 1080f*scaler_y;

        if(pot == "init"){
            return new Vector2 (0*ratio_x,0*ratio_y);
        }
        else if(pot == "Point"){
            return new Vector2 (-10*ratio_x,-20*ratio_y);
        }
        else if(pot == "Virgule"){
            return new Vector2 (-10*ratio_x,-30*ratio_y);
        }
        else if(pot == "Deux Points"){
            return new Vector2 (0*ratio_x,-10*ratio_y);
        }
        else if(pot == "Point Virgule"){
            return new Vector2 (0*ratio_x,-15*ratio_y);
        }
        else if(pot == "Exclamation"){
            return new Vector2 (0*ratio_x,0*ratio_y);
        }
        else if(pot == "Interrogation"){
            return new Vector2 (0*ratio_x,0*ratio_y);
        }
        else{
            return new Vector2 (0*ratio_x,0*ratio_y);
        }
    }

    public void init_taille_texte()
    {
        taillePolice = Screen.width*taille_police;
        scaler_x = 1;
        scaler_y = 1;

        text_scaler.SetActive(true);
        text_scaler.GetComponentInChildren<TextMeshProUGUI>().fontSize=taillePolice;
    
        lineWidth = Screen.width*(1f-debut_text_gauche-marge_text_droite);

        if(dualAnim)
        {
            textFloor = Screen.height*debut_hauteur_dual - text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredHeight / 2;
        }else
        {
            textFloor = Screen.height*debut_hauteur_normal - text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredHeight / 2;
        }
        
        text_sol = Screen.height*hauteur_table;
            
        spaceSize = 1.6f*text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

        lineJump = 1.1f*text_scaler.GetComponentInChildren<TextMeshProUGUI>().preferredHeight;

        

        text_scaler.SetActive(false);
    }
}
