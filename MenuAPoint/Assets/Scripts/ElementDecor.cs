using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementDecor : MonoBehaviour
{
    
    void Start()
    {
        //1-2-4-5-7-9-10-12-13-15
        List<bool> elements = GameObject.Find("Global").GetComponent<scrGlobal>().levelunlocked;
        bool verif;
        int outil = -1;

        for(int a=0;a<elements.Count;a++)
        {
            verif = true;
            switch(a)
            {
                case 1:
                    outil = 2;
                break;
                case 2:
                    outil = 3;
                break;
                case 4:
                    outil = 4;
                break;
                case 5:
                    outil = 5;
                break;
                case 7:
                    outil = 6;
                break;
                case 9:
                    outil = 7;
                break;
                case 10:
                    outil = 8;
                break;
                case 12:
                    outil = 9;
                break;
                case 13:
                    outil = 10;
                break;
                case 15:
                    outil = 11;
                break;
                default:
                verif = false;
                break;
            }

            if(verif && elements[a])
            {
                //transform.GetChild(outil).gameObject.SetActive(elements[a]);
                transform.GetChild(outil).GetComponent<Image>().color = new Color(1f,1f,1f);
            }        
        }
        
    }
}
