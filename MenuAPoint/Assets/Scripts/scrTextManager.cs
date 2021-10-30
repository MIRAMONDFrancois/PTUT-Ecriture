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
    public GameObject[] slots;
    [HideInInspector]
    public string[] separators;
    [HideInInspector]
    public GameObject[] wordsObj;

    private string correctText;
    private string currentText;

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

        cursorStart = new Vector3(-(lineWidth/2) - spaceSize, textFloor, 0); //cursor.transform.localPosition;
        cursor.transform.localPosition = cursorStart;

        // !!! Will not work with "..."

        s = new List<string>();
        words = new List<string>();

        correctText = TextFile.text;

        //  Filters the text into a list of words and separators
        string word = "";
        string fullText = ""; // may be useless
        bool skipNext = false;
        bool lowerNext = false;
        for (int i = 0; i < TextFile.text.Length; i++)
        {
            switch (TextFile.text[i])
            {
                case ',':
                    // VIRGULE
                    fullText += word + " ";
                    s.Add(word);
                    words.Add(word);
                    s.Add(",");
                    word = "";
                    skipNext = true; // we skip the next char because it is a ' '
                    break;
                case '.':
                    // POINT
                    fullText += word + " ";
                    s.Add(word);
                    words.Add(word);
                    s.Add(".");
                    word = "";
                    skipNext = true; // we skip the next char because it is a ' '
                    lowerNext = true; // we lower the next upper case (this is to avoid lowering any first name or the first letter of the text)
                    break;
                case ' ':
                    // ESPACE
                    if (!skipNext)
                    {
                        fullText += word + " ";
                        s.Add(word);
                        words.Add(word);
                        word = "";
                    } else
                    {
                        skipNext = false;
                    }
                    break;
                default:
                    // LETTER
                    word += TextFile.text[i];
                    if (lowerNext)
                    {
                        // at this point, if everything works, the "word" is only a letter long
                        word = word.ToLower();
                        lowerNext = false;
                    }
                    break;
            }
        }

        // creates a separators 
        separators = new string[words.Count];
        for (int i = 0; i < separators.Length; i++) separators[i] = "";
        wordsObj = new GameObject[words.Count];
        slots = new GameObject[words.Count];



        // Places the words
        float W = 0f; // width cursor
        float H = 0f; // height cursor
        for (int i = 0; i < words.Count; i++)
        {
            GameObject wordObj = Instantiate(WordPrefab);
            wordObj.GetComponentInChildren<TextMeshProUGUI>().text = words[i];
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
            slot.transform.localPosition = new Vector3(W - (spaceSize/2) - (lineWidth / 2), textFloor + H, 0); // test

            slot.GetComponent<scrSlot>().INDEX = i;
            slot.GetComponent<scrSlot>().txtManager = gameObject;
            slots[i] = slot;

            //wordObj.transform.parent = Canvas.transform;
            wordObj.transform.SetParent(canvas.transform);
            wordObj.transform.localPosition = new Vector3(W - (pw / 2) - spaceSize - (lineWidth/2), textFloor + H, 0);

            wordObj.GetComponent<Image>().enabled = false;
            //wordObj.GetComponent<Image>().enabled = true; //trust me, it works

            wordsObj[i] = wordObj;

            posToStop = W - (lineWidth / 2) - 10f;

        } // end of word placement


        // reads potential special file and adds unmovable or undeletable separators
        // incroyablement brut, on n'en parlera pas
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

    public void RefreshText()
    {
        debugText.text = "";
        for (int i = 0; i < words.Count; i++)
        {
            debugText.text += words[i] + separators[i] + " ";

            // adds an UPPERCASE letter to the next word
            if (i > 0 && separators[i - 1].Equals(".")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
            {
                string mot = wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().text;
                wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().text = mot.Substring(0, 1).ToUpper() + mot.Substring(1, mot.Length - 1);
            }
            else
            {
                wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().text = words[i];
            }

            if (addColors)
            { 
                // resets color
                wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().color = colorBasique;
                wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

                // colors the left part of the block
                switch (separators[i])
                {
                    case ",":
                        int j = i - 1;
                        wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().color = colorVirgule;
                        while (j >= 0 && separators[j].Equals("")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                        {
                            wordsObj[j].GetComponentInChildren<TextMeshProUGUI>().color = colorVirgule;
                            j--;
                        }
                        break;
                    case ".":
                        // find the first (0;i-1) and last word (i), and make something
                        // if i+1 exists, uppercase the letter (what will lowercase it?)
                        int k = i - 1;
                        wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().color = colorPoint;
                        wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                        while (k >= 0 && !separators[k].Equals(".")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                        {
                            if (wordsObj[k].GetComponentInChildren<TextMeshProUGUI>().color == colorBasique)
                            {
                                wordsObj[k].GetComponentInChildren<TextMeshProUGUI>().color = colorPoint;
                            }
                            wordsObj[k].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                            k--;
                        }
                        break;
                    default:
                        //nothing
                        break;
                }
            }

        }
        currentText = debugText.text.Substring(0, debugText.text.Length-1); // to remove the last space

        if (currentText.ToLower().Equals(correctText.ToLower())) // to lower pour éviter les ennuis atm
        {
            debugText.color = new Color(0, 200, 0);
        } else
        {
            debugText.color = new Color(0, 0, 0);
        }
    }


    public void Valider()
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
            if (!slots[i].GetComponent<scrSlot>().isUsed)
            {
                slots[i].GetComponentInChildren<Image>().enabled = false;
            }
        }
    }
}
