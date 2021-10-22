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
    public GameObject Canvas;

    private List<string> s; // list of words
    private string[] separators = { ".", "," };

    private float lineWidth = 800f;
    private float textFloor = 200f; // vertical position of the top of the text
    private float spaceSize = 20f;


    // Start is called before the first frame update
    void Start()
    {
        // !!! Will not work with "..."

        s = new List<string>();

        //  Filters the text into a list of words
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
                    s.Add(",");
                    word = "";
                    skipNext = true; // we skip the next char because it is a ' '
                    break;
                case '.':
                    // POINT
                    fullText += word + " ";
                    s.Add(word);
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
        




        // Places the words
        float W = 0f; // width cursor
        float H = 0f; // height cursor
        for (int i = 0; i < s.Count; i++)
        {
            // Because this is the full text with separators
            // if the "word" to add is a separator, we skip it
            bool skip = false;
            for (int j = 0; j < separators.Length; j++)
            {
                if (s[i].Equals(separators[j])) {
                    skip = true;
                }
            }

            if (!skip)
            {
                GameObject wordObj = Instantiate(WordPrefab);
                wordObj.GetComponentInChildren<TextMeshProUGUI>().text = s[i];
                float pw = wordObj.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;

                if (W + pw > lineWidth) // if the word is too long for the line size
                {
                    W = 0f; // moves cursors to the next line
                    H -= 50f;
                }
                W += (pw) + spaceSize;

                GameObject slot = Instantiate(SlotPrefab);
                slot.transform.SetParent(Canvas.transform);
                slot.transform.localPosition = new Vector3(W - (spaceSize/2) - (lineWidth / 2), textFloor + H, 0); // test

                //wordObj.transform.parent = Canvas.transform;
                wordObj.transform.SetParent(Canvas.transform);
                wordObj.transform.localPosition = new Vector3(W - (pw / 2) - spaceSize - (lineWidth/2), textFloor + H, 0);

                wordObj.GetComponent<Image>().enabled = false;
                //wordObj.GetComponent<Image>().enabled = true; //trust me, it works

            }
        } // end of word placement





    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
