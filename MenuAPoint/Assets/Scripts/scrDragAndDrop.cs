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
    public Vector3 ogPos;
    [HideInInspector]
    public Vector3 snapPos;
    [HideInInspector]
    public Collider2D col;
    [HideInInspector]
    public float offset = 8;

    public GameObject textManager;

    public void Awake()
    {
        textManager = GameObject.Find("GameManager");

        ogPos = transform.position;
        snapPos = ogPos;
        willSnap = false;

    }

    public void StartDragUI()
    {
        if (canBeMoved)
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
            textManager.GetComponent<scrTextManager>().ShowSlots();
        }
    }

    public void StopDragUI()
    {
        if (canBeMoved)
        {
            //Debug.Log("pointer dropped");
            if (dragging) dragging = false;
            if (!willSnap)
            {
                if (canBeDeleted) Destroy(gameObject);
            }
            else
            {
                //transform.position = snapPos;
                col.GetComponent<scrSlot>().SendPonct(ponct);
                col.GetComponent<scrSlot>().isUsed = true;
            }

            transform.position = snapPos;
            textManager.GetComponent<scrTextManager>().HideSlots();
        }
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
        } else
        {
            //gameObject.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            //gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
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
            }
        }
    }

}
