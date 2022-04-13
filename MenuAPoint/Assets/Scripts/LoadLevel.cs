using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public Scene level;
    public string nameLevel;

    public void LoadThisLevel()
    {
        scrGlobal.Instance.FromBonusLevel = false;
        scrGlobal.Instance.FromGameBuilder = false;

        SceneManager.LoadScene(nameLevel);
    }

    public void LoadBonusLevel()
    {
        scrGlobal.Instance.FromBonusLevel = true;
        SceneManager.LoadScene(nameLevel);
    }

    public void LoadBuilderLevel()
    {
        scrGlobal.Instance.FromGameBuilder = true;
        SceneManager.LoadScene(nameLevel);
    }

    public void FinishBuilder()
    {
        scrGlobal.Instance.CreateTexteBuilder();
        SceneManager.LoadScene(nameLevel);
    }

    public void intro()
    {
        if(scrGlobal.Instance.intro)nameLevel = "MapScene";

        scrGlobal.Instance.SetIntro();
        scrGlobal.Instance.intro = true;

        scrGlobal.Instance.FromBonusLevel = false;
        scrGlobal.Instance.FromGameBuilder = false;
        SceneManager.LoadScene(nameLevel);
    }
    public void tuto()
    {
        scrGlobal.Instance.SetTuto();
    }

    public void QuitGame()
    {
        Debug.Log("quit");  
        Application.Quit();
    }

}
