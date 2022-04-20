using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Joueurs
{
    //Globales
    public string joueur;
    public int indiceRestant = 0;
    public bool intro = false;
    public bool tuto = false;
    //public bool tutoIndice = false;

    //Niveaux
    public bool[] niveauxFinis = {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false};
    public bool[] indiceNiveau = {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false};
    public int[] essaies = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
    public int[] chronoNiveau = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

    //NiveauBonus
    public Dictionary<string, bool> niveauxBonusFinis = new Dictionary<string, bool>();
    public Dictionary<string, bool> indiceBonus = new Dictionary<string, bool>();
    public Dictionary<string, int> essaiesBonus = new Dictionary<string, int>();
    public Dictionary<string, int> chronoNiveauBonus = new Dictionary<string, int>();
}
