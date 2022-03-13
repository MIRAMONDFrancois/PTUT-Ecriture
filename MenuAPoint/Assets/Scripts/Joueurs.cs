using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Joueurs
{
     //Globales
    public string joueur;
    public int indiceRestant = 2;
    public bool intro = false;
    public bool tuto = false;

    //Niveaux
    public bool[] niveauxFinis = {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false};
    public bool[] indiceNiveau = {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false};
    public int[] essaies = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
    public int[] chronoNiveau = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

}
