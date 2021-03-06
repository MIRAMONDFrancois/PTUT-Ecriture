using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class scrMapManager : MonoBehaviour
{
    

    [Header("Scene")]
    public Scene maScene;
    public string sceneName;
    public GameObject boutons;
    public TextAsset text;
    public GameObject[] levelTab;

    void Start()
    {
        Invoke("activerBouttons",.1f);
        //checkLevelActuel();//mettre au valider reussi du dernier niveau
    }

    public IEnumerator PlaySound_Back()
    {
        AudioSource continueSource = GameObject.Find("BackSound").GetComponent<SFX_Script>().back;

        continueSource.GetComponent<SFX_Script>().back_sound();

        yield return new WaitForSeconds(continueSource.clip.length);

        continueSource.Stop();

        SceneManager.LoadScene("menuScene");
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void backToMenu()
    {
        StartCoroutine(PlaySound_Back());
    }

    private void LevelSetBaseValues(int levelNum, TextAsset TextFile) {
        scrGlobal.Instance.levelNum = levelNum;
        scrGlobal.Instance.file = TextFile;

    }
    
    private void LevelSetLimitedGen(int pointLimit, int virguleLimit, int exclamationLimit,int interrogationLimit,int deuxpointsLimit,int pointvirguleLimit) {
        scrGlobal.Instance.pointLimit = pointLimit;
        scrGlobal.Instance.virguleLimit = virguleLimit;
        scrGlobal.Instance.exclamationLimit = exclamationLimit;
        scrGlobal.Instance.pointvirguleLimit = pointvirguleLimit;
        scrGlobal.Instance.deuxpointsLimit = deuxpointsLimit;
        scrGlobal.Instance.interrogationLimit = interrogationLimit;
    }

    private void LevelSetAnim(bool nivAntiOubli, TextAsset animTextFile, bool canBeMoved, bool canBeDeleted) {
        scrGlobal.Instance.nivAntiOubli = nivAntiOubli;
        scrGlobal.Instance.animTextFile = animTextFile;
        scrGlobal.Instance.canBeMoved = canBeMoved;
        scrGlobal.Instance.canBeDeleted = canBeDeleted;
    }

    public void selectLevel()
    {
        int levelActuel = 0;
        switch(EventSystem.current.currentSelectedGameObject.name)
        {
            case "Level01":
                LevelSetBaseValues(1, Resources.Load("Texts/Level1") as TextAsset);
                LevelSetLimitedGen(2, 0, 0, 0, 0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 0;
                break;

            case "Level02":
                LevelSetBaseValues(2, Resources.Load("Texts/Level2") as TextAsset);
                LevelSetLimitedGen(1, 3, 0, 0, 0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 1;
                break;

            case "Level03":
                LevelSetBaseValues(3, Resources.Load("Texts/dual_1A") as TextAsset);
                LevelSetLimitedGen(-1, -1, -1, -1, -1, -1);
                LevelSetAnim(true, Resources.Load("Texts/dual_1B") as TextAsset, false, false);
                levelActuel = 2;
                break;

            case "Level04":
                LevelSetBaseValues(4, Resources.Load("Texts/Level3") as TextAsset);
                LevelSetLimitedGen(2, 0, 1, 0, 0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 3;
                break;

            case "Level05":
                LevelSetBaseValues(5, Resources.Load("Texts/Level4") as TextAsset);
                LevelSetLimitedGen(1, 1, 1, 0, 0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 4;
                break;

            case "Level06":
                LevelSetBaseValues(6, Resources.Load("Texts/dual_2A") as TextAsset);
                LevelSetLimitedGen(-1, -1, -1, -1, -1, -1);
                LevelSetAnim(true, Resources.Load("Texts/dual_2B") as TextAsset, false, false);
                levelActuel = 5;
                break;

            case "Level07":
                LevelSetBaseValues(7, Resources.Load("Texts/Level5") as TextAsset);
                LevelSetLimitedGen(1, 1, 0, 0, 0, 1);
                LevelSetAnim(false, null, false, false);
                levelActuel = 6;
                break;

            case "Level08":
                LevelSetBaseValues(8, Resources.Load("Texts/dual_3A") as TextAsset);
                LevelSetLimitedGen(-1, -1, -1, -1, -1, -1);
                LevelSetAnim(true, Resources.Load("Texts/dual_3B") as TextAsset, false, false);
                levelActuel = 7;
                break;

            case "Level09":
                LevelSetBaseValues(9, Resources.Load("Texts/Level6") as TextAsset);
                LevelSetLimitedGen(3, 2, 0, 0, 0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 8;
                break;

            case "Level10":
                LevelSetBaseValues(10, Resources.Load("Texts/Level7") as TextAsset);
                LevelSetLimitedGen(2, 2, 1, 1, 0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 9;
                break;

            case "Level11":
                LevelSetBaseValues(11, Resources.Load("Texts/dual_4A") as TextAsset);
                LevelSetLimitedGen(-1, -1, -1, -1, -1, -1);
                LevelSetAnim(true, Resources.Load("Texts/dual_4B") as TextAsset, false, false);
                levelActuel = 10;
                break;

            case "Level12":
                LevelSetBaseValues(12, Resources.Load("Texts/Level8") as TextAsset);
                LevelSetLimitedGen(3, 2, 1, 0, 0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 11;
                break;

            case "Level13":
                LevelSetBaseValues(13, Resources.Load("Texts/Level9") as TextAsset);
                LevelSetLimitedGen(1, 3, 0, 1, 1, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 12;
                break;

            case "Level14":
                LevelSetBaseValues(14, Resources.Load("Texts/dual_5A") as TextAsset);
                LevelSetLimitedGen(-1, -1, -1, -1, -1, -1);
                LevelSetAnim(true, Resources.Load("Texts/dual_5B") as TextAsset, false, false);
                levelActuel = 13;
                break;

            case "Level15":
                LevelSetBaseValues(15, Resources.Load("Texts/Level10") as TextAsset);
                LevelSetLimitedGen(2, 8, 1, 2, 0, 0);
                LevelSetAnim(false, null, false, false);
                levelActuel = 14;
                break;

            default:
                throw new ArgumentException("Element not found", nameof(EventSystem.current.currentSelectedGameObject.name));
        }

        if (checkUnlocked(levelActuel))
            if(levelActuel == 2 && !(scrGlobal.Instance.tutoChecked))
            {
                SceneManager.LoadScene("sceneTutorielDouble");
                scrGlobal.Instance.tutoChecked = true;
            }
            else
                LoadNewScene();
            

    }

    private bool checkUnlocked(int level)
    {
        return scrGlobal.Instance.levelunlocked[level];
    }

    void activerBouttons()
    {
        //le script global contient 1 élément en plus
        for(int a=0;a<boutons.transform.childCount-1;a++)
        {
            bool niv_next = scrGlobal.Instance.levelunlocked[a+1];
            bool niv_now = scrGlobal.Instance.levelunlocked[a];
            boutons.transform.GetChild(a).GetComponent<Button>().interactable=niv_now;
            
            if(niv_now)
            {
                ColorBlock cb = boutons.transform.GetChild(a).GetComponent<Button>().colors;
                if(niv_next)
                {
                    cb.normalColor = Color.blue;
                    
                }else
                {
                    cb.normalColor = Color.red;
                }
                boutons.transform.GetChild(a).GetComponent<Button>().colors = cb;
            }
            
            
        }
    }

    void checkLevelActuel()
    {
        if (scrGlobal.Instance.levelunlocked[15])
        {
            SceneManager.LoadScene("endScene");
        }
    }
}
