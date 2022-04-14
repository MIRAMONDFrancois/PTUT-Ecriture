using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPonctBuilder : MonoBehaviour
{
    public TMP_InputField PonctTexte;
    private List<PonctBuildEvents> ponctTot = new List<PonctBuildEvents>();

    public Transform[] ListePot;
    private int[] nbPot = {0,0,0,0,0,0};

    void Start()
    {
        Init();
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

    private void Init()
    {
        NiveauxBonus bonus = scrGlobal.Instance.GetBonusLevel();

        for(int a=0;a<ListePot.Length;a++)
        {
            ponctTot.Add(ListePot[a].GetComponent<PonctBuildEvents>());
            
            if(bonus.extraPonct[a] == -1)
            {
                ponctTot[a].PonctInfinite();
                ponctTot[a].TextTot.text = "-1";
            }else
            {
                ponctTot[a].TextTot.text = (bonus.extraPonct[a] - nbPot[a]) + "";
            }
        }
    }

    private void ChangementNbPonct()
    {
        for(int a=0;a<ListePot.Length;a++)
        {
            if(ponctTot[a].InfiniteToggle)
            {
                ponctTot[a].TextTot.text = "-1";
            }else
            {
                ponctTot[a].TextTot.text = ponctTot[a].IntSuppl + nbPot[a] + "";
            }
        }
    }

    public void OnEssayage()
    {
        scrGlobal.Instance.GameBuilderText = new TextAsset(PonctTexte.text);

        
        scrGlobal.Instance.pointLimit = ponctTot[0].InfiniteToggle ? -1 : ponctTot[0].IntSuppl + nbPot[0];
        scrGlobal.Instance.virguleLimit = ponctTot[1].InfiniteToggle ? -1 : ponctTot[1].IntSuppl + nbPot[1];
        scrGlobal.Instance.exclamationLimit = ponctTot[2].InfiniteToggle ? -1 : ponctTot[2].IntSuppl + nbPot[2];
        scrGlobal.Instance.interrogationLimit = ponctTot[3].InfiniteToggle ? -1 : ponctTot[3].IntSuppl + nbPot[3];
        scrGlobal.Instance.deuxpointsLimit = ponctTot[4].InfiniteToggle ? -1 : ponctTot[4].IntSuppl + nbPot[4];
        scrGlobal.Instance.pointvirguleLimit = ponctTot[5].InfiniteToggle ? -1 : ponctTot[5].IntSuppl + nbPot[5];
    }
}
