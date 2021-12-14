using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class scrMenu : MonoBehaviour 
{   
    
    public TextMeshProUGUI user_name;
    public TMP_InputField userInput_Field;
    
    public void SetName(){
        scrGlobal  truc = GameObject.Find("Global").GetComponent<scrGlobal>(); 
        user_name.text = userInput_Field.text;
        truc.playerName = user_name.text;
    }


}
