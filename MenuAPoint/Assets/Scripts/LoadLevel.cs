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

    public void intro()
    {
        if(GameObject.Find("Global").GetComponent<scrGlobal>().intro)nameLevel = "MapScene";

        GameObject.Find("Global").GetComponent<scrGlobal>().SetIntro();
        GameObject.Find("Global").GetComponent<scrGlobal>().intro = true;

        SceneManager.LoadScene(nameLevel);
    }
    public void tuto()
    {
        GameObject.Find("Global").GetComponent<scrGlobal>().SetTuto();
    }

    public void QuitGame()
    {
        Debug.Log("quit");  
        Application.Quit();
    }

}
