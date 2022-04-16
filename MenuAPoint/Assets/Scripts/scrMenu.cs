using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;

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

        string prenom = Regex.Replace(user_prenom.text, @"[^a-zA-Z0-9 ]", "");
        string nom = Regex.Replace(user_nom.text, @"[^a-zA-Z0-9 ]", "");
        
        scrGlobal.Instance.playerName = prenom+nom;
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
        Snblvl = userNbInput_Field.text;
        
        int.TryParse(Snblvl, out Inblvl);
        for (int i = 0; i < Inblvl; i++ ){
            scrGlobal.Instance.levelunlocked[i] = true;
        }
        


    }

    public IEnumerator LoadMenuSound()
    {
        AudioSource continueSource = GameObject.Find("ButtonSound").GetComponent<SFX_Script>().continuer;

        continueSource.GetComponent<SFX_Script>().continuer_sound();

        yield return new WaitForSeconds(continueSource.clip.length);

        continueSource.Stop();

        scrGlobal.Instance.ChargeJoueur();

        SceneManager.LoadScene("menuScene");
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadMenuSound());
    }
}
