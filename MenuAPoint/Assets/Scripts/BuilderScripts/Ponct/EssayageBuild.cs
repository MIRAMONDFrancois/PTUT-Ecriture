using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EssayageBuild : MonoBehaviour
{
    public TMP_InputField TextToEdit;
    public Button _valider;

    void FixedUpdate()
    {

        if(TextToEdit.text.Length == 0)
        {
            _valider.interactable = false;
            return;
        }

        
        if(TextToEdit.text[TextToEdit.text.Length-1].Equals('.') || TextToEdit.text[TextToEdit.text.Length-1].Equals('!') || TextToEdit.text[TextToEdit.text.Length-1].Equals('?'))
        {
            _valider.interactable = true;
        }else
        {
            _valider.interactable = false;
        }
    }
}
