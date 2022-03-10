using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluieCarotte : MonoBehaviour
{
    public GameObject carottePrefab;
    public float respawnTime = 1f;
    public float nb_carottes = 20;
    
    
    void Start()
    {
        StartCoroutine(carotteWave());
    }

    private void spawnCarotte()
    {
        GameObject a = Instantiate(carottePrefab);
        a.transform.position = new Vector2(Screen.width*Random.Range(.1f,.9f),Screen.height*Random.Range(1.5f,1.8f));
    }

    IEnumerator carotteWave()
    {
        while(true)
        {
            yield return new WaitForSeconds(respawnTime);
            for(int a=0;a<nb_carottes;a++)
            {
                spawnCarotte();
            }
            
        }      
    }
}
