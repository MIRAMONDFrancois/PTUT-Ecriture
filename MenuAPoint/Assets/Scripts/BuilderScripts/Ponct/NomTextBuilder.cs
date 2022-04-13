using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NomTextBuilder : MonoBehaviour
{
    public TMP_InputField nomFichier;
    public TextMeshProUGUI placeHolder;

    void Start()
    {
        placeHolder.text = scrGlobal.Instance.NameBuilderText;
    }

    public void NameChange()
    {
        scrGlobal.Instance.NameBuilderText = nomFichier.text;
        
        if(nomFichier.text.Trim() == "")
        {
            scrGlobal.Instance.NameBuilderText = placeHolder.text;
        }
        print(scrGlobal.Instance.NameBuilderText);
    }
}
