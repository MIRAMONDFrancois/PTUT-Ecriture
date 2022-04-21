using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scrDragAndDrop : MonoBehaviour
{
    public string ponct;
    public bool canBeMoved = true;
    public bool canBeDeleted = true;
    public int estsortie = 0;

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
    public float offset = 0;
    public float offset_x = 0;
    public float offset_y = 0;

    public GameObject textManager;

    public void Awake()//on commence à prendre un ingrédient
    {
        textManager = GameObject.Find("GameManager");
        RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        ogPos = transform.position;
        snapPos = ogPos;
        willSnap = false;
        
        //resize la taille du block
        float scale_x = textManager.GetComponent<scrTextManager>().scaler_x;
        float scale_y = textManager.GetComponent<scrTextManager>().scaler_y;

        Vector2 tailleponct = this.GetComponent<RectTransform>().sizeDelta;//taille origine

        //float potx = tailleponct[0]*Screen.width/1920*scale_x;
        //float poty = tailleponct[1]*Screen.height/1080*scale_y;

        float potx = tailleponct[0]*canvas.localScale.x;
        float poty = tailleponct[1]*canvas.localScale.y;

        this.GetComponent<RectTransform>().sizeDelta=new Vector2(potx,poty);
        this.GetComponent<BoxCollider2D>().size=new Vector2(potx*1.2f,poty*1.2f);
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
                col.GetComponent<scrSlot>().isUsed = false;

                col.GetComponent<scrSlot>().ponctuation = "";

                textManager.GetComponent<scrTextManager>().demajuscule(tag,col.GetComponent<scrSlot>().INDEX);      
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
                col.GetComponent<scrSlot>().isUsed = true;

                col.GetComponent<scrSlot>().ponctuation = ponct;
                prevcol = col;
                
                textManager.GetComponent<scrTextManager>().majuscule(tag,col.GetComponent<scrSlot>().INDEX);
                estsortie = 1;
                GameObject.Find("PopSound").GetComponent<SFX_Script>().pop_block();
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

                    textManager.GetComponent<scrTextManager>().majuscule(tag, col.GetComponent<scrSlot>().INDEX);
                }
                string nom_gen = generateur();
                GameObject.Find(nom_gen).GetComponent<scrBlockGenerator>().affichage(1);
            }

            transform.position = snapPos;
            
            Vector2 taillePot = this.GetComponent<RectTransform>().sizeDelta;
            textManager.GetComponent<scrTextManager>().HideSlots();
            ogPos = snapPos; // this becomes the "last viable position"

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeMoved && dragging)
        {
            if (collision.CompareTag("Slot") && collision.GetComponent<scrSlot>().isUsed == false)
            {
                willSnap = true;
                Vector3 vect = collision.transform.position;
                snapPos = vect;

                col = collision;
                estsortie++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (dragging && canBeMoved)
        {
            
            if(collision.CompareTag("Slot") && collision.GetComponent<scrSlot>().isUsed == false)
            {
                estsortie--;
            }

            if (collision.CompareTag("Slot") && estsortie==0)
            {
                willSnap = false;
                snapPos = ogPos;

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
            case "!":
                return "Exclamation Gen";
            case "?":
                return "Interrogation Gen";
            case ",":
                return "Virgule Gen";
            case ":":
                return "Deux Points Gen";
            case ";":
                return "Point Virgule Gen";
            case "…":
                return "Points Suspension Gen";
            default:
            break;
        }
        return null;
    }
}
