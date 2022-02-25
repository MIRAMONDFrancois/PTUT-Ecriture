﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scrDragAndDrop : MonoBehaviour
{
    public string ponct;
    public bool canBeMoved = true;
    public bool canBeDeleted = true;

    [HideInInspector]
    public bool dragging;
    [HideInInspector]
    public bool willSnap;
    [HideInInspector]
    public Vector3 ogPos; // more of a "last viable position" than "original"
    [HideInInspector]
    public Vector3 snapPos;
    [HideInInspector]
    public Collider2D col;
    [HideInInspector]
    public Collider2D prevcol;
    [HideInInspector]
    public float offset = 4;
    public float offset_x;
    public float offset_y;

    public GameObject textManager;

    public void Awake()//on commence à prendre un ingrédient
    {
        textManager = GameObject.Find("GameManager");

        ogPos = transform.position;
        snapPos = ogPos;
        willSnap = false;
        
        //resize la taille du block
        Vector2 tailleponct = this.GetComponent<RectTransform>().sizeDelta;

        float potx = tailleponct[0]*Screen.width/1920;
        float poty = tailleponct[1]*Screen.height/1080;

        this.GetComponent<RectTransform>().sizeDelta=new Vector2(potx,poty);
        this.GetComponent<BoxCollider2D>().size=new Vector2(potx,poty);

        offset_x = offset*Screen.width/1920;
        offset_y = offset*Screen.height/1080;
    }

    private void Update()//pour suivre le déplacement de la souris
    {
        if (dragging && canBeMoved)
        {
            Vector3 vect = Input.mousePosition;
            vect.x = vect.x - offset_x;
            vect.y = vect.y + offset_y;
            transform.position = vect;
        }
        
    }

    public void StartDragUI()//génération du block
    {
        if (canBeMoved && textManager.GetComponent<scrTextManager>().canTouchPonct)
        {
            if (!dragging) dragging = true;
            // might help for "depth" collision
            transform.SetAsLastSibling();

            if (col != null)
            {
                //col.GetComponent<scrSlot>().SendPonct("");
                col.GetComponent<scrSlot>().isUsed = false;

                col.GetComponent<scrSlot>().ponctuation = "";
                
            }

            Vector2 taillePot = this.GetComponent<RectTransform>().sizeDelta;
            textManager.GetComponent<scrTextManager>().ShowSlots(taillePot,this.tag);

        }
        
    }

    public void StopDragUI()
    {
        if (canBeMoved)
        {
            if (dragging) dragging = false;

            if (willSnap)
            {
                // WILL SNAP TO A SLOT NEARBY
                //transform.position = snapPos;
                //col.GetComponent<scrSlot>().SendPonct(ponct);
                col.GetComponent<scrSlot>().isUsed = true;

                col.GetComponent<scrSlot>().ponctuation = ponct;
                prevcol = col;
                

            } else
            {
                // WILL NOT SNAP TO A SLOT
                if (canBeDeleted)
                {
                    // DESTROY
                    
                    Destroy(gameObject);
                } else
                {
                    // SNAP BACK TO THE LAST POSSIBLE POSITION
                    col = prevcol;
                    //col.GetComponent<scrSlot>().SendPonct(ponct);
                    col.GetComponent<scrSlot>().isUsed = true;

                    col.GetComponent<scrSlot>().ponctuation = ponct;
                    
                }
                string nom_gen = generateur();
                GameObject.Find(nom_gen).GetComponent<scrBlockGenerator>().affichage(1);
            }

            transform.position = snapPos;
            
            Vector2 taillePot = this.GetComponent<RectTransform>().sizeDelta;
            textManager.GetComponent<scrTextManager>().HideSlots(taillePot,this.tag);
            ogPos = snapPos; // this becomes the "last viable position"


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeMoved)
        {
            if (collision.CompareTag("Slot") && collision.GetComponent<scrSlot>().isUsed == false)
            {
                willSnap = true;
                Vector3 vect = collision.transform.position;
                snapPos = vect;

                col = collision;
                //Debug.Log("enter coll");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (dragging && canBeMoved)
        {
            if (collision.CompareTag("Slot"))
            {
                willSnap = false;
                snapPos = ogPos;
                //Debug.Log("exit coll");

                // exits the first slot and save it in case of problems
                if (prevcol == null) { prevcol = col;  }
            }
        }
    }

    private string generateur()//return nom gen
    {
        switch(ponct)
        {
            case ".":
                return "Point Gen";
            break;
            case "!":
                return "Exclamation Gen";
            break;
            case "?":
                return "Interrogation Gen";
            break;
            case ",":
                return "Virgule Gen";
            break;
            case ":":
                return "Deux Points Gen";
            break;
            case ";":
                return "Point Virgule Gen";
            break;
            default:
            break;
        }
        return null;
    }
}
