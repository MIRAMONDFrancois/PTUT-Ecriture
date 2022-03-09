using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Script : MonoBehaviour
{
    public AudioSource pop;

    public void pop_block()
    {
        int popnb = Random.Range(1,4);
        pop.clip = Resources.Load<AudioClip>("Sounds/pop"+popnb);
        
        pop.Play();
    }
}
