using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class scrMenu : MonoBehaviour 
{   
    //pour le prenom
    public TMP_InputField user_nom;
    public TMP_InputField user_prenom;
    private bool nom;
    private bool prenom;

    public TMP_InputField userInput_Field;
    public Button bouton;

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
    
    void FixedUpdate()
    {
        if(user_prenom.text=="")
        {
            prenom = false;
        }else
        {
            prenom = true;
        }

        if(user_nom.text=="")
        {
            nom = false;
        }else
        {
            nom = true;
        }

        if(prenom && nom)
        {
            bouton.interactable = true;
        }else
        {
            bouton.interactable = false;
        }
    }

    public void SetName(){
        scrGlobal truc = GameObject.Find("Global").GetComponent<scrGlobal>();

        truc.playerName = user_prenom.text+user_nom.text;
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

    public void LoadMenu()
    {
        GameObject.Find("Global").GetComponent<scrGlobal>().ChargeJoueur();

        SceneManager.LoadScene("menuScene");
    }
}
