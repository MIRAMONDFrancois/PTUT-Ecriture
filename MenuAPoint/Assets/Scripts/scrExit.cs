﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrExit : MonoBehaviour
{
    // Start is called before the first frame update
    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
