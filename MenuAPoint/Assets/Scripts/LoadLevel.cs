using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public Scene level;
    public string nameLevel;

    public IEnumerator PlaySound()
    {
        AudioSource loadLevelSource = GameObject.Find("ButtonSound").GetComponent<SFX_Script>().continuer;

        loadLevelSource.GetComponent<SFX_Script>().continuer_sound();

        yield return new WaitForSeconds(loadLevelSource.clip.length);

        loadLevelSource.Stop();
        scrGlobal.Instance.FromBonusLevel = false;
        scrGlobal.Instance.FromGameBuilder = false;

        SceneManager.LoadScene(nameLevel);
    }

    public void LoadThisLevel()
    {
        StartCoroutine(PlaySound());
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
        if(scrGlobal.Instance.FromGameBuilder)scrGlobal.Instance.CreateTexteBuilder();

        SceneManager.LoadScene(nameLevel);
    }

    public IEnumerator Intro_Sound()
    {
        AudioSource soundSource = GameObject.Find("ButtonSound").GetComponent<SFX_Script>().continuer;

        soundSource.GetComponent<SFX_Script>().continuer_sound();

        yield return new WaitForSeconds(soundSource.clip.length);

        soundSource.Stop();

        if (scrGlobal.Instance.intro) nameLevel = "MapScene";

        scrGlobal.Instance.SetIntro();
        scrGlobal.Instance.intro = true;

        scrGlobal.Instance.FromBonusLevel = false;
        scrGlobal.Instance.FromGameBuilder = false;
        SceneManager.LoadScene(nameLevel);
    }

    public void intro()
    {
        StartCoroutine(Intro_Sound());
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
