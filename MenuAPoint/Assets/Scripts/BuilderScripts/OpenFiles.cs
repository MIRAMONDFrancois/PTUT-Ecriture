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
    [SerializeField] private Button _valider;

    public void OpenFolderAcces()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
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
}
