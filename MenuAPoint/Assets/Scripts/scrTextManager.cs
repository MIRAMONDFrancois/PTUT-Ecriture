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
    public Sprite machin;

    [Header("Custom")]
    public bool addColors = false; // if the blocks color the words or not

    [Header("Fixed Separators")]
    public bool useSpecial = false;
    public TextAsset SpecialFile;
    private string[] sp;

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
    private bool init_anim;
    public TextAsset CorrectFile;
    private bool hideGen; // may be irrelevant, like useSpecial and others
    public bool canBeMoved;
    public bool canBeDeleted;
    public GameObject animationObj;


    // Text pos
    private float lineWidth = 1550f; // 
    private float textFloor = 450f; // (default: 450f; anim: 75f) vertical position of the top of the text
    private float spaceSize = 50f; // 
    private float lineJump = 80f;
    private float taillePolice = 72f;

    // Text errors
    //fin de phrase
    private bool pointTropTot = false;
    private bool manquePoint = false;

    //mileu de phrase
    private bool tropVirgule = false;
    private bool pasAssezVirgule = false;
    private bool mauvaiseVirgule = false;//mauvaise position

    private bool textreussite = false; //pour pas faire "!bool && !bool" à chaque fois
    private bool point_reussite = false; 
    private bool virgule_reussite = false;

    // Debug text obj
    [Header("DEBUG")]
    public Text debugText;
    public bool IAmDebugging;

    // Colors
    [HideInInspector]
    public Color colorBasique;
    [HideInInspector]
    public Color colorVirgule; // Color(1f, 0.6f, 0f); Color(80f / 255f, 138f / 255f, 50f / 255f);
    [HideInInspector]
    public Color colorPoint; // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);
    [HideInInspector]
    public Color colorExclamation; // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);
    [HideInInspector]
    public Color colorInterrogation; // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);
    [HideInInspector]
    public Color colorDeuxPoints; // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);
    [HideInInspector]
    public Color colorPointVirgule; // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);

    // Data Export
    private string fullFolderName;
    private string recapContent;
    private int frames;
    private int errorNum;


    // Start is called before the first frame update
    void Start()
    {
        // DATA IMPORT
        scrGlobal globalScript = GameObject.Find("Global").GetComponent<scrGlobal>();
        if (!IAmDebugging) {
            TextFile = globalScript.file;
            CorrectFile = globalScript.animTextFile;
            useSpecial = globalScript.isSpecial;
            SpecialFile = globalScript.specialFile;
            dualAnim = globalScript.nivAntiOubli;
            pointLimit = globalScript.pointLimit;
            virguleLimit = globalScript.virguleLimit;
            exclamationLimit = globalScript.exclamationLimit;
            interrogationLimit = globalScript.interrogationLimit;
            deuxpointsLimit = globalScript.deuxpointsLimit;
            pointvirguleLimit = globalScript.pointvirguleLimit;
        } else {
            globalScript.playerName = "MICHEL";
            globalScript.levelNum = 4;
        }



        colorBasique = new Color(0f, 0f, 0f);
        colorVirgule = new Color(80f / 255f, 138f / 255f, 50f / 255f); // Color(1f, 0.6f, 0f); Color(80f / 255f, 138f / 255f, 50f / 255f);
        colorPoint = new Color(131f / 255f, 208f / 255f, 245f / 255f); // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);
        //à modifier
        colorExclamation = new Color(131f / 255f, 208f / 255f, 245f / 255f); // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);
        colorInterrogation = new Color(131f / 255f, 208f / 255f, 245f / 255f); // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);
        colorPointVirgule = new Color(80f / 255f, 138f / 255f, 50f / 255f); // Color(1f, 0.6f, 0f); Color(80f / 255f, 138f / 255f, 50f / 255f);
        colorDeuxPoints = new Color(131f / 255f, 208f / 255f, 245f / 255f); // Color(0.9f, 0.9f, 0.5f); Color(131f / 255f, 208f / 255f, 245f / 255f);


        cursorStart = new Vector3(-(lineWidth/2) - spaceSize, textFloor, 0); //cursor.transform.localPosition;
        cursor.transform.localPosition = cursorStart;
        cursor.gameObject.SetActive(!dualAnim);




        init_anim = dualAnim;
        if (!dualAnim) {
            // Classic mode

            textFloor = 450f;

            canTouchPonct = true;

            s = new List<string>();
            words = new List<string>();

            correctText = TextFile.text;
            
            CutsWordsDual(TextFile, s, words);
            
            // animation log
            animationLog.gameObject.SetActive(showLog);
            animationLog.text = "Les clients ont hâte de manger votre plat !";

            // creates separators list
            separators = new string[words.Count];
            for (int i = 0; i < separators.Length; i++) separators[i] = "";
            wordsObj = new GameObject[words.Count];
            slots = new GameObject[words.Count];
            // Places the words
            float W = 0f; // width cursor
            float H = 0f; // height cursor
            (float, float) thing = placesWords(words, slots, wordsObj, W, H, 1);
            W = thing.Item1;
            H = thing.Item2;

            GameObject virguleGen = GameObject.Find("Virgule Gen");
            GameObject pointGen = GameObject.Find("Point Gen");
            GameObject exclamationGen = GameObject.Find("Exclamation Gen");
            GameObject interrogationGen = GameObject.Find("Interrogation Gen");
            GameObject pointvirguleGen = GameObject.Find("point Virgule Gen");
            GameObject deuxpointsGen = GameObject.Find("Deux Points Gen");

            if (hideGen) // irrelevant?
            {
                virguleGen.SetActive(false);
                pointGen.SetActive(false);
                exclamationGen.SetActive(false);
                interrogationGen.SetActive(false);
                pointvirguleGen.SetActive(false);
                deuxpointsGen.SetActive(false);
            }

            // reads potential special file and adds unmovable or undeletable separators
            // incroyablement brut, on n'en parlera pas
            if (useSpecial) // may be irrelevant?
            {
                string st = SpecialFile.text;
                sp = st.Split('|');

                bool canMoved = sp[0].Equals("true");
                bool canDeleted = sp[1].Equals("true");

                
                GameObject block;
                Vector3 vect = new Vector3();

                string[] ss = sp[2].Split(';');
                for (int i = 0; i < ss.Length; i++)
                {
                    //separators[i] = ss[i];
                    switch (ss[i])
                    {
                        case ",":
                            block = virguleGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                            slots[i].GetComponent<scrSlot>().SendPonct(",");

                            vect = slots[i].transform.position + quelle_ponct("Virgule");
                            break;
                        case ".":
                            block = pointGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                            slots[i].GetComponent<scrSlot>().SendPonct(".");

                            vect = slots[i].transform.position + quelle_ponct("Point");
                            break;
                        case "!":
                            block = exclamationGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                            slots[i].GetComponent<scrSlot>().SendPonct("!");

                            vect = slots[i].transform.position + quelle_ponct("Exclamation");
                            break;
                        case "?":
                            block = interrogationGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                            slots[i].GetComponent<scrSlot>().SendPonct("?");

                            vect = slots[i].transform.position + quelle_ponct("Interrogation");
                            break;
                        case ";":
                            block = pointvirguleGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                            slots[i].GetComponent<scrSlot>().SendPonct(";");

                            vect = slots[i].transform.position + quelle_ponct("Point Virgule");
                            break;
                        case ":":
                            block = deuxpointsGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                            slots[i].GetComponent<scrSlot>().SendPonct(":");

                            vect = slots[i].transform.position + quelle_ponct("Deux Points");
                            break;
                        default:
                            block = null;
                            break;
                    }
                    if (block != null)
                    {
                        block.GetComponent<scrDragAndDrop>().dragging = false;
                        block.GetComponent<scrDragAndDrop>().canBeMoved = canMoved;
                        block.GetComponent<scrDragAndDrop>().canBeDeleted = canDeleted;
                        slots[i].GetComponent<scrSlot>().isUsed = true;
                        block.GetComponent<scrDragAndDrop>().willSnap = true;
                        //Vector3 vect = slots[i].transform.position;
                        //vect.y -= 25;
                        block.GetComponent<scrDragAndDrop>().ogPos = vect;
                        block.GetComponent<scrDragAndDrop>().snapPos = vect;
                        block.transform.position = vect;
                        block.GetComponent<scrDragAndDrop>().col = slots[i].GetComponent<BoxCollider2D>();

                    }
                }
            } // END OF SPECIAL
            // END OF CLASSIC MODE


        } else {
            // Dual Mode --------------------------------------------------

            textFloor = 75f;
            animationObj.gameObject.SetActive(true);
            animationObj.GetComponent<Animator>().SetInteger("Niveau",globalScript.levelNum);

            //hideGen = true; // dans le cas avec toutes les ponct placées?
            //canTouchPonct = false; // jusque la fin de l'animation jouée
            //showLog = false;
            canTouchPonct = true; // will move

            s = new List<string>();
            words = new List<string>();

            correctText = CorrectFile.text;

            CutsWordsDual(TextFile, s, words);

            // animation log
            animationLog.gameObject.SetActive(false); //??

            // creates separators list
            separators = new string[words.Count]; // ??
            for (int i = 0; i < separators.Length; i++) separators[i] = ""; // ??
            wordsObj = new GameObject[words.Count];
            slots = new GameObject[words.Count];

            // Places the words
            float W = 0f; // width cursor
            float H = 0f; // height cursor
            (float, float) thing = placesWords(words, slots, wordsObj, W, H, 1);
            W = thing.Item1;
            H = thing.Item2;

            GameObject virguleGen = GameObject.Find("Virgule Gen");
            GameObject pointGen = GameObject.Find("Point Gen");
            GameObject exclamationGen = GameObject.Find("Exclamation Gen");
            GameObject interrogationGen = GameObject.Find("Interrogation Gen");
            GameObject pointvirguleGen = GameObject.Find("Point Virgule Gen");
            GameObject deuxpointsGen = GameObject.Find("Deux Points Gen");

            if (hideGen) // irrelevant?
            {
                virguleGen.SetActive(false);
                pointGen.SetActive(false);
                exclamationGen.SetActive(false);
                interrogationGen.SetActive(false);
                pointvirguleGen.SetActive(false);
                deuxpointsGen.SetActive(false);
            }

            // special version dual
            GameObject block;
            Vector3 vect = new Vector3();
            int k = -1; // shh
            for (int i = 0; i < s.Count; i++) {
                switch (s[i]) {
                    case ",":
                        block = virguleGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                        slots[k].GetComponent<scrSlot>().SendPonct(",");

                        vect = slots[k].transform.position + quelle_ponct("Virgule");
                        break;
                    case ".":
                        block = pointGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                        slots[k].GetComponent<scrSlot>().SendPonct(".");

                        vect = slots[k].transform.position + quelle_ponct("Point");
                        break;
                    case "!":
                        block = exclamationGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                        slots[k].GetComponent<scrSlot>().SendPonct("!");

                        vect = slots[k].transform.position + quelle_ponct("Exclamation");
                        break;
                    case "?":
                        block = interrogationGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                        slots[k].GetComponent<scrSlot>().SendPonct("?");

                        vect = slots[k].transform.position + quelle_ponct("Interrogation");
                        break;
                    case ":":
                        block = deuxpointsGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                        slots[k].GetComponent<scrSlot>().SendPonct(":");

                        vect = slots[k].transform.position + quelle_ponct("Deux Points");
                        break;
                    case ";":
                        block = pointvirguleGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                        slots[k].GetComponent<scrSlot>().SendPonct(";");

                        vect = slots[k].transform.position + quelle_ponct("Point Virgule");
                        break;
                    default:
                        block = null;
                        k++;
                        break;
                }
                if (block != null)
                {
                    block.GetComponent<scrDragAndDrop>().dragging = false;
                    block.GetComponent<scrDragAndDrop>().canBeMoved = canBeMoved;
                    block.GetComponent<scrDragAndDrop>().canBeDeleted = canBeDeleted;
                    slots[k].GetComponent<scrSlot>().isUsed = true;
                    block.GetComponent<scrDragAndDrop>().willSnap = true;
                    block.GetComponent<scrDragAndDrop>().col = slots[k].GetComponent<BoxCollider2D>();

                    //Vector3 vect = slots[k].transform.position;
                    //vect.y -= 25;
                    block.GetComponent<scrDragAndDrop>().ogPos = vect;
                    block.GetComponent<scrDragAndDrop>().snapPos = vect;
                    block.transform.position = vect;
                }
            }


            // END OF DUAL MODE
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


        RefreshText();
        HideSlots(new Vector2(0,0),"init");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (movingCursor)
        {
            Vector3 trans = cursor.transform.localPosition;
            trans.x += cursorSpeed;

            if ( trans.x > (lineWidth/2) )
            {
                trans.x = -(lineWidth / 2f);
                trans.y -= lineJump;
                lineCursor++;
                //Debug.Log(lineCursor);
            }
            if (lineCursor >= lineToStop)
            {
                // last line
                if (trans.x > posToStop - 40) // HARD FIX 
                {
                    // play animations
                    trans.x = posToStop; // stop
                    movingCursor = false;
                    
                    //trans = cursorStart; // reset

                    /*if (pointTropTot) Debug.Log("<color=orange>(MAIGRE)</color> Point trop tôt");
                    if (manquePoint) Debug.Log("<color=orange>(GROS)</color> Manque de point");

                    if (tropVirgule) Debug.Log("<color=orange>(FEU)</color> Trop de virgules");
                    if (pasAssezVirgule) Debug.Log("<color=orange>(FADE)</color> Pas assez de virgules");
                    if (mauvaiseVirgule) Debug.Log("<color=orange>(CONFUS)</color> Mauvais placement de virgule");
*/
                    //prhase correcte
                    if (!pointTropTot && !manquePoint && !tropVirgule && !pasAssezVirgule && !mauvaiseVirgule)
                    {
                        // CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT
                        //Debug.Log("<color=green>CORRECT!</color>");
                        
                        
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
                        recapContent += "\nTerminé en " + frames/60 + " secondes avec " + errorNum + " erreur(s).";
                        scrGlobal globalScript = GameObject.Find("Global").GetComponent<scrGlobal>();
                        System.IO.File.WriteAllText(fullFolderName + "/Niveau"+globalScript.levelNum+".txt", recapContent);
                        
                        // Updates level unlocked + 1
                        // ???
                        // ???
                        // ???

                        // Hides all buttons and shows the "continue" one, which is the first child
                        ButtonLayer.SetActive(true);
                        ButtonLayer.transform.GetChild(0).gameObject.SetActive(true);
                        for (int i = 1; i < ButtonLayer.transform.childCount; i++) {
                            ButtonLayer.transform.GetChild(i).gameObject.SetActive(false);
                        }

                        // Unlocks next level
                        globalScript.levelunlocked[globalScript.levelNum] = true;
                    }

                    // recap phrase for the animation recall
                    if (pointTropTot || manquePoint || tropVirgule || pasAssezVirgule || mauvaiseVirgule)
                    {
                        // ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR
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
                        if ((tropVirgule || pasAssezVirgule || mauvaiseVirgule) && (pointTropTot || manquePoint)) { recap += " et"; }
                        if (pointTropTot) { recap += " trop léger."; recapContent += "- point trop tôt\n"; }
                        if (manquePoint) { recap += " trop lourd."; recapContent += "- manque d'un point\n"; }
                        animationLog.text = recap;

                        // data recap
                        recapContent += currentText + "\n";
                        recapContent += "\n----------------------------------------------\n";
                        errorNum++;
                        ButtonLayer.SetActive(true);

                        canTouchPonct = true;
                    }
                    
                    //Début animation après fin validation
                    AnimationFondu();
                }
            }
            // cursor gets back to original position
            cursor.transform.localPosition = trans;

        } else {
            // counts frames (for the timer) when the cursor isn't moving, to be fair
            frames++;
        }
        //anim curseur miam
        cursor.GetComponent<Animator>().SetBool("Validation",movingCursor);
    }

    private (float, float) placesWords(List<string> words_e, GameObject[] slots_e, GameObject[] wordsObj_e, float W, float H, int INDEX)
    {
        taillePolice = taille_Police(correctText.Length);
        WordPrefab.GetComponentInChildren<TextMeshProUGUI>().fontSize=taillePolice;
        WordPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().fontSize=taillePolice;
        
        for (int i = 0; i < words_e.Count; i++) // NEEDS: words, slots, wordsObj
        {
            GameObject wordObj = Instantiate(WordPrefab);
            wordObj.GetComponentInChildren<TextMeshProUGUI>().text = words_e[i];

            float pw = wordObj.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

            if (W + pw > lineWidth) // if the word is too long for the line size
            {
                W = 0f; // moves cursors to the next line
                H -= lineJump;
                lineToStop++;
                lineNumber++;
            }
            W += (pw) + spaceSize;

            GameObject slot = Instantiate(SlotPrefab);
            slot.transform.SetParent(canvas.transform);
            slot.transform.localPosition = new Vector3(W - (spaceSize / 2) - (lineWidth / 2), textFloor + H, 0); // test

            slot.GetComponent<scrSlot>().INDEX = i;
            slot.GetComponent<scrSlot>().txtManager = gameObject;
            slots_e[i] = slot;

            //wordObj.transform.parent = Canvas.transform;
            wordObj.transform.SetParent(canvas.transform);
            wordObj.transform.localPosition = new Vector3(W - (pw / 2) - spaceSize - (lineWidth / 2), textFloor + H, 0);

            wordObj.GetComponent<Image>().enabled = false;
            //wordObj.GetComponent<Image>().enabled = true; //trust me, it works

            wordsObj_e[i] = wordObj;

            posToStop = W - (lineWidth / 2) - 10f;

        } // end of word placement
        return (W, H);
    }

    private string RefreshTextN(string curr, string corr, List<string> W, string[] SEP, GameObject[] WOBJ)
    {
        debugText.text = "";
        curr = "";

        for (int i = 0; i < W.Count; i++)
        {
            
            if(SEP[i].Equals("!") || SEP[i].Equals("?") || SEP[i].Equals(":") || SEP[i].Equals(";"))
            {
                curr += W[i] + " " + SEP[i] + " ";
            }else
            {
                curr += W[i] + SEP[i] + " ";
            }
            // adds an UPPERCASE letter to the next word
            if (i > 0 && (SEP[i - 1].Equals(".") || SEP[i - 1].Equals("!") || SEP[i - 1].Equals("?"))) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
            {
                string mot = WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().text;
                WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().text = mot.Substring(0, 1).ToUpper() + mot.Substring(1, mot.Length - 1);
                
                //bazar brayan
                GameObject test3 = WOBJ[i].transform.GetChild(0).transform.GetChild(1).gameObject;
                GameObject test4 = WOBJ[i].transform.GetChild(0).transform.GetChild(0).gameObject;
                
                
                test3.GetComponentInChildren<TextMeshProUGUI>().text = mot.ToUpper()[0].ToString();
                float test3w = test3.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;
                float test3h = test3.GetComponentInChildren<TextMeshProUGUI>().preferredHeight;
                float test3size = Mathf.Max(test3w,test3h);
                

                //position
                test4.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(test3w/2,0);
                test3.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(test3w/2,0);
                //taille
                test4.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(test3size, test3size);
                test3.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(test3w,test3h);
                
                test4.SetActive(true);
                test3.SetActive(true);
                //fin bazar
            }
            else
            {
                WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().text = W[i];
                //bazar brayan
                WOBJ[i].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                WOBJ[i].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                //fin bazar
            }

            if (addColors)
            {
                // resets color
                WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().color = colorBasique;
                WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

                // colors the left part of the block
                switch (SEP[i])
                {
                    case ",":
                        int j = i - 1;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().color = colorVirgule;
                        while (j >= 0 && SEP[j].Equals("")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                        {
                            WOBJ[j].GetComponentInChildren<TextMeshProUGUI>().color = colorVirgule;
                            j--;
                        }
                        break;
                    case ".":
                        // find the first (0;i-1) and last word (i), and make something
                        // if i+1 exists, uppercase the letter (what will lowercase it?)
                        int k = i - 1;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().color = colorPoint;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                        while (k >= 0 && !SEP[k].Equals(".")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                        {
                            if (WOBJ[k].GetComponentInChildren<TextMeshProUGUI>().color == colorBasique)
                            {
                                WOBJ[k].GetComponentInChildren<TextMeshProUGUI>().color = colorPoint;
                            }
                            WOBJ[k].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                            k--;
                        }
                        break;
                    case "!":
                        // find the first (0;i-1) and last word (i), and make something
                        // if i+1 exists, uppercase the letter (what will lowercase it?)
                        int l = i - 1;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().color = colorExclamation;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                        while (l >= 0 && !SEP[l].Equals("!")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                        {
                            if (WOBJ[l].GetComponentInChildren<TextMeshProUGUI>().color == colorBasique)
                            {
                                WOBJ[l].GetComponentInChildren<TextMeshProUGUI>().color = colorExclamation;
                            }
                            WOBJ[l].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                            l--;
                        }
                        break;
                    case "?":
                        // find the first (0;i-1) and last word (i), and make something
                        // if i+1 exists, uppercase the letter (what will lowercase it?)
                        int m = i - 1;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().color = colorInterrogation;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                        while (m >= 0 && !SEP[m].Equals("?")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                        {
                            if (WOBJ[m].GetComponentInChildren<TextMeshProUGUI>().color == colorBasique)
                            {
                                WOBJ[m].GetComponentInChildren<TextMeshProUGUI>().color = colorInterrogation;
                            }
                            WOBJ[m].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                            m--;
                        }
                        break;
                    case ":":
                        int n = i - 1;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().color = colorDeuxPoints;
                        while (n >= 0 && SEP[n].Equals("")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                        {
                            WOBJ[n].GetComponentInChildren<TextMeshProUGUI>().color = colorDeuxPoints;
                            n--;
                        }
                        break;
                    case ";":
                        int o = i - 1;
                        WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().color = colorPointVirgule;
                        while (o >= 0 && SEP[o].Equals("")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                        {
                            WOBJ[o].GetComponentInChildren<TextMeshProUGUI>().color = colorPointVirgule;
                            o--;
                        }
                        break;
                    default:
                        //nothing
                        break;
                }
            }
        }
        curr = curr.Substring(0, curr.Length - 1); // to remove the last space
        debugText.text = curr;

        if (curr.ToLower().Equals(corr.ToLower())) // to lower pour éviter les ennuis atm
        {
            debugText.color = new Color(0, 200, 0);
        }
        else
        {
            debugText.color = new Color(0, 0, 0);
        }
        return curr;
    }

    public void RefreshText()
    {
        currentText = RefreshTextN(currentText, correctText, words, separators, wordsObj);
    }

    public void ValiderClick()
    {
        if (!dualAnim) Valider();
        else ValiderDual();
    }

    public void ValiderDual() {
        bool dual_reussite = correctText == currentText;
        Debug.Log(correctText);
        Debug.Log(currentText);
        animationObj.GetComponent<Animator>().SetBool("Validation",true);
        animationObj.GetComponent<Animator>().SetBool("Reussite",dual_reussite);
        //Debug.Log(correctText == currentText);
        
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

    private void Valider()
    {
        //Debug.Log("Validation...");
        Debug.Log(correctText);
        Debug.Log(currentText);
        bool willStop = false;
        bool forceStop = false;
        int vN = 0; // nombre de virgule
        int vNC = 0; // nombre de virgule dans la correction
        int vNBP = 0; // nombre de virgule bien placée


        pointTropTot = false;
        manquePoint = false;
        tropVirgule = false;
        pasAssezVirgule = false;
        mauvaiseVirgule = false;


        int i = 0; // indice current
        int j = 0; // indice correct

        //parcours
        while ( (i < currentText.Length) && !forceStop && (!((willStop && (currentText[i].Equals('.') || currentText[i].Equals('!') || currentText[i].Equals('?'))))) )
        {
            vN += currentText[i].Equals(',') ? 1 : 0;
            vNBP += (currentText[i].Equals(',') && correctText[j].Equals(',')) ? 1 : 0;
            if (!willStop)
            {
                vNC += correctText[j].Equals(',') ? 1 : 0;
            }

            if (currentText[i].Equals('.') || currentText[i].Equals('!') || currentText[i].Equals('?') )
            {
                if (!correctText[j].Equals(currentText[i]))
                {
                    // point trop tôt
                    forceStop = true;
                    pointTropTot = true;
                } else
                {
                    if (vN != vNC)
                    {
                        // mauvais nombre de virgule
                        forceStop = true;
                        pasAssezVirgule = (vN < vNC);
                        tropVirgule = !pasAssezVirgule;
                    } else
                    {
                        if (vNBP != vNC)
                        {
                            // nombre de virgule bon, mais mal placées
                            forceStop = true;
                            mauvaiseVirgule = true;
                        }
                    }
                }
            }

            //absence
            if (correctText[j].Equals(',') && !currentText[i].Equals(','))
            {
                willStop = true;
                //mauvaiseVirgule = true;
            }
            if (correctText[j].Equals('.') && !currentText[i].Equals('.'))
            {
                willStop = true;
                manquePoint = true;
            }
            if (correctText[j].Equals('!') && !currentText[i].Equals('!'))
            {
                willStop = true;
                manquePoint = true;
            }
            if (correctText[j].Equals('?') && !currentText[i].Equals('?'))
            {
                willStop = true;
                manquePoint = true;
            }

            bool currPonct = (currentText[i].Equals(',') || currentText[i].Equals('.') || currentText[i].Equals('!') || currentText[i].Equals('?'));
            bool corrPonct = (correctText[j].Equals(',') || correctText[j].Equals('.') || correctText[j].Equals('!') || correctText[j].Equals('?'));

            if (currPonct && !corrPonct)
            {
                i++;
            }
            if (!currPonct && corrPonct)
            {
                j++;
            }
            if (currPonct && corrPonct)
            {
                i++;
                j++;
            }
            if (!currPonct && !corrPonct)
            {
                i++;
                j++;
            }
        }
        // post search error check
        if (willStop || !currentText[currentText.Length - 1].Equals(correctText[correctText.Length - 1]))
        {
            if ( (i == currentText.Length) && !currentText[currentText.Length - 1].Equals(correctText[correctText.Length - 1]))
            {
                manquePoint = true;
            }

            if (i < currentText.Length && !manquePoint) //  && !willStop ------ bruteforce, i know
            {
                if ((currentText[i].Equals('.') && !correctText[j].Equals('.')))
                {
                    pointTropTot = true;
                }
                if ((currentText[i].Equals('!') && !correctText[j].Equals('!')))
                {
                    pointTropTot = true;
                }
                if ((currentText[i].Equals('?') && !correctText[j].Equals('?')))
                {
                    pointTropTot = true;
                }
            }

            if (vN != vNC)
            {
                pasAssezVirgule = (vN < vNC);
                tropVirgule = !pasAssezVirgule;
            }
            else
            {
                if (vNBP != vNC)
                {
                    mauvaiseVirgule = true;
                }
            }
        }

        // <- here were the debug.logs of the errors

        if (pointTropTot || manquePoint || tropVirgule || pasAssezVirgule || mauvaiseVirgule)
        {
            // il faut trouver l'indice du point sur lequel s'arrêter
            int cCount = 0;

            string t = currentText.Substring(0, i);
            char[] a = { ',', '.', '!', '?',';',':' };
            int l = t.Split(a).Length;
            
            int cIndex = i - 1 - l; // char sur lequel s'arrêter

            //Debug.Log("char de fin : " + cIndex);
            //Debug.Log("char de fin : " + cIndex);
            //Debug.Log("" + currentText[i]);
            //Debug.Log("i : " + currentText[cIndex]);
            bool found = false;
            int k = 0;
            while ( (!found) && (k < words.Count) )
            {
                cCount += words[k].Length;
                if (cCount >= cIndex)
                {
                    found = true;
                } else
                {
                    k++;
                    cCount++; // espace
                }
            }
            //Debug.Log("-> " + k);
            //Debug.Log("->" + wordsObj[k].GetComponentInChildren<TextMeshProUGUI>().text);
            //Debug.Log("->" + wordsObj[k].transform.localPosition.y);
            
            lineToStop = (Mathf.Abs( (int) wordsObj[k].transform.localPosition.y - textFloor) ) / lineJump;

            posToStop = slots[k].transform.localPosition.x;


        } else
        {
            lineToStop = lineNumber;
            posToStop = slots[slots.Length-1].transform.localPosition.x;
        }
        //Debug.Log("FIN DE LA VALIDATION (" + i + "/" + currentText.Length + ")");

        //Debug.Log("lineToStop:"+lineToStop + " posToStop:" + posToStop);

        animationLog.text = "Les clients sont en train de tester votre plat...";

        movingCursor = true;
        canTouchPonct = false;
        cursor.transform.localPosition = cursorStart;
        lineCursor = 0;
        cursor.transform.SetAsLastSibling();
        ButtonLayer.SetActive(false);
    }


    public void ShowSlots(Vector2 taillePot, string pot)
    {
        Vector2 slot_pos = quel_pot(pot);

        if(dualAnim && init_anim)HideSlots(taillePot, pot);

        for (int i = 0; i < slots.Length; i++)
        {
            if(!slots[i].GetComponentInChildren<scrSlot>().isUsed)slots[i].GetComponentInChildren<Image>().enabled = true;

            slots[i].GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta = taillePot;
            
            Vector3 V3_slot = slots[i].GetComponentInChildren<RectTransform>().position;
            slots[i].GetComponentInChildren<RectTransform>().position = new Vector3(V3_slot.x+slot_pos.x,V3_slot.y+slot_pos.y,V3_slot.z);
            
        }

        
    }

    public void HideSlots(Vector2 taillePot, string pot)
    {
        Vector2 slot_pos = quel_pot(pot);
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponentInChildren<Image>().enabled = false;

            Vector3 V3_slot = slots[i].GetComponentInChildren<RectTransform>().position;
            slots[i].GetComponentInChildren<RectTransform>().position = new Vector3(V3_slot.x-slot_pos.x,V3_slot.y-slot_pos.y,V3_slot.z);

        }

        if(pot.Equals("init"))init_anim = false;
    }


    private void CutsWordsDual(TextAsset TF, List<string> S, List<string> W)
    {
        string word = "";
        bool skipNext = false;
        bool lowerNext = false;
        
        for (int i = 0; i < TF.text.Length; i++)
        {
            switch (TF.text[i])
            {
                case ',':
                    // VIRGULE
                    S.Add(word);
                    W.Add(word);
                    S.Add(",");
                    word = "";
                    skipNext = true; // we skip the next char because it is a ' '
                    break;
                case '.':
                    // POINT
                    S.Add(word);
                    W.Add(word);
                    S.Add(".");
                    word = "";
                    skipNext = true; // we skip the next char because it is a ' '
                    lowerNext = true; // we lower the next upper case (this is to avoid lowering any first name or the first letter of the text)
                    break;
                case '!':
                    // EXCLAMATION
                    S.Add("!");
                    skipNext = true; // we skip the next char because it is a ' '
                    lowerNext = true; // we lower the next upper case (this is to avoid lowering any first name or the first letter of the text)
                    break;
                case '?':
                    // INTERROGATION
                    S.Add("?");
                    skipNext = true; // we skip the next char because it is a ' '
                    lowerNext = true; // we lower the next upper case (this is to avoid lowering any first name or the first letter of the text)
                    break;
                case ':':
                    // DEUX POINTS
                    S.Add(":");
                    skipNext = true; // we skip the next char because it is a ' '
                    break;
                case ';':
                    // POINT VIRGULE
                    S.Add(";");
                    skipNext = true; // we skip the next char because it is a ' '
                    break;
                /*case '«':
                    word += TF.text[i]+" ";
                    skipNext = true;
                    break;
                case '»':
                    word += TF.text[i]+" ";
                    skipNext = true;
                    break;*/
                case ' ':
                    // ESPACE
                    if (!skipNext)
                    {
                        S.Add(word);
                        W.Add(word);
                        word = "";
                    }
                    else        
                    {
                        skipNext = false;
                    }
                    break;
                default://Lettre
                    word += TF.text[i];
                    if (lowerNext)
                    {
                        // at this point, if everything works, the "word" is only a letter long
                        word = word.ToLower();
                        lowerNext = false;
                    }
                    break;
            }
        }
    }

    public void GoToMap() {
        SceneManager.LoadScene("MapScene");
    }

    public void AnimationFondu()
    {
        //raccourci variable
        point_reussite = !pointTropTot && !manquePoint;
        virgule_reussite = !tropVirgule && !pasAssezVirgule && !mauvaiseVirgule;
        textreussite = point_reussite && virgule_reussite;

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
        fondu.GetComponent<Animator>().SetBool("Actif",false);

        cursor.transform.localPosition = cursorStart;
        
        //reset
        client_virgule.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0);
        client_virgule.GetComponent<RectTransform>().sizeDelta = new Vector2(478,844);
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
        if(pot == "init"){
            return new Vector2 (0,0);
        }
        else if(pot == "Deux Points"){
            return new Vector2 (0,20);
        }
        else if(pot == "Point Virgule"){
            return new Vector2 (-5,10);
        }
        else if(pot == "Exclamation"){
            return new Vector2 (-5,25);
        }
        else if(pot == "Interrogation"){
            return new Vector2 (-5,25);
        }
        else{
            return new Vector2 (-5,0);
        }
    }

    private Vector3 quelle_ponct(string ponct)
    {
        if(ponct == "Point"){
            return new Vector3 (-5,-25,0);
        }
        else if(ponct == "Virgule"){
            return new Vector3 (-5,-25,0);
        }
        else if(ponct == "Deux Points"){
            return new Vector3 (0,-5,0);
        }
        else if(ponct == "Point Virgule"){
            return new Vector3 (-5,-15,0);
        }
        else if(ponct == "Exclamation"){
            return new Vector3 (-5,0,0);
        }
        else if(ponct == "Interrogation"){
            return new Vector3 (-5,0,0);
        }
        else{
            return new Vector3 (0,0,0);
        }
    }

    public float taille_Police(int lg_text)
    {
        float ratio = textFloor/lg_text;
        if(lg_text>textFloor)
        {
            lineJump = lineJump*ratio;
            spaceSize = spaceSize*ratio;
            return ratio*taillePolice;
        }
        return taillePolice;
        
    }
}
