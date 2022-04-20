using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class NomTextBuilder : MonoBehaviour
{
    public TMP_InputField nomFichier;
    public TextMeshProUGUI placeHolder;

    void Start()
    {
        placeHolder.text = scrGlobal.Instance.NameBuilderText.Split('.')[0];
    }

    public void NameChange()
    {
        
        nomFichier.text = Regex.Replace(nomFichier.text, @"[^a-zA-Z0-9áéíóúÁÉÍÓÚâêîôûÂÊÎÔãõñÃÕÑçÇäëïöüÄËÏÖÜ_ ]", "");
        scrGlobal.Instance.NameBuilderText = nomFichier.text;
        
        if(nomFichier.text.Trim() == "")
        {
            scrGlobal.Instance.NameBuilderText = placeHolder.text;
        }
    }
}
