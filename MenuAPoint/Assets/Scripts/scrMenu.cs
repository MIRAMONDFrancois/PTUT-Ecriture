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

        truc.playerName = user_prenom.text+"_"+user_nom.text;
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
        scrGlobal truc = GameObject.Find("Global").GetComponent<scrGlobal>();


        //Nom deja present ou creation
        if (Directory.Exists("Assets/Resources/RESULTATS/"+truc.playerName))
        {
            init_valeur();  
        }else
        {
            Directory.CreateDirectory ("Assets/Resources/RESULTATS/"+truc.playerName);
        }

        SceneManager.LoadScene("menuScene");
    }

    private void init_valeur()
    {
        scrGlobal truc = GameObject.Find("Global").GetComponent<scrGlobal>();

        TextAsset save = Resources.Load("RESULTATS/"+truc.playerName+"/Save") as TextAsset;

        bool saut = false;
        string numero = "";

        for(int a=0;a<save.text.Length;a++)
        {
            numero += save.text[a];

            if(save.text[a].Equals('\n'))
            {
                int valeur;
                int.TryParse(numero, out valeur);
                truc.levelunlocked[valeur]=true;
                
                numero = "";
            }

            
        }
    }

}
