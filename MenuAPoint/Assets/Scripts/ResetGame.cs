using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public Scene level;
    public string levelName;

    public void LoadNewGame()
    {
        resetGame();
        SceneManager.LoadScene(levelName);
    }

    public void resetGame()
    {
        GameObject.Find("Global").GetComponent<scrGlobal>().levelunlocked.Clear();
        GameObject.Find("Global").GetComponent<scrGlobal>().setLevelUnlocked();
    }
    
}
