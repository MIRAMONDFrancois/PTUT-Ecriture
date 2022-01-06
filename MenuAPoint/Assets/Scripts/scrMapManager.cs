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

    private void LevelSetBaseValues(int levelNum, TextAsset TextFile) {
        GameObject.Find("Global").GetComponent<scrGlobal>().levelNum = levelNum;
        GameObject.Find("Global").GetComponent<scrGlobal>().file = TextFile;
    }

    private void LevelSetSpecial(bool isSpecial, TextAsset specialFile) {
        GameObject.Find("Global").GetComponent<scrGlobal>().isSpecial = isSpecial;
        GameObject.Find("Global").GetComponent<scrGlobal>().specialFile = specialFile;
    }
    
    private void LevelSetLimitedGen(int pointLimit, int virguleLimit) {
        GameObject.Find("Global").GetComponent<scrGlobal>().pointLimit = pointLimit;
        GameObject.Find("Global").GetComponent<scrGlobal>().virguleLimit = virguleLimit;
    }

    private void LevelSetAnim(bool nivAntiOubli, TextAsset animTextFile, bool canBeMoved, bool canBeDeleted) {
        GameObject.Find("Global").GetComponent<scrGlobal>().nivAntiOubli = nivAntiOubli;
        GameObject.Find("Global").GetComponent<scrGlobal>().animTextFile = animTextFile;
        GameObject.Find("Global").GetComponent<scrGlobal>().canBeMoved = canBeMoved;
        GameObject.Find("Global").GetComponent<scrGlobal>().canBeDeleted = canBeDeleted;
    }

    public void selectLevel()
    {
        int levelActuel = 0;
        switch(EventSystem.current.currentSelectedGameObject.name)
        {
            case "Level01":
                LevelSetBaseValues(1, Resources.Load("Texts/Level1") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(2, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 0;
                break;

            case "Level02":
                LevelSetBaseValues(2, Resources.Load("Texts/Level5") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(2, 1);
                LevelSetAnim(false, null, false, false);
                levelActuel = 1;
                break;

            case "Level03":
                LevelSetBaseValues(3, Resources.Load("Texts/dual_1A") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(1, 1);
                LevelSetAnim(true, Resources.Load("Texts/dual_1B") as TextAsset, false, false);
                levelActuel = 2;
                break;

            case "Level04":
                LevelSetBaseValues(4, Resources.Load("Texts/Level3") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(1, 3);
                LevelSetAnim(false, null, false, false);
                levelActuel = 3;
                break;

            case "Level05":
                LevelSetBaseValues(5, Resources.Load("Texts/Level4") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(0, 3);
                LevelSetAnim(false, null, false, false);
                levelActuel = 4;
                break;

            case "Level06":
                LevelSetBaseValues(6, Resources.Load("Texts/dual_2A") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(0, 0);
                LevelSetAnim(true, Resources.Load("Texts/dual_2B") as TextAsset, false, false);
                levelActuel = 5;
                break;

            case "Level07":
                LevelSetBaseValues(7, Resources.Load("Texts/Level5") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(1, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 6;
                break;

            case "Level08":
                LevelSetBaseValues(8, Resources.Load("Texts/dual_3A") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(0, 0);
                LevelSetAnim(true, Resources.Load("Texts/dual_3B") as TextAsset, false, false);
                levelActuel = 7;
                break;

            case "Level09":
                LevelSetBaseValues(9, Resources.Load("Texts/Level6") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(1, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 8;
                break;

            case "Level10":
                LevelSetBaseValues(10, Resources.Load("Texts/Level7") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(2, 5);
                LevelSetAnim(false, null, false, false);
                levelActuel = 9;
                break;

            case "Level11":
                LevelSetBaseValues(11, Resources.Load("Texts/dual_4A") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(0, 0);
                LevelSetAnim(true, Resources.Load("Texts/dual_4B") as TextAsset, false, false);
                levelActuel = 10;
                break;

            case "Level12":
                LevelSetBaseValues(12, Resources.Load("Texts/Level8") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(5, 4);
                LevelSetAnim(false, null, false, false);
                levelActuel = 11;
                break;

            case "Level13":
                LevelSetBaseValues(13, Resources.Load("Texts/Level9") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(4, 4);
                LevelSetAnim(false, null, false, false);
                levelActuel = 12;
                break;

            case "Level14":
                LevelSetBaseValues(14, Resources.Load("Texts/dual_5A") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(0, 0);
                LevelSetAnim(true, Resources.Load("Texts/dual_5B") as TextAsset, false, false);
                levelActuel = 13;
                break;

            case "Level15":
                LevelSetBaseValues(15, Resources.Load("Texts/Level10") as TextAsset);
                LevelSetSpecial(false, null);
                LevelSetLimitedGen(0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 14;
                break;

            default:
                throw new ArgumentException("Element not found", nameof(EventSystem.current.currentSelectedGameObject.name));
        }

        //Debug.Log(levelActuel);

        if (levelActuel != 0)
        {
            if (checkUnlocked(levelActuel - 1))
                LoadNewScene();
        }
        else
            LoadNewScene();

    }

    private bool checkUnlocked(int level)
    {
        return GameObject.Find("Global").GetComponent<scrGlobal>().levelunlocked[level];
    }
}
