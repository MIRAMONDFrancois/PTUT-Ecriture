using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class OpenFiles : MonoBehaviour
{
    public TextMeshProUGUI TextToEdit;
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
            _valider.interactable = true;
            GameObject.Find("Global").GetComponent<scrGlobal>().FromGameBuilder = true;
            GameObject.Find("Global").GetComponent<scrGlobal>().GameBuilderText = textAsset;
        }
    }
}
