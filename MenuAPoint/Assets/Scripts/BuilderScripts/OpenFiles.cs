using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class OpenFiles : MonoBehaviour
{
    public TMP_InputField TextToEdit;
    public GameObject TextQuiBouge;
    [SerializeField] private Button _valider;


    void Start()
    {
        if(scrGlobal.Instance.GameBuilderText == null)return;

        TextToEdit.text = scrGlobal.Instance.GameBuilderText.text;
    }

    public void OpenFileBrowser()
    {   
        string path = EditorUtility.OpenFilePanel("Sélectionner un fichier .txt", "", "txt");
        if (path.Length != 0)
        {
            TextAsset textAsset = new TextAsset(File.ReadAllText(path));
            TextToEdit.text = textAsset.text;
            scrGlobal.Instance.FromGameBuilder = true;
            scrGlobal.Instance.GameBuilderText = textAsset;
        }
    }
    
    void FixedUpdate()
    {

        if(TextToEdit.text.Length == 0)
        {
            _valider.interactable = false;
            return;
        }

        if(TextToEdit.text[TextToEdit.text.Length-1].Equals('.'))
        {
            _valider.interactable = true;
        }else
        {
            _valider.interactable = false;
        }
    }

    void Update()
    {
        TextQuiBouge.transform.localPosition = Vector3.zero;
    }
}
