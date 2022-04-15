using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Script : MonoBehaviour
{
    public AudioSource pop;
    public AudioSource back;
    public AudioSource indice;
    public AudioSource clientconfu;
    public AudioSource clientfade;
    public AudioSource clientepice;
    public AudioSource reussite;

    public void pop_block()
    {
        int popnb = Random.Range(1,4);
        pop.clip = Resources.Load<AudioClip>("Sounds/pop"+popnb);
        
        pop.Play();
    }
    public void back_sound()
    {
        back.clip = Resources.Load<AudioClip>("Sounds/bouton_retour");

        back.Play();
    }

    public void indice_sound()
    {
        indice.clip = Resources.Load<AudioClip>("Sounds/indices");

        indice.Play();
    }

    public void client_confus()
    {
        clientconfu.clip = Resources.Load<AudioClip>("Sounds/client_confu");

        clientconfu.Play();
    }

    public void client_fade()
    {
        clientfade.clip = Resources.Load<AudioClip>("Sounds/client_fade");
        clientfade.Play();
    }

    public void client_epice()
    {
        clientepice.clip = Resources.Load<AudioClip>("Sounds/client_epice");
        clientepice.Play();
    }

    public void reussite_sound()
    {
        reussite.clip = Resources.Load<AudioClip>("Sounds/reussite");
        reussite.Play();
    }
}
