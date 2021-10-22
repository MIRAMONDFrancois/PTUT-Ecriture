using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDragAndDrop : MonoBehaviour
{
    bool dragging;
    bool willSnap;
    Vector3 ogPos;
    Vector3 snapPos;

    public void Start()
    {
        ogPos = transform.position;
        snapPos = ogPos;
        willSnap = false;

        // might help for "depth" collision
        //transform.SetAsLastSibling();
    }

    public void StartDragUI()
    {
        if (!dragging) dragging = true;
        // might help for "depth" collision
        transform.SetAsLastSibling();
    }

    public void StopDragUI()
    {
        if (dragging) dragging = false;
        if (!willSnap)
        {
            transform.position = ogPos;
            // or might delete, will see
        } else
        {
            transform.position = snapPos;
        }
    }

    private void Update()
    {
        if (dragging)
        {
            Vector3 vect = Input.mousePosition;
            vect.z = -10;
            transform.position = vect;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Slot"))
        {
            willSnap = true;
            Vector3 vect = collision.transform.position;
            vect.z = -10; // MIGHT BE USELESS TOO
            snapPos = vect;
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

    /*
    private Vector3 dragOffset;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        dragOffset = transform.position - GetMousePos();
        Debug.Log("oui");
    }

    private void OnMouseDrag()
    {
        transform.position = GetMousePos() + dragOffset;
    }

    Vector3 GetMousePos()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
    */
}
