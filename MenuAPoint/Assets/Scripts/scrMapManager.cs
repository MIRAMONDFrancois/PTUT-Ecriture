using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class scrMapManager : MonoBehaviour
{

    [Header("Scene")]
    public Scene maScene;
    public string sceneName;
    //public string selectText;
    public TextAsset text;
    //public scrTextManager test;
    //public scrGlobal globalScript;
    //public TextAsset filename;

    public void LoadNewScene()
    {
       SceneManager.LoadScene(sceneName);
    }

    public void test()
    {
        GameObject.Find("Global").GetComponent<scrGlobal>().file = Resources.Load("Texts/dual_2A") as TextAsset;
        GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 2;
    }

}
