using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PonctBuildEvents : MonoBehaviour
{
    //nombre tot
    public TextMeshProUGUI TextTot;

    //nombre de ponct supplémentaire
    public TextMeshProUGUI TextSuppl;
    [HideInInspector] public int IntSuppl; 

    //action sur image
    public Image InfinitePonct;
    public Image PlusPonct;
    public Image MoinsPonct;
    [HideInInspector] public bool InfiniteToggle;


    public void TextMoins()
    {
        IntSuppl--;
        TextSuppl.text = IntSuppl+"";

        if(IntSuppl <= 0)
        {
            IntSuppl = 0;
            MoinsPonct.gameObject.SetActive(false);
        }
    }

    public void TextPlus()
    {
        IntSuppl++;
        TextSuppl.text = IntSuppl+"";

        MoinsPonct.gameObject.SetActive(true);
    }

    public void PonctInfinite()
    {
        if(InfiniteToggle)
        {
            InfinitePonct.color = Color.black;
        }else
        {
            InfinitePonct.color = Color.green;
        }

        InfiniteToggle = !InfiniteToggle;
        
    }
}
