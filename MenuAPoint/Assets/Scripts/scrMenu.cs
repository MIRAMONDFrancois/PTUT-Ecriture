using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class scrMenu : MonoBehaviour 
{   
    //pour le prenom
    public TextMeshProUGUI user_name;
    public TMP_InputField userInput_Field;

    // pour le mdp 
    public TMP_InputField userpwdInput_Field;

    //pour le choix du niveau
    public TMP_InputField userNbInput_Field;

    // pour pouvoir les setactive 
    public GameObject password ;
    public GameObject lvl;
    public GameObject opt;
    
    //le mot de passe 
    private string mdp = "genieponct";

    //les variable privates
    private string bidule;
    private int Inblvl;
    private string Snblvl;
    

    
    
    public void SetName(){
        scrGlobal truc = GameObject.Find("Global").GetComponent<scrGlobal>(); 
        user_name.text = userInput_Field.text;
        var leNom = user_name.text;
        leNom = leNom.Replace(" ","");
        leNom = leNom.Replace("\n","");
        truc.playerName = leNom;
        //Debug.Log("["+truc.playerName+"]"); // retour à la ligne de morts
    }

    public void Enterpwd(){

        bidule = userInput_Field.text;
        
        

        if (mdp.Trim().Equals(bidule)){
            password.SetActive(false);
            lvl.SetActive(true);
        }else
        {
            password.SetActive(false);
            opt.SetActive(true);
        }

    }
    public void Setlvl(){
        scrGlobal truc2 = GameObject.Find("Global").GetComponent<scrGlobal>(); 
        Snblvl = userNbInput_Field.text;

        
        
        int.TryParse(Snblvl, out Inblvl);
        for (int i = 0; i < Inblvl; i++ ){
            truc2.levelunlocked[i] = true;
        }
        


    }

}
