using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPonctBuilder : MonoBehaviour
{
    public TMP_InputField PonctTexte;
    private PonctBuildEvents ponctTot;

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
            ponctTot = ListePot[a].GetComponent<PonctBuildEvents>();

            if(ponctTot.InfiniteToggle)
            {
                ponctTot.TextTot.text = "-1";
            }else
            {
                ponctTot.TextTot.text = ponctTot.IntSuppl + nbPot[a] + "";
            }
        }
    }

    public void OnEssayage()
    {
        print(scrGlobal.Instance.NameBuilderText);

        scrGlobal.Instance.GameBuilderText = new TextAsset(PonctTexte.text);
        scrGlobal.Instance.pointLimit = ponctTot.IntSuppl + nbPot[0];
        scrGlobal.Instance.virguleLimit = ponctTot.IntSuppl + nbPot[1];
        scrGlobal.Instance.exclamationLimit = ponctTot.IntSuppl + nbPot[2];
        scrGlobal.Instance.interrogationLimit = ponctTot.IntSuppl + nbPot[3];
        scrGlobal.Instance.deuxpointsLimit = ponctTot.IntSuppl + nbPot[4];
        scrGlobal.Instance.pointvirguleLimit = ponctTot.IntSuppl + nbPot[5];
    }
}
