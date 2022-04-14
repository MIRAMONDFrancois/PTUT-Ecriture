using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NiveauxBonus
{
    public string nom;
    public int[] extraPonct = {-1,-1,-1,-1,-1,-1};

    public NiveauxBonus(string nom)
    {
        this.nom = nom;
    }
}
