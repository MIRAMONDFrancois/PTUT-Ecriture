using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scrBlockGenerator : MonoBehaviour
{
    public string ponct; // ?
    public GameObject block;
    public GameObject canvas;
    //public GameObject eventSys;
    public GameObject textManager;

    private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        ponct = ",";
    }

    public void CreatesBlock()
    {
        obj = Instantiate(block);
        obj.transform.SetParent(canvas.transform);
        obj.GetComponent<scrDragAndDrop>().canBeMoved = true;
        obj.GetComponent<scrDragAndDrop>().canBeDeleted = true;

        obj.GetComponent<scrDragAndDrop>().StartDragUI();
    }

    public GameObject CreatesBlockForManager() //brute as heck
    {
        obj = Instantiate(block);
        obj.transform.SetParent(canvas.transform);
        obj.GetComponent<scrDragAndDrop>().canBeMoved = true;
        obj.GetComponent<scrDragAndDrop>().canBeDeleted = true;

        obj.GetComponent<scrDragAndDrop>().StartDragUI();

        return obj;
    }

    public void DouillePointerUp()
    {
        obj.GetComponent<scrDragAndDrop>().StopDragUI();
    }
}
