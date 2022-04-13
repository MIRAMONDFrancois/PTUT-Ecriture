using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class BonusReussite : MonoBehaviour
{
    public Image ImageCorrecte;
    public TextMeshProUGUI TextCorrect;

    void Start()
    {
        GetComponentInParent<ScrollAreaManager>().OnNiveauSelected += ShowOutline;

        if(!scrGlobal.Instance.FromBonusLevel)return;
        
        Joueurs j = scrGlobal.Instance.GetPlayer();

        if(j.niveauxBonusFinis[TextCorrect.text])
        {
            ImageCorrecte.color = Color.green;
        }else
        {
            ImageCorrecte.color = Color.red;
        }
                
    }
    
    public void ShowOutline()
    {
        ImageCorrecte.GetComponent<Outline>().enabled = false;
    }

    public void Unsub(ScrollAreaManager ceci)
    {
        ceci.OnNiveauSelected -= ShowOutline;
    }

    public void ApresClique()
    {
        GetComponentInParent<ScrollAreaManager>().OnNiveauSelected?.Invoke();

        ImageCorrecte.GetComponent<Outline>().enabled = true;

        scrGlobal.Instance.GameBuilderText = new TextAsset(File.ReadAllText(scrGlobal.Instance.chemin_bonus+"/"+TextCorrect.text));

        scrGlobal.Instance.NameBuilderText = TextCorrect.text;

    }
}
