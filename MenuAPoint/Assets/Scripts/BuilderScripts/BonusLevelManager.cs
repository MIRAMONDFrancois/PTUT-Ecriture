﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevelManager : MonoBehaviour
{
    public GameObject[] buttons;

    void Start()
    {
        if(scrGlobal.Instance.FromBonusLevel)
        {
            buttons[0].SetActive(true);
        }else
        {
            buttons[1].SetActive(true);
            buttons[2].SetActive(true);
        }

        #if (UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN ||UNITY_EDITOR)
            buttons[3].SetActive(true);
        #endif
    }

    public void OpenFolder()
    {
        Application.OpenURL(Application.persistentDataPath +"/NiveauxBonus");
    }

    public void SetPlayBonus()
    {   
        //niveau
        NiveauxBonus niveauxBonus = scrGlobal.Instance.GetBonusLevel();
        scrGlobal.Instance.pointLimit = niveauxBonus.extraPonct[0];
        scrGlobal.Instance.virguleLimit = niveauxBonus.extraPonct[1];
        scrGlobal.Instance.exclamationLimit = niveauxBonus.extraPonct[2];
        scrGlobal.Instance.interrogationLimit = niveauxBonus.extraPonct[3];
        scrGlobal.Instance.deuxpointsLimit = niveauxBonus.extraPonct[4];
        scrGlobal.Instance.pointvirguleLimit = niveauxBonus.extraPonct[5];

        //joueur
        Joueurs j = scrGlobal.Instance.GetPlayer();
    }
}