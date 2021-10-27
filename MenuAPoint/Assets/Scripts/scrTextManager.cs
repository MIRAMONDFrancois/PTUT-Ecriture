using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scrTextManager : MonoBehaviour
{
    public TextAsset TextFile;
    public GameObject WordPrefab;
    public GameObject SlotPrefab;
    public GameObject canvas;

    private List<string> s; // list of words and separators
    private List<string> words; // list of words and separators
    [HideInInspector]
    public GameObject[] slots;
    public string[] separators;
    public GameObject[] wordsObj;

    private string correctText;
    private string currentText;

    private float lineWidth = 850f; // 800f
    private float textFloor = 200f; // vertical position of the top of the text
    private float spaceSize = 25f; // 20f

    public Text debugText;

    [HideInInspector]
    public Color colorBasique = new Color(1f, 1f, 1f);
    [HideInInspector]
    public Color colorVirgule = new Color(1f, 0.6f, 0f);
    [HideInInspector]
    public Color colorPoint = new Color(0.9f, 0.9f, 0.5f);




    // Start is called before the first frame update
    void Start()
    {
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
                H -= 50f;
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

        } // end of word placement

        RefreshText();
        HideSlots();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshText()
    {
        debugText.text = "";
        for (int i = 0; i < words.Count; i++)
        {
            debugText.text += words[i] + separators[i] + " ";
            wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().color = colorBasique;


            if (i > 0 && separators[i-1].Equals(".")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
            {
                string mot = wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().text;
                wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().text = mot.Substring(0, 1).ToUpper() + mot.Substring(1, mot.Length - 1);
            } else
            {
                wordsObj[i].GetComponentInChildren<TextMeshProUGUI>().text = words[i];
            }

            switch (separators[i])
            {
                case ",":
                    int j = i-1;
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
                    while (k >= 0 && !separators[k].Equals(".")) // using the fact that if the first condition is false, it directly stops, preventing the possible out of bounds error
                    {
                        if (wordsObj[k].GetComponentInChildren<TextMeshProUGUI>().color == colorBasique)
                        {
                            wordsObj[k].GetComponentInChildren<TextMeshProUGUI>().color = colorPoint;
                        }
                        k--;
                    }


                    break;



                default:
                    //nothing
                    break;
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
        // spawn le curseur
        // avancer le curseur
        //Debug.Log("DEBUT DE LA VALIDATION");

        bool willStop = false;
        bool forceStop = false;
        int vN = 0; // nombre de virgule
        int vNC = 0; // nombre de virgule dans la correction
        int vNBP = 0; // nombre de virgule bien placée
        

        //int pN = 0; // nombre de point
        //int pNC = 0; // nombre de point dans la correction

        bool pointTropTot = false;
        bool manquePoint = false;
        bool tropVirgule = false;
        bool pasAssezVirgule = false;
        bool mauvaiseVirgule = false;


        int i = 0; // indice current
        int j = 0; // indice correct


        while ( (i < currentText.Length) && (! (willStop && currentText[i].Equals('.')) ) && !forceStop)
        {
            //Debug.Log("Indice CURRENT" + i + " / " + currentText.Length);
            //Debug.Log("Indice CORRECT" + j + " / " + correctText.Length);
            vN += currentText[i].Equals(',') ? 1 : 0;
            vNBP += (currentText[i].Equals(',') && correctText[j].Equals(',')) ? 1 : 0;
            if (!willStop)
            {
                vNC += correctText[j].Equals(',') ? 1 : 0;
                //pN += currentText[i].Equals('.') ? 1 : 0;
                //pNC += correctText[j].Equals('.') ? 1 : 0;
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
        if (willStop || !currentText[currentText.Length - 1].Equals('.'))
        {
            if ( (i == currentText.Length) && !currentText[currentText.Length - 1].Equals('.'))
            {
                manquePoint = true;
            }

            if (i < currentText.Length)
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
        if (pointTropTot) Debug.Log("(MAIGRE) Point trop tôt");
        if (manquePoint) Debug.Log("(GROS) Manque de point");
        if (tropVirgule) Debug.Log("(FEU) Trop de virgules");
        if (pasAssezVirgule) Debug.Log("(FADE) Pas assez de virgules");
        if (mauvaiseVirgule) Debug.Log("(CONFUS) Mauvais placement de virgule");
        
        Debug.Log("FIN DE LA VALIDATION (" + i + "/" + currentText.Length + ")");
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
