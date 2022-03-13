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
        if(nameLevel.Equals("synopsisScene"))nameLevel="MapScene";

        SceneManager.LoadScene(nameLevel);
    }

    public void intro()
    {
        GameObject.Find("Global").GetComponent<scrGlobal>().SetIntro();
        GameObject.Find("Global").GetComponent<scrGlobal>().intro = true;
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
