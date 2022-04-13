using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPonctBuilder : MonoBehaviour
{
    public TMP_InputField PonctTexte;

    public Transform[] ListePot;
    private int[] nbPot = {0,0,0,0,0,0};

    void Start()
    {
        StartCoroutine(ComptagePonct());
    }

    //(int pointLimit, int virguleLimit, int exclamationLimit,int interrogationLimit,int deuxpointsLimit,int pointvirguleLimit)
    IEnumerator ComptagePonct()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
        
            for(int a=0;a<nbPot.Length;a++)
            {
                nbPot[a]=0;
            }
            
            for(int a=0;a<PonctTexte.text.Length;a++)
            {
                switch (PonctTexte.text[a])
                {
                    case '.':
                        nbPot[0]++;
                    break;
                    case '!':
                        nbPot[2]++;
                    break;
                    case '?':
                        nbPot[3]++;
                    break;
                    case ',':
                        nbPot[1]++;
                    break;
                    case ':':
                        nbPot[4]++;
                    break;
                    case ';':
                        nbPot[5]++;
                    break;
                }
            }

            ChangementNbPonct();


        }
        
    }

    private void ChangementNbPonct()
    {
        for(int a=0;a<ListePot.Length;a++)
        {
            PonctBuildEvents ponctTemp = ListePot[a].GetComponent<PonctBuildEvents>();

            if(ponctTemp.InfiniteToggle)
            {
                ponctTemp.TextTot.text = "-1";
            }else
            {
                ponctTemp.TextTot.text = ponctTemp.IntSuppl + nbPot[a] + "";
            }
        }
    }



}
