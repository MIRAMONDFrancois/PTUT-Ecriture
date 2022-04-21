using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class scrButton : MonoBehaviour
{
    private bool isTargeted;
    private float normalScale = 1f;
    private float upScale = 1.2f;
    private float speedScale = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        isTargeted = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTargeted && (transform.localScale.x < upScale)) {
            Vector3 newTrans = gameObject.transform.localScale;
            newTrans.x += speedScale;
            newTrans.y += speedScale;
            gameObject.transform.localScale = newTrans;
        } 
        if (!isTargeted && (transform.localScale.x > normalScale)) {
            Vector3 newTrans = gameObject.transform.localScale;
            newTrans.x -= speedScale;
            newTrans.y -= speedScale;
            gameObject.transform.localScale = newTrans;
        }
    }

    public void SetTargeted(bool value) {
        isTargeted = value;
           
    }

    public IEnumerator PlaySound()
    {
        AudioSource soundSource = GameObject.Find("ButtonSound").GetComponent<SFX_Script>().continuer;

        soundSource.GetComponent<SFX_Script>().continuer_sound();

        yield return new WaitForSeconds(soundSource.clip.length);

        soundSource.Stop();
    }

    public void ButtonSound()
    {
        StartCoroutine(PlaySound());
    }
}
