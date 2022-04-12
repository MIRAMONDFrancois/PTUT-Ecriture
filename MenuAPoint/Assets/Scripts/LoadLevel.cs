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
        SceneManager.LoadScene(nameLevel);
    }

    public void LoadBonusLevel(bool play)
    {
        scrGlobal.Instance.FromGameBuilder = play;
        SceneManager.LoadScene(nameLevel);
    }

    public void intro()
    {
        if(scrGlobal.Instance.intro)nameLevel = "MapScene";

        scrGlobal.Instance.SetIntro();
        scrGlobal.Instance.intro = true;

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
