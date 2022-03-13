using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scrCredits : MonoBehaviour
{
    public TextMeshProUGUI TextFin;

    private void Start()
    {
        afficheCredits();
    }

    public void afficheCredits()
    {
        TextFin.text = "Benjamin, \n Brayan, \n François, \n Gabriel, \n Hugo, \n Julie. \n" +
            "Avec le soutien de Véronique Paolacci.";
    }
}
