using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NiveauxBonus
{
    public string nom;
    public bool indice = false;
    public int[] totalPonct = {-1,-1,-1,-1,-1,-1,-1};
    public int[] extraPonct = {0,0,0,0,0,0,0};

    public NiveauxBonus(string nom)
    {
        this.nom = nom;
    }
}
