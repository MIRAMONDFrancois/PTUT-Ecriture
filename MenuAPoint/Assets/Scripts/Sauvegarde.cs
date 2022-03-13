using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Sauvegarde : MonoBehaviour
{
    private Donnees GetDonnees()
    {
        TextAsset jsonFile = Resources.Load("donnees") as TextAsset;
        Donnees donnees = JsonUtility.FromJson<Donnees>(jsonFile.text);

        return donnees;
    }

    //Cration Dossier + Ajout dans .json
    public void CreationJoueur()
    {
        string nom = GameObject.Find("Global").GetComponent<scrGlobal>().playerName;
        Donnees data = GetDonnees();
        Joueurs j = new Joueurs();

        j.joueur = nom;

        data.donnees.Add(j);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("Assets/Resources/donnees.json", json);

        Directory.CreateDirectory ("./RESULTATS/"+nom);  
    }


    /*public void sauvegarde()
    {
        string nom = GameObject.Find("Global").GetComponent<scrGlobal>().playerName;
        string chemin = "Assets/Resources/Donnes/"+nom+"/";

        TextAsset jsonFile = Resources.Load("Donnes/"+nom+"/donnes") as TextAsset;
        Donnes obj = JsonUtility.FromJson<Donnes>(jsonFile.text);


        File.WriteAllText(chemin+"donnes.json", JsonUtility.ToJson(obj));
    }*/

    /*public void sauvegarde()
    {
        TextAsset jsonFile = Resources.Load("donnees") as TextAsset;

        Donnees donneesJson = JsonUtility.FromJson<Donnees>(jsonFile.text);
 
        foreach (Joueurs j in donneesJson.donnees)
        {
            Debug.Log(j.joueur);
        }
    }*/
}


    