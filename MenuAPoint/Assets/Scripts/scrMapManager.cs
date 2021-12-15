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

    private void modifValue(int level, bool isSpecial, bool nivAntiOubli, TextAsset text, TextAsset specialText, int pointLimit, int virguleLimit)
    {
        GameObject.Find("Global").GetComponent<scrGlobal>().file = text;
        GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = level;
        GameObject.Find("Global").GetComponent<scrGlobal>().specialFile = specialText;
        GameObject.Find("Global").GetComponent<scrGlobal>().isSpecial = isSpecial;
        GameObject.Find("Global").GetComponent<scrGlobal>().nivAntiOubli = nivAntiOubli;
        GameObject.Find("Global").GetComponent<scrGlobal>().pointLimit = pointLimit;
        GameObject.Find("Global").GetComponent<scrGlobal>().virguleLimit = virguleLimit;
    }

    public void selectLevel()
    {
        int levelActuel = 0;
        switch(EventSystem.current.currentSelectedGameObject.name)
        {
            case "Level01":
                modifValue(1, false, false, Resources.Load("Texts/Level1") as TextAsset, null, 2, 0);
                levelActuel = 1;
                break;
            case "Level02":
                modifValue(2, false, true, Resources.Load("Texts/Level2") as TextAsset, null, 1, 0);
                levelActuel = 2;
                break;
            case "Level03":
                modifValue(3, true, false, Resources.Load("Texts/dual_1A") as TextAsset, Resources.Load("Texts/dual_1B") as TextAsset, 0, 0);
                levelActuel = 3;
                break;
            case "Level04":
                modifValue(4, false, false, Resources.Load("Texts/Level3") as TextAsset, null, 1, 3);
                levelActuel = 4;
                break;
            case "Level05":
                modifValue(5, false, false, Resources.Load("Texts/Level4") as TextAsset, null, 2, 3);
                levelActuel = 5;
                break;
            case "Level06":
                modifValue(6, true, true, Resources.Load("Texts/dual_2A") as TextAsset, Resources.Load("Texts/dual_2B") as TextAsset,0,0);
                levelActuel = 6;
                break;
            case "Level07":
                modifValue(7, false, false, Resources.Load("Texts/Level5") as TextAsset, null,1,1);
                levelActuel = 7;
                break;
            case "Level08":
                modifValue(8, true, true, Resources.Load("Texts/dual_3A") as TextAsset, Resources.Load("Texts/dual_3B") as TextAsset, 0, 0);
                break;
            case "Level09":
                modifValue(9, false, false, Resources.Load("Texts/Level6") as TextAsset, null, 2, 0);
                break;
            case "Level10":
                modifValue(10, false, true, Resources.Load("Texts/Level7") as TextAsset, null, 3, 5);
                break;
            case "Level11":
                modifValue(11, true, false, Resources.Load("Texts/dual_4A") as TextAsset, Resources.Load("Texts/dual_4B") as TextAsset, 0, 0);
                break;
            case "Level12":
                modifValue(12, false, false, Resources.Load("Texts/Level8") as TextAsset, null, 5, 4);
                break;
            case "Level13":
                modifValue(13, false, false, Resources.Load("Texts/Level9") as TextAsset, null, 4, 4);
                break;
            case "Level14":
                modifValue(14, true, true, Resources.Load("Texts/dual_5A") as TextAsset, Resources.Load("Texts/dual_5B") as TextAsset, 0, 0);
                break;
            case "Level15":
                modifValue(15, false, false, Resources.Load("Texts/Level10") as TextAsset, null, 0, 0);
                break;
            default:
                throw new ArgumentException("Element not found", nameof(EventSystem.current.currentSelectedGameObject.name));
        }

        if(checkUnlocked(levelActuel - 1))
            LoadNewScene();

    }

    private bool checkUnlocked(int level)
    {
        return GameObject.Find("Global").GetComponent<scrGlobal>().levelunlocked[level];
    }
}
