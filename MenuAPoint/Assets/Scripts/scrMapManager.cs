﻿using System;
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

    private void modifValue(int level, bool isSpecial, bool nivAntiOubli, TextAsset text, TextAsset specialText)
    {
        GameObject.Find("Global").GetComponent<scrGlobal>().file = text;
        GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = level;
        GameObject.Find("Global").GetComponent<scrGlobal>().specialFile = specialText;
        GameObject.Find("Global").GetComponent<scrGlobal>().isSpecial = isSpecial;
        GameObject.Find("Global").GetComponent<scrGlobal>().nivAntiOubli = nivAntiOubli;
    }

    public void selectLevel()
    {
        switch(EventSystem.current.currentSelectedGameObject.name)
        {
            case "Level01":
                modifValue(1, false, false, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level02":
                modifValue(2, true, false, Resources.Load("Texts/dual_2A") as TextAsset, Resources.Load("Texts/dual_2B") as TextAsset);
                break;
            case "Level03":
                modifValue(3, false, true, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level04":
                modifValue(4, false, false, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level05":
                modifValue(5, false, false, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level06":
                modifValue(6, true, true, Resources.Load("Texts/dual_2A") as TextAsset, Resources.Load("Texts/dual_2B") as TextAsset);
                break;
            case "Level07":
                modifValue(7, false, false, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level08":
                modifValue(8, true, true, Resources.Load("Texts/dual_1A") as TextAsset, Resources.Load("Texts/dual_1B") as TextAsset);
                break;
            case "Level09":
                modifValue(9, false, false, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level10":
                modifValue(10, true, false, Resources.Load("Texts/dual_2A") as TextAsset, Resources.Load("Texts/dual_2B") as TextAsset);
                break;
            case "Level11":
                modifValue(11, false, true, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level12":
                modifValue(12, false, false, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level13":
                modifValue(13, false, false, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            case "Level14":
                modifValue(14, true, true, Resources.Load("Texts/dual_1A") as TextAsset, Resources.Load("Texts/dual_1B") as TextAsset);
                break;
            case "Level15":
                modifValue(15, false, false, Resources.Load("Texts/dual_1A") as TextAsset, null);
                break;
            default:
                throw new ArgumentException("Element not found", nameof(EventSystem.current.currentSelectedGameObject.name));
        }

    }
}
