using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrIndice : MonoBehaviour
{
    private bool isTargeted = false;
    private float normalScale = 1f;
    private float upScale = 1.2f;
    private float speedScale = 0.03f;

    public bool used = false;
    public GameObject Indice;

    void Start()
    {
        level3unlocked();
    }

    public void decrementIndice()
    {
        if (GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices > 0 && !used)
        {
            GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices--;

            used = true;
            GameObject.Find("GameManager").GetComponent<scrTextManager>().showIndice();
        }
    }

    public void level3unlocked()
    {
        if(GameObject.Find("Global").GetComponent<scrGlobal>().levelNum > 3)
        {
            
            Indice.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if(!used)
        {
            if (isTargeted && (transform.localScale.x < upScale)) {
            Vector3 newTrans = gameObject.transform.localScale;
            newTrans.x += speedScale;
            newTrans.y += speedScale;
            gameObject.transform.localScale = newTrans;
            } 
        }
        if (!isTargeted && (transform.localScale.x > normalScale)){
            Vector3 newTrans = gameObject.transform.localScale;
            newTrans.x -= speedScale;
            newTrans.y -= speedScale;
            gameObject.transform.localScale = newTrans;
        }
        
    }

    public void SetTargeted(bool value) {
        isTargeted = value;   
    }
}
