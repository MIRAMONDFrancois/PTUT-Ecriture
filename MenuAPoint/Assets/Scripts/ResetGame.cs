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
        scrGlobal.Instance.levelunlocked.Clear();
        scrGlobal.Instance.setLevelUnlocked();
    }
    
}
