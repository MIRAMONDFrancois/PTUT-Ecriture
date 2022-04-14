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
        TextFin.text = "Benjamin Zanini, \n Brayan Weber, \n François Miramond, \n Gabriel Viau, \n Hugo Baudin, \n Julie Musseau. \n" +
            "Avec le soutien de Véronique Paolacci. \n" + "\n" +
            "Nos remerciements à la classe de CM1-CM2 de l'École Verte de l'Hippodrome de Montauban et leur enseignant, Maxime Combes";
    }
}
