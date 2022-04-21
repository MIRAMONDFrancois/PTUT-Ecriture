using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberPonctBuilder : MonoBehaviour
{
    public GameObject ButtonOpen;
    public TMP_InputField PonctTexte;
    private List<PonctBuildEvents> ponctTot = new List<PonctBuildEvents>();
    
    public Image Indice;

    public Button testTextButton;
    public Transform[] ListePot;
    private int[] nbPot = {0,0,0,0,0,0,0};

    void Start()
    {
        #if (UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN ||UNITY_EDITOR)
            ButtonOpen.SetActive(true);
        #endif

        Init();
        StartCoroutine(ComptagePonct());
    }

    //(int pointLimit, int virguleLimit, int exclamationLimit,int interrogationLimit,int deuxpointsLimit,int pointvirguleLimit)
    IEnumerator ComptagePonct()
    {
        while(true)
        {
            for(int a=0;a<nbPot.Length;a++)
            {
                nbPot[a]=0;
            }
            
            StringBuilder sb = new StringBuilder(PonctTexte.text);
            sb.Replace("...","…");
            PonctTexte.text = sb.ToString();

            int taille = PonctTexte.text.Length;
            for(int a=0;a<taille;a++)
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
                    case '…':
                        nbPot[6]++;
                    break;
                }
            }

            ChangementNbPonct();

            yield return new WaitForSeconds(.5f);
        }
        
    }

    private void Init()
    {
        NiveauxBonus bonus = scrGlobal.Instance.GetBonusLevel();

        for(int a=0;a<ListePot.Length;a++)
        {
            ponctTot.Add(ListePot[a].GetComponent<PonctBuildEvents>());

            ponctTot[a].IntSuppl = bonus.extraPonct[a];
            ponctTot[a].TextSuppl.text = bonus.extraPonct[a] +"";

            if(bonus.totalPonct[a] == -1)
            {
                ponctTot[a].PonctInfinite();
            }

            if(ponctTot[a].IntSuppl > 0) ponctTot[a].MoinsPonct.gameObject.SetActive(true);
        }

        float alpha = bonus.indice ? 1f : 0.4f;
        Indice.color = new Color(1f,1f,1f,alpha);
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
        NiveauxBonus bonus = scrGlobal.Instance.GetBonusLevel();

        for(int a=0;a<nbPot.Length;a++)
        {
            bonus.extraPonct[a] = ponctTot[a].IntSuppl;
        }

        scrGlobal.Instance.pointLimit = ponctTot[0].InfiniteToggle ? -1 : ponctTot[0].IntSuppl + nbPot[0];
        scrGlobal.Instance.virguleLimit = ponctTot[1].InfiniteToggle ? -1 : ponctTot[1].IntSuppl + nbPot[1];
        scrGlobal.Instance.exclamationLimit = ponctTot[2].InfiniteToggle ? -1 : ponctTot[2].IntSuppl + nbPot[2];
        scrGlobal.Instance.interrogationLimit = ponctTot[3].InfiniteToggle ? -1 : ponctTot[3].IntSuppl + nbPot[3];
        scrGlobal.Instance.deuxpointsLimit = ponctTot[4].InfiniteToggle ? -1 : ponctTot[4].IntSuppl + nbPot[4];
        scrGlobal.Instance.pointvirguleLimit = ponctTot[5].InfiniteToggle ? -1 : ponctTot[5].IntSuppl + nbPot[5];
        scrGlobal.Instance.suspensionLimit = ponctTot[6].InfiniteToggle ? -1 : ponctTot[6].IntSuppl + nbPot[6];
    }

    public void LockIndice()
    {
        scrGlobal.Instance.LockIndice = !scrGlobal.Instance.LockIndice;

        float alpha = scrGlobal.Instance.LockIndice ? 1f : 0.4f;
        Indice.color = new Color(1f,1f,1f,alpha);
    }
}
