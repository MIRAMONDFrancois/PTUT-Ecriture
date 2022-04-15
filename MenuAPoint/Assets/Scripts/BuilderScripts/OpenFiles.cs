using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using TMPro;
using SFB;

public class OpenFiles : MonoBehaviour
{
    public TMP_InputField TextToEdit;
    public GameObject TextQuiBouge;

    void Start()
    {
        if(scrGlobal.Instance.GameBuilderText == null)return;

        TextToEdit.text = scrGlobal.Instance.GameBuilderText.text;
    }

    public void OpenFileBrowser()
    {   
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);

        if (path.Length != 0)
        {
            TextAsset textAsset = new TextAsset(File.ReadAllText(path[0]));
            TextToEdit.text = textAsset.text;
            scrGlobal.Instance.FromGameBuilder = true;
            scrGlobal.Instance.GameBuilderText = textAsset;
        }
    }

    void Update()
    {
        TextQuiBouge.transform.localPosition = Vector3.zero;
    }
}
