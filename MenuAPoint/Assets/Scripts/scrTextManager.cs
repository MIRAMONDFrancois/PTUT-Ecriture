using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Custom")]
    public bool addColors = false; // if the blocks color the words or not

    [Header("Fixed Separators")]
    public bool useSpecial = false;
    public TextAsset SpecialFile;
    private string[] sp;

    // Cursor
    private bool movingCursor;
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

    [Header("Dual Phrases")]
    public bool dualPhrases;
    public TextAsset TextFile2;

    private List<string> s2; // list of words and separators
    private List<string> words2; // list of words
    [HideInInspector]
    public GameObject[] slots2; // list of slots
    [HideInInspector]
    public string[] separators2; // list of the content of the slots
    [HideInInspector]
    public GameObject[] wordsObj2; // list of the objects of the words

    private string correctText2; // correct string
    private string currentText2; // string composed of the words and separators

    // ---------------- dual


    // Text pos
    private float lineWidth = 1550f; // 
    private float textFloor = 350f; // vertical position of the top of the text
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
    public Text debugText2;

    // Colors
    [HideInInspector]
    public Color colorBasique;
    [HideInInspector]
    public Color colorVirgule; // Color(1f, 0.6f, 0f);
    [HideInInspector]
    public Color colorPoint; // Color(0.9f, 0.9f, 0.5f);




    // Start is called before the first frame update
    void Start()
    {
        colorBasique = new Color(1f, 1f, 1f);
        colorVirgule = new Color(1f, 0.6f, 0f); // Color(1f, 0.6f, 0f);
        colorPoint = new Color(0.9f, 0.9f, 0.5f); // Color(0.9f, 0.9f, 0.5f);

        if (!dualPhrases)
        {
            // NORMAL LEVEL

            cursorStart = new Vector3(-(lineWidth/2) - spaceSize, textFloor, 0); //cursor.transform.localPosition;
            cursor.transform.localPosition = cursorStart;


            s = new List<string>();
            words = new List<string>();

            correctText = TextFile.text;

            CutsWordsDual(TextFile, s, words);

        } else
        {
            // DUAL LEVEL

            // "removes" cursor and point gen
            cursor.SetActive(false);
            GameObject.Find("Point Gen").transform.position = new Vector3(-2000, 0, 0); // BEGONE

            // WORDS 1
            s = new List<string>();
            words = new List<string>();
            correctText = TextFile.text;

            CutsWordsDual(TextFile, s, words);

            // WORDS 2
            s2 = new List<string>();
            words2 = new List<string>();
            correctText2 = TextFile2.text;

            CutsWordsDual(TextFile2, s2, words2);
        }
        

        

        // creates separators list
        separators = new string[words.Count];
        for (int i = 0; i < separators.Length; i++) separators[i] = "";
        wordsObj = new GameObject[words.Count];
        slots = new GameObject[words.Count];

        if (dualPhrases)
        {
            separators2 = new string[words2.Count];
            for (int i = 0; i < separators2.Length; i++) separators2[i] = "";
            wordsObj2 = new GameObject[words2.Count];
            slots2 = new GameObject[words2.Count];
        }



        // Places the words
        float W = 0f; // width cursor
        float H = 0f; // height cursor
        (float, float) thing = placesWords(words, slots, wordsObj, W, H, 1);
        W = thing.Item1;
        H = thing.Item2;

        if (dualPhrases)
        {
            W = 0f;
            H -= lineJump * 2f;
            placesWords(words2, slots2, wordsObj2, W, H, 2);
        }



        // reads potential special file and adds unmovable or undeletable separators
        // incroyablement brut, on n'en parlera pas
        if (useSpecial)
        {
            string st = SpecialFile.text;
            sp = st.Split('|');

            bool canMoved = sp[0].Equals("true");
            bool canDeleted = sp[1].Equals("true");

            GameObject virguleGen = GameObject.Find("Virgule Gen");
            GameObject pointGen = GameObject.Find("Point Gen");
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
        if (dualPhrases)
        {
            // same as special, but to add unmovable points to the text
            GameObject virguleGen = GameObject.Find("Virgule Gen");
            GameObject pointGen = GameObject.Find("Point Gen");
            GameObject block;

            // point 1
            block = pointGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
            slots2[slots2.Length - 1].GetComponent<scrSlot>().fromSlotsOne = true;
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

            // point 2
            block = pointGen.GetComponent<scrBlockGenerator>().CreatesBlockForManager();
            slots2[slots2.Length - 1].GetComponent<scrSlot>().fromSlotsOne = false;
            slots2[slots2.Length - 1].GetComponent<scrSlot>().SendPonct(".");
            block.GetComponent<scrDragAndDrop>().dragging = false;
            block.GetComponent<scrDragAndDrop>().canBeMoved = false;
            block.GetComponent<scrDragAndDrop>().canBeDeleted = false;
            slots2[slots2.Length - 1].GetComponent<scrSlot>().isUsed = true;
            block.GetComponent<scrDragAndDrop>().willSnap = true;
            Vector3 vect2 = slots2[slots2.Length - 1].transform.position;
            vect2.y -= 25;
            block.GetComponent<scrDragAndDrop>().ogPos = vect2;
            block.GetComponent<scrDragAndDrop>().snapPos = vect2;
            block.transform.position = vect2;
            block.GetComponent<scrDragAndDrop>().col = slots2[slots2.Length - 1].GetComponent<BoxCollider2D>();
        }



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
                if (trans.x > posToStop)
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

                    if (!pointTropTot && !manquePoint && !tropVirgule && !pasAssezVirgule && !mauvaiseVirgule)
                    {
                        Debug.Log("<color=green>CORRECT!</color>");
                    }
                    ButtonLayer.SetActive(true);
                }
            }

            cursor.transform.localPosition = trans;

        }
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
            if (INDEX == 2) slot.GetComponent<scrSlot>().fromSlotsOne = false;
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

    private string RefreshTextN(string curr, string corr, List<string> W, string[] SEP, GameObject[] WOBJ, int INDEX)
    {
        if (INDEX == 1) debugText.text = ""; else debugText2.text = "";
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
        if (INDEX == 1) debugText.text = curr; else debugText2.text = curr;

        if (curr.ToLower().Equals(corr.ToLower())) // to lower pour éviter les ennuis atm
        {
            if (INDEX == 1) debugText.color = new Color(0, 200, 0); else debugText2.color = new Color(0, 200, 0);
        }
        else
        {
            if (INDEX == 1) debugText.color = new Color(0, 0, 0); else debugText2.color = new Color(0, 0, 0);
        }
        return curr;
    }

    public void RefreshText()
    {
        currentText = RefreshTextN(currentText, correctText, words, separators, wordsObj, 1);
        if (dualPhrases) currentText2 = RefreshTextN(currentText2, correctText2, words2, separators2, wordsObj2, 2);
    }

    public void ValiderClick()
    {
        if (!dualPhrases)
        {
            Valider();
        } else
        {
            ValiderDual();
        }
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

            //Debug.Log("line: " + lineToStop + "; xpos = " + posToStop);
        } else
        {
            lineToStop = lineNumber;
            posToStop = slots[slots.Length-1].transform.localPosition.x;
        }
        //Debug.Log("FIN DE LA VALIDATION (" + i + "/" + currentText.Length + ")");

        movingCursor = true;
        cursor.transform.localPosition = cursorStart;
        lineCursor = 0;
        cursor.transform.SetAsLastSibling();
        ButtonLayer.SetActive(false);
    }

    private void ValiderDual()
    {
        if ((currentText.Equals(correctText)) && (currentText2.Equals(correctText2)))
        {
            Debug.Log("Les deux phrases sont correctes");
        } else
        {
            Debug.Log("C'est incorrect...");
        }
    }

    public void ShowSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponentInChildren<Image>().enabled = true;
        }
        if (dualPhrases)
        {
            for (int i = 0; i < slots2.Length; i++)
            {
                slots2[i].GetComponentInChildren<Image>().enabled = true;
            }
        }
    }

    public void HideSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponentInChildren<Image>().enabled = false;
        }
        if (dualPhrases)
        {
            for (int i = 0; i < slots2.Length; i++)
            {
                slots2[i].GetComponentInChildren<Image>().enabled = false;
            }
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
}
