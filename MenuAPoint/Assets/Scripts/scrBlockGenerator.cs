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

        //eventSys.GetComponent<UnityEventQueueSystem>();
        //var eventData = new PointerEventData(EventSystem.current);
        //eventData.pointerPress = obj;



        obj.GetComponent<scrDragAndDrop>().StartDragUI();




    }

    public void DouillePointerUp()
    {
        obj.GetComponent<scrDragAndDrop>().StopDragUI();
    }
}
