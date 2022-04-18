using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrExit : MonoBehaviour
{
    // Start is called before the first frame update

    public IEnumerator ExitGameSound()
    {
        AudioSource continueSource = GameObject.Find("ButtonSound").GetComponent<SFX_Script>().continuer;

        continueSource.GetComponent<SFX_Script>().continuer_sound();

        yield return new WaitForSeconds(continueSource.clip.length);

        continueSource.Stop();

        Application.Quit();
    }
    public void QuitGame()
    {
        StartCoroutine(ExitGameSound());
    }
}
