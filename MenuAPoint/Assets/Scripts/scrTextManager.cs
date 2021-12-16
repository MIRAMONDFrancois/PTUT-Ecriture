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
    public GameObject client_virgule;
    public bool showLog;
    public Text animationLog;

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
    private int cursorSpeed = 9;
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

    [Header("Limited Ponct (0 = infinite)")]
    public int pointLimit;
    public int virguleLimit;

    [Header("Dual Animation mode")]
    public bool dualAnim;
    public TextAsset CorrectFile;
    public bool hideGen;


    // Text pos
    private float lineWidth = 1550f; // 
    private float textFloor = 350f; // (default: 350f; anim: 75f) vertical position of the top of the text
    private float spaceSize = 50f; // 
    private float lineJump = 80f;

    // Text errors
    private bool pointTropTot = false;
    private bool manquePoint = false;
    private bool tropVirgule = false;
    private bool pasAssezVirgule = false;
    private bool mauvaiseVirgule = false;

    // Debug text obj
    [Header("DEBUG")]
    public Text debugText;
    public bool IAmDebugging;

    // Colors
    [HideInInspector]
    public Color colorBasique;
    [HideInInspector]
    public Color colorVirgule; // Color(1f, 0.6f, 0f);
    [HideInInspector]
    public Color colorPoint; // Color(0.9f, 0.9f, 0.5f);

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
            useSpecial = globalScript.isSpecial;
            SpecialFile = globalScript.specialFile;
            dualAnim = globalScript.nivAntiOubli;
            pointLimit = globalScript.pointLimit;
            virguleLimit = globalScript.virguleLimit;
        } else {
            globalScript.playerName = "MICHEL";
            globalScript.levelNum = 4;
        }



        colorBasique = new Color(1f, 1f, 1f);
        colorVirgule = new Color(1f, 0.6f, 0f); // Color(1f, 0.6f, 0f);
        colorPoint = new Color(0.9f, 0.9f, 0.5f); // Color(0.9f, 0.9f, 0.5f);

        
        cursorStart = new Vector3(-(lineWidth/2) - spaceSize, textFloor, 0); //cursor.transform.localPosition;
        cursor.transform.localPosition = cursorStart;
        cursor.gameObject.SetActive(!dualAnim);





        if (!dualAnim) {
            // Classic mode

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

            if (hideGen) // irrelevant?
            {
                virguleGen.SetActive(false);
                pointGen.SetActive(false);
            }

            // reads potential special file and adds unmovable or undeletable separators
            // incroyablement brut, on n'en parlera pas
            if (useSpecial)
            {
                string st = SpecialFile.text;
                sp = st.Split('|');

                bool canMoved = sp[0].Equals("true");
                bool canDeleted = sp[1].Equals("true");

                
                GameObject block;


                string[] ss = sp[2].Split(';');
                for (int i = 0; i < ss.Length; i++)
                {
                    //separators[i] = ss[i];
                    switch (ss[i])
                    {
                        case ",":
                            block = virguleGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                            slots[i].GetComponent<scrSlot>().SendPonct(",");
                            break;
                        case ".":
                            block = pointGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                            slots[i].GetComponent<scrSlot>().SendPonct(".");
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
                        Vector3 vect = slots[i].transform.position;
                        vect.y -= 25;
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

            if (hideGen) // irrelevant?
            {
                virguleGen.SetActive(false);
                pointGen.SetActive(false);
            }

            // special version dual
            GameObject block;
            int k = -1; // shh
            for (int i = 0; i < s.Count; i++) {
                switch (s[i]) {
                    case ",":
                        block = virguleGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                        slots[k].GetComponent<scrSlot>().SendPonct(",");
                        break;
                    case ".":
                        block = pointGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
                        slots[k].GetComponent<scrSlot>().SendPonct(".");
                        break;
                    default:
                        block = null;
                        k++;
                        break;
                }
                if (block != null)
                {
                    block.GetComponent<scrDragAndDrop>().dragging = false;
                    block.GetComponent<scrDragAndDrop>().canBeMoved = true; // DEPENDS !!!
                    block.GetComponent<scrDragAndDrop>().canBeDeleted = false; // DEPENDS !!!
                    slots[k].GetComponent<scrSlot>().isUsed = true;
                    block.GetComponent<scrDragAndDrop>().willSnap = true;
                    Vector3 vect = slots[k].transform.position;
                    vect.y -= 25;
                    block.GetComponent<scrDragAndDrop>().ogPos = vect;
                    block.GetComponent<scrDragAndDrop>().snapPos = vect;
                    block.transform.position = vect;
                    block.GetComponent<scrDragAndDrop>().col = slots[k].GetComponent<BoxCollider2D>();

                }
            }


            // END OF DUAL MODE
        } 
        



        /*
        if (false) // COMMENT PLACER UN POINT FIXE
        { 
            // same as special, but to add unmovable points to the text
            GameObject virguleGen = GameObject.Find("Virgule Gen");
            GameObject pointGen = GameObject.Find("Point Gen");
            GameObject block;

            // point 1
            block = pointGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
            slots[slots.Length - 1].GetComponent<scrSlot>().SendPonct(".");
            block.GetComponent<scrDragAndDrop>().dragging = false;
            block.GetComponent<scrDragAndDrop>().canBeMoved = false;
            block.GetComponent<scrDragAndDrop>().canBeDeleted = false;
            slots[slots.Length - 1].GetComponent<scrSlot>().isUsed = true;
            block.GetComponent<scrDragAndDrop>().willSnap = true;
            Vector3 vect = slots[slots.Length - 1].transform.position;
            vect.y -= 25;
            block.GetComponent<scrDragAndDrop>().ogPos = vect;
            block.GetComponent<scrDragAndDrop>().snapPos = vect;
            block.transform.position = vect;
            block.GetComponent<scrDragAndDrop>().col = slots[slots.Length - 1].GetComponent<BoxCollider2D>();

        }*/


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
        HideSlots();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (movingCursor)
        {
            Vector3 trans = cursor.transform.localPosition;
            trans.x += cursorSpeed;

            if ( trans.x > (lineWidth/2) - spaceSize )
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
                    trans = cursorStart; // reset

                    if (pointTropTot) Debug.Log("<color=orange>(MAIGRE)</color> Point trop tôt");
                    if (manquePoint) Debug.Log("<color=orange>(GROS)</color> Manque de point");

                    if (tropVirgule) Debug.Log("<color=orange>(FEU)</color> Trop de virgules");
                    if (pasAssezVirgule) Debug.Log("<color=orange>(FADE)</color> Pas assez de virgules");
                    if (mauvaiseVirgule) Debug.Log("<color=orange>(CONFUS)</color> Mauvais placement de virgule");

                    //prhase correcte
                    if (!pointTropTot && !manquePoint && !tropVirgule && !pasAssezVirgule && !mauvaiseVirgule)
                    {
                        // CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT CORRECT
                        Debug.Log("<color=green>CORRECT!</color>");
                        
                        
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
                    
                    AnimationFondu();
                    //curseur
                    //cursor.transform.SetAsFirstSibling();

                    //anim curseur reactions
                    cursor.GetComponent<Animator>().SetBool("Reussite",!pointTropTot && !manquePoint && !tropVirgule && !pasAssezVirgule && !mauvaiseVirgule);
                    cursor.GetComponent<Animator>().SetBool("Maigre",pointTropTot);
                    cursor.GetComponent<Animator>().SetBool("Gros",manquePoint);

                    
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
            curr += W[i] + SEP[i] + " ";

            // adds an UPPERCASE letter to the next word
            if (i > 0 && SEP[i - 1].Equals(".")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
            {
                string mot = WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().text;
                WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().text = mot.Substring(0, 1).ToUpper() + mot.Substring(1, mot.Length - 1);
            }
            else
            {
                WOBJ[i].GetComponentInChildren<TextMeshProUGUI>().text = W[i];
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
        Debug.Log(correctText == currentText);
        // jouer les animations, tout ça tout ça
    }

    private void Valider()
    {
        Debug.Log("Validation...");

        

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


        while ( (i < currentText.Length) && (! (willStop && currentText[i].Equals('.')) ) && !forceStop)
        {
            vN += currentText[i].Equals(',') ? 1 : 0;
            vNBP += (currentText[i].Equals(',') && correctText[j].Equals(',')) ? 1 : 0;
            if (!willStop)
            {
                vNC += correctText[j].Equals(',') ? 1 : 0;
            }

            if (currentText[i].Equals('.'))
            {
                if (!correctText[j].Equals('.'))
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

            bool currPonct = (currentText[i].Equals(',') || currentText[i].Equals('.'));
            bool corrPonct = (correctText[j].Equals(',') || correctText[j].Equals('.'));
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
        if (willStop || !currentText[currentText.Length - 1].Equals('.'))
        {
            if ( (i == currentText.Length) && !currentText[currentText.Length - 1].Equals('.'))
            {
                manquePoint = true;
            }

            if (i < currentText.Length && !manquePoint) //  && !willStop ------ bruteforce, i know
            {
                if (currentText[i].Equals('.') && !correctText[j].Equals('.'))
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
            char[] a = { ',', '.' };
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


    public void ShowSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponentInChildren<Image>().enabled = true;
        }
    }

    public void HideSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponentInChildren<Image>().enabled = false;
        }
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
                default:
                    // LETTER (ou point final)
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
        fondu.transform.SetAsLastSibling();
        cursor.transform.SetAsLastSibling();
        fondu.SetActive(true);
        fondu.GetComponent<Animator>().SetBool("Actif",true);
    }
}
