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
    public float offset = 8;

    public GameObject textManager;

    public void Awake()
    {
        textManager = GameObject.Find("GameManager");

        ogPos = transform.position;
        snapPos = ogPos;
        willSnap = false;
        
        Vector2 tailleponct = this.GetComponent<RectTransform>().sizeDelta;

        float potx = tailleponct[0]*Screen.width/1920;
        float poty = tailleponct[1]*Screen.height/1080;

        this.GetComponent<RectTransform>().sizeDelta=new Vector2(potx,poty);
        this.GetComponent<BoxCollider2D>().size=new Vector2(potx,poty);
    }


    private void Update()
    {
        if (dragging && canBeMoved)
        {
            Vector3 vect = Input.mousePosition;
            vect.x = vect.x - offset;
            vect.y = vect.y + offset;
            transform.position = vect;
            //gameObject.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            //gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);
        }
        else
        {
            //gameObject.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            //gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        }
    }

    public void StartDragUI()
    {
        if (canBeMoved && textManager.GetComponent<scrTextManager>().canTouchPonct)
        {
            //Debug.Log("pointer picked up");
            if (!dragging) dragging = true;
            // might help for "depth" collision
            transform.SetAsLastSibling();

            if (col != null)
            {
                col.GetComponent<scrSlot>().SendPonct("");
                col.GetComponent<scrSlot>().isUsed = false;
            }

            Vector2 taillePot = this.GetComponent<RectTransform>().sizeDelta;
            textManager.GetComponent<scrTextManager>().ShowSlots(taillePot,this.tag);
        }
    }

    public void StopDragUI()
    {
        if (canBeMoved)
        {
            //Debug.Log("pointer dropped");
            if (dragging) dragging = false;
            if (willSnap)
            {
                // WILL SNAP TO A SLOT NEARBY
                //transform.position = snapPos;
                col.GetComponent<scrSlot>().SendPonct(ponct);
                col.GetComponent<scrSlot>().isUsed = true;
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
                    col.GetComponent<scrSlot>().SendPonct(ponct);
                    col.GetComponent<scrSlot>().isUsed = true;
                }
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
                vect.y = vect.y - 25;
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

}
