//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.Video;

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

    [Header("Taille Police Debut")]
    public float taille_police;

    //Events
    public System.Action OnTextLoad;

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
    private List<float> listeToStop;

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
    public GameObject text_scalerUL;
    public GameObject text_scalerUL_Dual;
    public GameObject text_scalerDR;
    public float scaler_x;//pour affichage slot
    public float scaler_y;//pour affichage slot
    private float size_init_x;//pour affichage slot
    private float size_init_y;//pour affichage slot
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
    private string recapitulatif;
    private int essaie;
    private int frames;
    private int errorNum;

    [Header("Item win menu")]
    [SerializeField] private GameObject popupItemWinner;
    [SerializeField] private GameObject itemImage;
    [SerializeField] private GameObject popupIndice;
    private bool popupCheckUnlockedLevel = false;
    [SerializeField] private GameObject container;

    // Start is called before the first frame update
    void Start()
    {
        vrai_nomspropres.Add("Tokki");
        vrai_nomspropres.Add("Pesto");

        // DATA IMPORT
        if (scrGlobal.Instance.FromGameBuilder || scrGlobal.Instance.FromBonusLevel)
        {
            TextFile = scrGlobal.Instance.GameBuilderText;
        }
        else
        {
            TextFile = scrGlobal.Instance.file;
        }

        CorrectFile = scrGlobal.Instance.animTextFile;
        dualAnim = scrGlobal.Instance.nivAntiOubli;


        init_taille_texte();

        canTouchPonct = true;
        s = new List<string>();
        words = new List<string>();
        cursor.gameObject.SetActive(!dualAnim);

        if (dualAnim)
        {
            CutsWordsDual(CorrectFile);
            GameObject.Find("Fond").GetComponent<Image>().sprite = Resources.Load<Sprite>("background_animation");
        }
        else
        {
            CutsWordsDual(TextFile);
            GameObject.Find("Fond").GetComponent<Image>().sprite = Resources.Load<Sprite>("background03");
        }

        // creates separators list
        separators = new string[words.Count];
        for (int i = 0; i < separators.Length; i++) separators[i] = "";
        wordsObj = new GameObject[words.Count];
        slots = new GameObject[words.Count];

        placesWords();

        if (!dualAnim)
        {

            animationLog.gameObject.SetActive(showLog);
            animationLog.text = "Les clients ont hâte de manger votre plat !";

            //Taille Curseur
            GameObject.Find("Curseur").GetComponent<CurseurScript>().change_taille();
            cursorStart = new Vector3(cursor.transform.position.x, textFloor, 0);

            cursor.transform.position = cursorStart;

        }
        else
        {
            depart_block(TextFile);
            canTouchPonct = false;
            ButtonLayer.SetActive(false);

            animationObj.gameObject.SetActive(true);
            animationObj.GetComponent<SelectionAnimation>().SelectAnimation(false, scrGlobal.Instance.levelNum);

            animationLog.gameObject.SetActive(false);
            Invoke("boutonlayer_anim", (float) animationObj.GetComponentInChildren<VideoPlayer>().length);//problem
        }
        HideSlots();

        OnTextLoad?.Invoke();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movingCursor)
        {

            Vector3 trans = cursor.transform.position;
            trans.x += cursorSpeed;

            //arret deplacement curseur
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


                        // Hides all buttons and shows the "continue" one, which is the first child
                        ButtonLayer.SetActive(true);
                        if (scrGlobal.Instance.FromGameBuilder)
                        {
                            ButtonLayer.transform.GetChild(1).gameObject.SetActive(true);
                        }
                        else
                        {
                            ButtonLayer.transform.GetChild(0).gameObject.SetActive(true);
                        }

                        for (int i = 2; i < ButtonLayer.transform.childCount; i++)
                        {
                            ButtonLayer.transform.GetChild(i).gameObject.SetActive(false);
                        }

                        // Unlocks next level
                        popupCheckUnlockedLevel = scrGlobal.Instance.levelunlocked[scrGlobal.Instance.levelNum];
                        scrGlobal.Instance.levelunlocked[scrGlobal.Instance.levelNum] = true;
                        scrGlobal.Instance.levelunlocked[scrGlobal.Instance.levelNum] = true;
                    }
                    // recap phrase for the animation recall
                    else
                    {
                        string recap = "Les clients ont trouvé ce repas";

                        recapContent += "\nErreur à " + frames / 60 + " secondes :\n";

                        //Cas simples
                        if (pasAssezVirgule && !mauvaiseVirgule && !pasbonneVirgule) { recap += " trop doux"; recapContent += " - pas assez de virgules \n"; }
                        if (tropVirgule && !mauvaiseVirgule && !pasbonneVirgule) { recap += " trop piquant"; recapContent += " - trop de virgules \n"; }
                        if (mauvaiseVirgule && !tropVirgule && !pasAssezVirgule && !pasbonneVirgule) { recap += " mal épicé"; recapContent += " - mauuvais placement de la virgule \n"; }
                        if (pasbonneVirgule && !pasAssezVirgule && !tropVirgule && !mauvaiseVirgule) { recap += " déséquilibré"; recapContent += " - mauvaise virgule \n"; }

                        //Cas à deux true
                        if (pasAssezVirgule && mauvaiseVirgule && point_reussite) { recap += " trop doux et mal épicé"; recapContent += " - pas assez de virgules et mauvais placement de la virgule \n"; }
                        if (pasAssezVirgule && mauvaiseVirgule && !point_reussite) { recap += " trop doux, mal épicé"; recapContent += " - pas assez de virgules et mauvais placement de la virgule \n"; }

                        if (tropVirgule && mauvaiseVirgule && point_reussite) { recap += " trop piquant et mal épicé"; recapContent += " - trop de virgules et mauvais placement d'une virgule \n"; }
                        if (tropVirgule && mauvaiseVirgule && !point_reussite) { recap += " trop piquant, mal épicé"; recapContent += " - trop de virgules et mauvais placement d'une virgule \n"; }

                        if (pasbonneVirgule && pasAssezVirgule && point_reussite) { recap += " déséquilibré et trop doux"; recapContent += " - mauvaise virgule et pas assez de virgules \n"; }
                        if (pasbonneVirgule && pasAssezVirgule && !point_reussite) { recap += " déséquilibré, trop doux"; recapContent += " - mauvaise virgule et pas assez de virgules \n"; }

                        if (pasbonneVirgule && tropVirgule && point_reussite) { recap += " déséquilibré et trop piquant"; recapContent += " - mauvaise virgule et trop de virgules \n"; }
                        if (pasbonneVirgule && tropVirgule && !point_reussite) { recap += " déséquilibré, trop piquant"; recapContent += " - mauvaise virgules et trop de virgules \n"; }

                        if (pasbonneVirgule && mauvaiseVirgule && point_reussite) { recap += " déséquilibré  et mal épicé"; recapContent += " mauvaise position et mauvaise virgule \n"; }
                        if (pasbonneVirgule && mauvaiseVirgule && !point_reussite) { recap += " déséquilibré, mal épicé"; recapContent += " mauvaise position et mauvaise virgule \n"; }

                        if (point_reussite) { recap += "."; }
                        if ((!virgule_reussite) && (!point_reussite)) { recap += " et"; }

                        if (pointTropTot) { recap += " trop court."; recapContent += "- point trop tôt\n"; }//trop salé
                        if (manquePoint) { recap += " trop long."; recapContent += "- manque d'un point\n"; }//pas assez salé
                        if (mauvaisPoint) { recap += " désordonné."; recapContent += "- mauvais point\n"; }//étrangement salé.
                        animationLog.text = recap;
                        // data recap
                        ButtonLayer.SetActive(true);

                        canTouchPonct = true;
                    }
                    //Début animation après fin validation
                    AnimationFondu();
                }
            }
            else if (trans.x > listeToStop[lineCursor])
            {
                trans.x = cursorStart.x;
                trans.y -= lineJump;
                lineCursor++;
            }

            // cursor gets back to original position
            cursor.transform.position = trans;

        }
        else
        {
            // counts frames (for the timer) when the cursor isn't moving, to be fair
            if (!textreussite) frames++;

            if (frames == 10800)//60 -> 1sec
            {
                GameObject.Find("Indice").GetComponent<scrIndice>().limite = true;
            }

        }
        //anim curseur miam
        cursor.GetComponentInChildren<Animator>().SetBool("Validation", movingCursor);
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
                    v_mots = "";
                    break;
                case '.'://lettre_ponct_espace ou lettre_ponct_retourligne
                    vrai_separators.Add(".");
                    vrai_slot.Add("");
                    vrai_mots.Add(v_mots);
                    v_skip = true;
                    v_mots = "";
                    toLower = true;
                    break;
                case '!'://espace_ponct_espace ou espace_ponct_retourligne
                    vrai_separators[vrai_separators.Count - 1] = "!";
                    v_skip = true;
                    toLower = true;
                    break;
                case '?'://espace_ponct_espace ou espace_ponct_retourligne
                    vrai_separators[vrai_separators.Count - 1] = "?";
                    v_skip = true;
                    toLower = true;
                    break;
                case ':'://espace_ponct_espace ou espace_ponct_retourligne
                    vrai_separators[vrai_separators.Count - 1] = ":";
                    v_skip = true;
                    break;
                case ';'://espace_ponct_espace
                    vrai_separators[vrai_separators.Count - 1] = ";";
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
                    v_mots = "\n";
                    cond1 = true;
                    if (cond3)
                    {
                        v_skip = true;
                        cond3 = false;
                        cond4 = true;
                    }
                    break;
                case '/':
                    if (cond1)
                    {
                        cond1 = false;
                        cond2 = true;
                        break;
                    }
                    if (cond2)
                    {
                        cond2 = false;
                        cond3 = true;
                        break;
                    }
                    //lettre
                    v_mots += TF.text[i];

                    if (toLower)
                    {
                        v_mots = v_mots.ToLower();
                        toLower = false;
                    }
                    v_tiret = false;
                    break;
                case ' '://espace_ponct_espace ou lettre_espace ou ponct_espace

                    if (v_tiret)
                    {
                        v_tiret = false;
                        v_mots += " ";
                    }
                    else if (v_skip)
                    {
                        v_skip = false;
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

                    if (toLower)
                    {
                        v_mots = v_mots.ToLower();
                        toLower = false;
                    }
                    v_tiret = false;
                    break;
            }
            if (cond4) break;
        }
        if (!v_skip)
        {
            vrai_mots.Add(v_mots);
        }

        //verif noms
        for (int a = 0; a < vrai_mots.Count; a++)
        {
            string verif_mot = vrai_mots[a].ToLower();
            for (int b = 0; b < vrai_nomspropres.Count; b++)
            {
                string verif_nom = vrai_nomspropres[b].ToLower();
                if (verif_mot.Equals(verif_nom)) vrai_mots[a] = verif_mot.Substring(0, 1).ToUpper() + verif_mot.Substring(1, verif_mot.Length - 1);
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
        for (int a = 0; a < TF.text.Length; a++)
        {
            switch (TF.text[a])
            {
                case '.':
                    liste_ponct.Add(".");
                    v_skip = true;
                    break;
                case '!':
                    liste_ponct[liste_ponct.Count - 1] = "!";
                    v_skip = true;
                    break;
                case '?':
                    liste_ponct[liste_ponct.Count - 1] = "?";
                    v_skip = true;
                    break;
                case ',':
                    liste_ponct.Add(",");
                    v_skip = true;
                    break;
                case ';':
                    liste_ponct[liste_ponct.Count - 1] = ";";
                    v_skip = true;
                    break;
                case ':':
                    liste_ponct[liste_ponct.Count - 1] = ":";
                    v_skip = true;
                    break;
                case '-':
                    v_tiret = true;
                    cond1 = false;
                    break;
                case '\n':
                    v_skip = false;
                    cond1 = true;
                    if (cond3)
                    {
                        v_skip = true;
                        cond3 = false;
                        cond4 = true;
                    }
                    break;
                case '/':
                    if (cond1)
                    {
                        cond1 = false;
                        cond2 = true;
                        break;
                    }
                    if (cond2)
                    {
                        cond2 = false;
                        cond3 = true;
                        break;
                    }
                    v_tiret = false;
                    break;
                case ' ':
                    if (v_tiret)
                    {
                        v_tiret = false;
                    }
                    else if (v_skip)
                    {
                        v_skip = false;
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
            if (cond4)
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
        for (int a = 0; a < TF.text.Length; a++)
        {
            switch (TF.text[a])
            {
                case '.':
                    liste_ponct.Add(".");
                    v_skip = true;
                    break;
                case '!':
                    liste_ponct[liste_ponct.Count - 1] = "!";
                    v_skip = true;
                    break;
                case '?':
                    liste_ponct[liste_ponct.Count - 1] = "?";
                    v_skip = true;
                    break;
                case ',':
                    liste_ponct.Add(",");
                    v_skip = true;
                    break;
                case ';':
                    liste_ponct[liste_ponct.Count - 1] = ";";
                    v_skip = true;
                    break;
                case ':':
                    liste_ponct[liste_ponct.Count - 1] = ":";
                    v_skip = true;
                    break;
                case '-':
                    v_tiret = true;
                    break;
                case '\n':
                    v_skip = false;
                    break;
                case ' ':
                    if (v_tiret)
                    {
                        v_tiret = false;
                    }
                    else if (v_skip)
                    {
                        v_skip = false;
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

        for (int a = 0; a < liste_ponct.Count; a++)
        {
            switch (liste_ponct[a])
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
        listeToStop = new List<float>();


        //taille police
        WordPrefab.GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;
        WordPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().fontSize = taillePolice;

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
            for (int a = 0; a < vrai_mots[i].Length; a++)
            {
                if (!vrai_mots[i][a].Equals('-') && !vrai_mots[i][a].Equals(' '))
                {
                    wordObj.GetComponent<MotsGO>().maj_pos = a;
                    break;
                }
            }

            //verif nom propre et mot en maj
            if (vrai_mots[i].Equals(vrai_mots[i].ToLower())) wordObj.GetComponent<MotsGO>().nom_propre = false;

            float pw = wordObj.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

            alaligne = vrai_mots[i][0].Equals('\n');

            if (total_width + pw > lineWidth || alaligne)
            {
                if (alaligne)
                {
                    wordObj.GetComponentInChildren<TextMeshProUGUI>().text = vrai_mots[i].Substring(1, vrai_mots[i].Length - 1);
                }

                listeToStop.Add(vrai_slots_GO[i - 1].transform.position.x);

                total_width = 0f;
                total_height -= lineJump;

                lineNumber++;
            }

            //positions des mots et slots
            total_width += pw / 2;
            wordObj.transform.position = new Vector3((text_scalerUL.transform.position.x - text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredWidth) + total_width, textFloor + total_height, 0);
            total_width += spaceSize / 2 + pw / 2;
            slot.transform.position = new Vector3((text_scalerUL.transform.position.x - text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredWidth) + total_width, textFloor + total_height, 0);
            slot.transform.GetComponent<scrSlot>().ligne = lineNumber;
            slot.transform.GetComponent<scrSlot>().INDEX = i;
            slot.transform.GetComponent<scrSlot>().pos_origine = slot.transform.position;
            total_width += spaceSize / 2;

            //afficher sur le canvas
            wordObj.transform.SetParent(container.transform);
            slot.transform.SetParent(container.transform);

            vrai_slots_GO.Add(slot);
            vrai_mots_GO.Add(wordObj);
        } // end of word placement

        listeToStop.Add(vrai_slots_GO[vrai_slots_GO.Count - 1].transform.position.x);

        if (vrai_mots_GO[vrai_mots_GO.Count - 1].transform.position.y < text_sol)
        {
            replaceWords();
        }

    }

    public void vrai_valider()
    {
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


        for (int a = 0; a < liste_separators.Count; a++)
        {
            bool reussite = true;//flag

            for (int b = 0; b < liste_separators[a].Count; b++)
            {
                if (!vrai_slots_GO[b].GetComponent<scrSlot>().ponctuation.Equals(liste_separators[a][b])) reussite = false;
            }

            autre_reussite.Add(reussite);
        }

        for (int a = 0; a < autre_reussite.Count; a++)
        {
            if (autre_reussite[a]) textreussite = true;
        }

        if (textreussite)
        {
            deplacement_cursor(vrai_slots_GO.Count - 1);
            return;
        }
        //boucle de vérification
        bool arret_premier_point = false;
        for (int a = 0; a < vrai_separators.Count; a++)
        {
            string slot_joueur = vrai_slots_GO[a].GetComponent<scrSlot>().ponctuation;
            string slot_verif = vrai_separators[a];



            if (!slot_joueur.Equals(slot_verif))
            {
                arret_premier_point = true;

                //slot sans ponctuation
                if (slot_joueur.Equals(""))
                {
                    //au lieu de mid ponct
                    if (slot_verif.Equals(",") || slot_verif.Equals(":") || slot_verif.Equals(";"))
                    {
                        nb_midponct_verif++;
                    }
                    //au lieu de fin ponct
                    else if (slot_verif.Equals(".") || slot_verif.Equals("?") || slot_verif.Equals("!"))
                    {

                        manquePoint = true;
                    }
                }
                //slot mid ponctuation
                else if (slot_joueur.Equals(",") || slot_joueur.Equals(":") || slot_joueur.Equals(";"))
                {
                    nb_midponct_joueur++;
                    //au lieu d'un vide
                    if (slot_verif.Equals(""))
                    {
                        mauvaiseVirgule = true;
                    }
                    //au lieu de fin ponct
                    else if (slot_verif.Equals(".") || slot_verif.Equals("?") || slot_verif.Equals("!"))
                    {
                        manquePoint = true;
                        mauvaiseVirgule = true;

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
                    if (slot_verif.Equals(""))
                    {
                        pointTropTot = (true != manquePoint);
                    }
                    //au lieu de mid ponct
                    else if (slot_verif.Equals(",") || slot_verif.Equals(":") || slot_verif.Equals(";"))
                    {
                        pointTropTot = (true != manquePoint);
                    }
                    else// mauvais point
                    {
                        mauvaisPoint = true;
                    }

                    //verification nb mid ponct
                    pasAssezVirgule = nb_midponct_joueur < nb_midponct_verif;
                    tropVirgule = nb_midponct_joueur > nb_midponct_verif;

                    deplacement_cursor(a);

                    return;
                }
            }

            pasAssezVirgule = nb_midponct_joueur < nb_midponct_verif;
            tropVirgule = nb_midponct_joueur > nb_midponct_verif;

            if (arret_premier_point)
            {
                if (slot_joueur.Equals(".") || slot_joueur.Equals("?") || slot_joueur.Equals("!"))
                {
                    deplacement_cursor(a);
                    return;
                }
            }
        }
        deplacement_cursor(vrai_slots_GO.Count - 1);
    }

    private void deplacement_cursor(int pos)
    {
        point_reussite = !pointTropTot && !manquePoint && !mauvaisPoint;
        virgule_reussite = !tropVirgule && !pasAssezVirgule && !mauvaiseVirgule && !pasbonneVirgule;
        textreussite = (point_reussite && virgule_reussite) || textreussite;

        animationLog.text = "Les clients sont en train de tester votre plat...";

        lineToStop = vrai_slots_GO[pos].GetComponent<scrSlot>().ligne;
        text_scalerUL.SetActive(true);
        posToStop = vrai_slots_GO[pos].transform.position.x - text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;
        text_scalerUL.SetActive(false);

        movingCursor = true;
        cursor.transform.position = cursorStart;
        lineCursor = 0;
        cursor.transform.SetAsLastSibling();
    }

    public void replaceWords()
    {
        //globale
        taillePolice -= .1f;
        lineNumber = 0;
        listeToStop = new List<float>();
        //fonction
        bool alaligne = false;
        float total_width = 0;
        float total_height = 0;

        text_scalerUL.SetActive(true);
        text_scalerUL_Dual.SetActive(true);
        text_scalerDR.SetActive(true);

        text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;
        text_scalerUL_Dual.GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;
        text_scalerDR.GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;

        scaler_x = text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredWidth / size_init_x;
        scaler_y = text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredHeight / size_init_y;

        if (dualAnim)
        {
            textFloor = text_scalerUL_Dual.transform.position.y - text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredHeight;

        }
        else
        {
            textFloor = text_scalerUL.transform.position.y - text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredHeight;
        }

        text_sol = text_scalerDR.transform.position.y + text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredHeight / 2;

        spaceSize = 1.6f * text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

        lineJump = 1.1f * text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredHeight;

        text_scalerUL.SetActive(false);
        text_scalerUL_Dual.SetActive(false);
        text_scalerDR.SetActive(false);

        for (int a = 0; a < vrai_mots_GO.Count; a++)
        {
            vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;
            vrai_mots_GO[a].transform.GetChild(0).GetChild(1).GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;

            float pw = vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

            alaligne = vrai_mots[a][0].Equals('\n');

            if (total_width + pw > lineWidth || alaligne)
            {
                if (alaligne)
                {
                    vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().text = vrai_mots[a].Substring(1, vrai_mots[a].Length - 1);
                }
                listeToStop.Add(vrai_slots_GO[a - 1].transform.position.x);

                total_width = 0f;
                total_height -= lineJump;

                lineNumber++;
            }

            total_width += pw / 2;
            vrai_mots_GO[a].transform.position = new Vector3((text_scalerUL.transform.position.x - text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredWidth) + total_width, textFloor + total_height, 0);
            total_width += spaceSize / 2 + pw / 2;
            vrai_slots_GO[a].transform.position = new Vector3((text_scalerUL.transform.position.x - text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredWidth) + total_width, textFloor + total_height, 0);
            vrai_slots_GO[a].transform.GetComponent<scrSlot>().ligne = lineNumber;
            vrai_slots_GO[a].transform.GetComponent<scrSlot>().pos_origine = vrai_slots_GO[a].transform.position;
            total_width += spaceSize / 2;

        }

        listeToStop.Add(vrai_slots_GO[vrai_slots_GO.Count - 1].transform.position.x);

        if (vrai_mots_GO[vrai_mots_GO.Count - 1].transform.position.y < text_sol)
        {
            replaceWords();
        }
    }

    public void majuscule(string tag, int slot_pos)
    {
        if (slot_pos == vrai_mots_GO.Count - 1) return;


        if (tag.Equals("Point") || tag.Equals("Exclamation") || tag.Equals("Interrogation"))
        {
            if (!vrai_mots_GO[slot_pos + 1].GetComponent<MotsGO>().nom_propre)
            {
                //ajout majuscule
                string mot = vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                int maj_pos = vrai_mots_GO[slot_pos + 1].GetComponent<MotsGO>().maj_pos + 1;
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mot.Substring(0, maj_pos).ToUpper() + mot.Substring(maj_pos, mot.Length - maj_pos);



                //triche majuscule animation
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = mot.Substring(0, maj_pos).ToUpper();
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

                //var raccourci
                float maj_width = vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().preferredWidth;
                float maj_height = vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().preferredHeight;
                float maj_max = Mathf.Max(maj_width, maj_height);

                //partie texte
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(maj_width, maj_height);
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(maj_width / 2, 0);


                //partie anim
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maj_max, maj_max);
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(maj_width / 2, 0);

                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }

        }


    }

    public void demajuscule(string tag, int slot_pos)
    {
        if (slot_pos == vrai_mots_GO.Count - 1) return;


        if (tag.Equals("Point") || tag.Equals("Exclamation") || tag.Equals("Interrogation"))
        {
            if (!vrai_mots_GO[slot_pos + 1].GetComponent<MotsGO>().nom_propre)
            {
                string mot = vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mot.ToLower();

                //partie anim
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                vrai_mots_GO[slot_pos + 1].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }

        }
    }

    public void ValiderClick()
    {
        ButtonLayer.SetActive(false);
        canTouchPonct = false;

        if (!dualAnim) vrai_valider();
        else ValiderDual();

        if (textreussite)
        {
            texte_data("Reussite");
        }
        else
        {
            texte_data("Erreur");
        }
    }

    public void ValiderDual()
    {

        textreussite = false;
        List<bool> autre_reussite = new List<bool>();//liste flag

        //verification
        for (int a = 0; a < liste_separators.Count; a++)
        {
            bool reussite = true;//flag

            for (int b = 0; b < liste_separators[a].Count; b++)
            {
                if (!vrai_slots_GO[b].GetComponent<scrSlot>().ponctuation.Equals(liste_separators[a][b])) reussite = false;
            }

            autre_reussite.Add(reussite);
        }

        for (int a = 0; a < autre_reussite.Count; a++)
        {
            if (autre_reussite[a]) textreussite = true;
        }

        animationObj.GetComponent<SelectionAnimation>().SelectAnimation(textreussite, scrGlobal.Instance.levelNum);
        Invoke("boutonlayer_anim", (float) animationObj.GetComponentInChildren<VideoPlayer>().length);//timing a changer

        if (textreussite)
        {
            Invoke("DisplayRewardsIndice", (float) animationObj.GetComponentInChildren<VideoPlayer>().length);
            if (scrGlobal.Instance.levelunlocked[scrGlobal.Instance.levelNum]) return;
            scrGlobal.Instance.levelunlocked[scrGlobal.Instance.levelNum] = true;
            scrGlobal.Instance.nbIndices++;

        }

    }

    public void showIndice()
    {
        texte_data("Indice");

        for (int a = 0; a < vrai_separators.Count; a++)
        {
            if (!vrai_separators[a].Equals(""))
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
            if (!vrai_slots_GO[i].GetComponentInChildren<scrSlot>().isUsed)
            {
                vrai_slots_GO[i].GetComponentInChildren<Image>().enabled = true;

                if (vrai_slots_GO[i].GetComponentInChildren<scrSlot>().indice)
                {
                    vrai_slots_GO[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                }


            }
            else
            {
                vrai_slots_GO[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }

            vrai_slots_GO[i].GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta = taillePot;
            vrai_slots_GO[i].GetComponentInChildren<BoxCollider2D>().size = taillePot * 1.25f;

            Vector3 V3_slot = vrai_slots_GO[i].GetComponentInChildren<RectTransform>().position;
            vrai_slots_GO[i].GetComponentInChildren<RectTransform>().position = new Vector3(V3_slot.x + slot_pos.x, V3_slot.y + slot_pos.y, V3_slot.z);

        }


    }

    public void HideSlots()
    {
        for (int i = 0; i < vrai_slots_GO.Count; i++)
        {

            vrai_slots_GO[i].GetComponentInChildren<Image>().enabled = false;

            if (vrai_slots_GO[i].GetComponentInChildren<scrSlot>().isUsed && vrai_slots_GO[i].GetComponentInChildren<scrSlot>().indice)
            {
                vrai_slots_GO[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }

            vrai_slots_GO[i].GetComponentInChildren<RectTransform>().position = vrai_slots_GO[i].GetComponent<scrSlot>().pos_origine;
        }
    }

    public void texte_joueur()
    {
        currentText = "";

        for (int a = 0; a < vrai_mots_GO.Count; a++)
        {
            string slotText = vrai_slots_GO[a].GetComponent<scrSlot>().ponctuation;

            if (vrai_mots[a][0].Equals('\n'))
            {
                currentText += "\n";
            }

            currentText += vrai_mots_GO[a].GetComponentInChildren<TextMeshProUGUI>().text;


            if (slotText.Equals("") || slotText.Equals(",") || slotText.Equals("."))
            {
                currentText += slotText + " ";
            }
            else
            {
                currentText += " " + slotText + " ";
            }
        }
    }

    public void texte_data(string etat)//Retour(Retour) Valider(Reussite, Erreur) Indice(Indice)
    {
        switch (etat)
        {
            case "Retour":
                recapitulatif += "--- " + etat + " après " + frames / 60 + " secondes. pour un total de " + (frames + scrGlobal.Instance.GetChrono()) / 60 + " secondes. ---";
                scrGlobal.Instance.SetTexteFichier(recapitulatif);

                scrGlobal.Instance.SetRetour(frames);
                return;
            case "Indice":
                recapitulatif += "--- " + etat + " après " + frames / 60 + " secondes. ---\n";
                return;
            case "Erreur":
                recapitulatif += "--- " + etat + " après " + frames / 60 + " secondes. ---\n";
                texte_joueur();
                recapitulatif += currentText + "\n\n";
                scrGlobal.Instance.SetTexteFichier(recapitulatif);
                return;
            case "Reussite":
                recapitulatif += "--- " + etat + " après " + frames / 60 + " secondes. pour un total de " + (frames + scrGlobal.Instance.GetChrono()) / 60 + " secondes. ---\n";
                texte_joueur();
                recapitulatif += currentText;
                scrGlobal.Instance.SetTexteFichier(recapitulatif);

                scrGlobal.Instance.SetReussite(frames);
                return;
        }
    }

    public void GoToMap()
    {
        if (scrGlobal.Instance.FromGameBuilder)
        {
            SceneManager.LoadScene("GameBuilder");
            return;
        }

        if (!textreussite)
        {
            texte_data("Retour");
        }

        if (scrGlobal.Instance.FromBonusLevel)
        {
            SceneManager.LoadScene("AccesBonus");
            return;
        }

        if (scrGlobal.Instance.levelNum == 15)
        {
            SceneManager.LoadScene("endScene");
            return;
        }
        SceneManager.LoadScene("MapScene");
    }

    public void DisplayRewardsItem()
    {
        if (textreussite && checkClassicLevel() && !popupCheckUnlockedLevel)
        {
            popupItemWinner.SetActive(true);
            popupItemWinner.transform.SetAsLastSibling();
            container.SetActive(false);
            switch (scrGlobal.Instance.levelNum)
            {
                case 1:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Casserole");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(170, 300);
                    break;
                case 2:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Poele");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 470);
                    break;
                case 4:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Cuillere");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 260);
                    break;
                case 5:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Spatule");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(175, 300);
                    break;
                case 7:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Rape");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(290, 366);
                    break;
                case 9:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Louche");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 350);
                    break;
                case 10:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Fouet");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(190, 375);
                    break;
                case 12:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Couteau");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 210);
                    break;
                case 13:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Passoire");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(260, 260);
                    break;
                case 15:
                    itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/endSceneSprites/Rouleau");
                    itemImage.GetComponent<RectTransform>().sizeDelta = new Vector2(260, 260);
                    break;
                default:
                    break;
            }
        }
    }

    public void DisplayRewardsIndice()
    {
        if (!checkClassicLevel())
        {
            popupIndice.SetActive(true);
            popupIndice.transform.SetAsLastSibling();
            container.SetActive(false);
        }
    }

    public bool checkClassicLevel()
    {
        return scrGlobal.Instance.levelNum == 1 || scrGlobal.Instance.levelNum == 2 || scrGlobal.Instance.levelNum == 4 ||
            scrGlobal.Instance.levelNum == 5 || scrGlobal.Instance.levelNum == 7 || scrGlobal.Instance.levelNum == 9 ||
            scrGlobal.Instance.levelNum == 10 || scrGlobal.Instance.levelNum == 12 || scrGlobal.Instance.levelNum == 13 ||
            scrGlobal.Instance.levelNum == 15;
    }

    public void AnimationFondu()
    {
        //affichage premier plan
        fondu.transform.SetAsLastSibling();


        //début fond noir
        fondu.SetActive(true);
        fondu.GetComponent<Animator>().SetBool("Actif", true);

        if (point_reussite && !virgule_reussite)
        {
            //saute l'animation curseur

            animation_virgule();
        }
        else
        {

            //animation curseur ->  .2 sec
            Invoke("animation_point", .2f);
        }


    }

    public void animation_point()
    {
        cursor.transform.SetAsLastSibling();


        //ManquePoint > PointTropTot > MauvaisPoint
        cursor.GetComponentInChildren<Animator>().SetBool("Reussite", textreussite);
        cursor.GetComponentInChildren<Animator>().SetBool("Gros", manquePoint);
        cursor.GetComponentInChildren<Animator>().SetBool("Maigre", pointTropTot);
        cursor.GetComponentInChildren<Animator>().SetBool("Mauvaise", mauvaisPoint);

        //delai avant passage en fond et suite de l'animation des clients
        if (!point_reussite && virgule_reussite)
        {
            Invoke("fondu_fin", 1);
        }
        else
        {
            Invoke("animation_virgule", 1);
        }

    }


    public void animation_virgule()
    {

        fondu.transform.SetAsLastSibling();
        clients.transform.SetAsLastSibling();

        client_virgule.SetActive(true);

        // > MauvaiseVirgule > PasBonneVirgule
        client_virgule.GetComponent<Animator>().SetBool("Actif", true);
        client_virgule.GetComponent<Animator>().SetBool("Reussite", textreussite);
        client_virgule.GetComponent<Animator>().SetBool("Feu", tropVirgule);
        client_virgule.GetComponent<Animator>().SetBool("Berk", pasAssezVirgule);
        client_virgule.GetComponent<Animator>().SetBool("Confu", mauvaiseVirgule);
        client_virgule.GetComponent<Animator>().SetBool("Mauvaise", pasbonneVirgule);



        //Invoke("fin_animation",2);
        Invoke("fondu_fin", 1);
    }

    public void fondu_fin()
    {
        fondu.GetComponent<Animator>().SetBool("Actif", false);
        Invoke("fin_animation", 1);
        Invoke("DisplayRewardsItem", 1);
    }

    public void fin_animation()
    {
        cursor.GetComponentInChildren<Animator>().SetBool("Maigre", false);
        cursor.GetComponentInChildren<Animator>().SetBool("Gros", false);
        cursor.GetComponentInChildren<Animator>().SetBool("Mauvaise", false);
        fondu.GetComponentInChildren<Animator>().SetBool("Actif", false);

        cursor.transform.position = cursorStart;

        //reset
        client_virgule.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);
        client_virgule.GetComponent<RectTransform>().sizeDelta = new Vector2(478, 844);
        client_virgule.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        client_virgule.SetActive(false);
        fondu.SetActive(false);
    }

    public void dual_anim_reset()
    {
        animationObj.GetComponent<Animator>().SetBool("Validation", false);
        animationObj.GetComponent<Animator>().SetBool("Reussite", false);
    }

    private void boutonlayer_anim()
    {
        ButtonLayer.SetActive(true);
        if (textreussite)
        {
            ButtonLayer.transform.GetChild(0).gameObject.SetActive(true);
            for (int i = 1; i < ButtonLayer.transform.childCount; i++)
            {
                ButtonLayer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        canTouchPonct = true;
    }

    private Vector2 quel_pot(string pot)
    {
        float ratio_x = Screen.width / 1920f * scaler_x;
        float ratio_y = Screen.height / 1080f * scaler_y;

        //décalage position slot
        if (pot == "init")
        {
            return new Vector2(0 * ratio_x, 0 * ratio_y);
        }
        else if (pot == "Point")
        {
            return new Vector2(-10 * ratio_x, -20 * ratio_y);
        }
        else if (pot == "Virgule")
        {
            return new Vector2(-10 * ratio_x, -30 * ratio_y);
        }
        else if (pot == "Deux Points")
        {
            return new Vector2(0 * ratio_x, -10 * ratio_y);
        }
        else if (pot == "Point Virgule")
        {
            return new Vector2(0 * ratio_x, -15 * ratio_y);
        }
        else if (pot == "Exclamation")
        {
            return new Vector2(0 * ratio_x, 0 * ratio_y);
        }
        else if (pot == "Interrogation")
        {
            return new Vector2(0 * ratio_x, 0 * ratio_y);
        }
        else
        {
            return new Vector2(0 * ratio_x, 0 * ratio_y);
        }
    }

    public void init_taille_texte()
    {

        float widthScreenRatio = Screen.width * taille_police / 1920;
        float heightScreenRatio = Screen.height * taille_police / 1080;
        taille_police = Mathf.Min(widthScreenRatio, heightScreenRatio);
        taillePolice = taille_police;//Faut commencer avec une valeur
        scaler_x = 1;
        scaler_y = 1;

        text_scalerUL.SetActive(true);
        text_scalerUL_Dual.SetActive(true);
        text_scalerDR.SetActive(true);

        text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;
        text_scalerUL_Dual.GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;
        text_scalerDR.GetComponentInChildren<TextMeshProUGUI>().fontSize = taillePolice;

        size_init_x = text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;
        size_init_y = text_scalerUL.GetComponentInChildren<TextMeshProUGUI>().preferredHeight;

        lineWidth = (text_scalerDR.transform.position.x - text_scalerUL.transform.position.x) + size_init_x;

        if (dualAnim)
        {
            textFloor = text_scalerUL_Dual.transform.position.y - size_init_y;

        }
        else
        {
            textFloor = text_scalerUL.transform.position.y - size_init_y;
        }

        text_sol = text_scalerDR.transform.position.y + size_init_y / 2;

        spaceSize = 1.6f * size_init_x;

        lineJump = 1.1f * size_init_y;

        text_scalerUL.SetActive(false);
        text_scalerUL_Dual.SetActive(false);
        text_scalerDR.SetActive(false);
    }
}
