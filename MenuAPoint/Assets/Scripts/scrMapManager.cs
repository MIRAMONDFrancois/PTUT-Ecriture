using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public GameObject[] levelTab;

    public void LoadNewScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void selectLevel()
    {
        switch(EventSystem.current.currentSelectedGameObject.name)
        {
            case "Level01":
                GameObject.Find("Global").GetComponent<scrGlobal>().file = Resources.Load("Texts/dual_1A") as TextAsset;
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 1;
                break;
            case "Level02":
                GameObject.Find("Global").GetComponent<scrGlobal>().file = Resources.Load("Texts/dual_2A") as TextAsset;
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 2;
                break;
            case "Level03":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 3;
                break;
            case "Level04":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 4;
                break;
            case "Level05":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 5;
                break;
            case "Level06":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 6;
                break;
            case "Level07":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 7;
                break;
            case "Level08":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 8;
                break;
            case "Level09":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 9;
                break;
            case "Level10":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 10;
                break;
            case "Level11":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 11;
                break;
            case "Level12":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 12;
                break;
            case "Level13":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 13;
                break;
            case "Level14":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 14;
                break;
            case "Level15":
                GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = 15;
                break;
            default:
                throw new ArgumentException("Element not found", nameof(EventSystem.current.currentSelectedGameObject.name));
        }

    }
}
