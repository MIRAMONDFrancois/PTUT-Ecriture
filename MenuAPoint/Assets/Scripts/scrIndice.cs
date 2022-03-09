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
    public GameObject Indice1;
    public GameObject Indice2;
    public GameObject Indice3;
    public GameObject Indice4;
    public GameObject Indice5;

    void Start()
    {
        level3unlocked();
        affichageIndices();
    }

    public void decrementIndice()
    {
        if (GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices > 0 && !used)
        {
            GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices--;

            used = true;
            GameObject.Find("GameManager").GetComponent<scrTextManager>().showIndice();
        }

        affichageIndices();
    }

    public void affichageIndices()
    {
        switch (GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices)
        {
            case 0:
                Indice1.SetActive(false);
                Indice2.SetActive(false);
                Indice3.SetActive(false);
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                break;
            case 1:
                Indice2.SetActive(false);
                Indice3.SetActive(false);
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                break;
            case 2:
                Indice3.SetActive(false);
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                break;
            case 3:
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                break;
            case 4:
                Indice5.SetActive(false);
                break;
            case 5:
                break;
            default:
                throw new System.Exception("Erreur indice");
        }
    }

    public void level3unlocked()
    {
        if (GameObject.Find("Global").GetComponent<scrGlobal>().levelNum <= 3)
            Indice.SetActive(false);
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
