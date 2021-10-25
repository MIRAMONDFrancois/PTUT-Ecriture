﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scrDragAndDrop : MonoBehaviour
{
    public string ponct;

    bool dragging;
    bool willSnap;
    Vector3 ogPos;
    Vector3 snapPos;
    Collider2D col;

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
        Debug.Log("pointer picked up");
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

    public void StopDragUI()
    {
        Debug.Log("pointer dropped");
        if (dragging) dragging = false;
        if (!willSnap)
        {
            Destroy(gameObject);
        } else
        {
            transform.position = snapPos;
            col.GetComponent<scrSlot>().SendPonct(ponct);
            col.GetComponent<scrSlot>().isUsed = true;
        }

        textManager.GetComponent<scrTextManager>().HideSlots();
    }

    private void Update()
    {
        if (dragging)
        {
            Vector3 vect = Input.mousePosition;
            vect.x = vect.x - 5;
            vect.y = vect.y + 5;
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
        if (collision.CompareTag("Slot") && collision.GetComponent<scrSlot>().isUsed == false)
        {
            willSnap = true;
            Vector3 vect = collision.transform.position;
            vect.y = vect.y - 10;
            snapPos = vect;

            col = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Slot"))
        {
            willSnap = false;
            snapPos = ogPos;
        }
    }

}
