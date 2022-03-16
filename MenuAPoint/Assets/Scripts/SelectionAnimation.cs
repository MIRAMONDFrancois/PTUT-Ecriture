using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SelectionAnimation : MonoBehaviour
{

    public VideoPlayer video;

    public void SelectAnimation(bool reussite, int niveau)
    {
        string chemin = "Video/Oubli/Incorrect/";
        if(reussite)
        {
            chemin = "Video/Oubli/Correct/";
        }

        video.clip = Resources.Load<VideoClip>(chemin+niveau) as VideoClip;
        video.Play();
    }
}
