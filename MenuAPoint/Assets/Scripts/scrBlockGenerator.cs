using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class scrBlockGenerator : MonoBehaviour
{
    public GameObject block;
    public GameObject canvas;
    //public GameObject eventSys;

    public string ponct;
    public GameObject textManager;

    private GameObject obj;

    private bool isActive = true;
    private int numberLeft = 0;


    public void Start() {
        //gameObject.transform.GetChild(1).gameObject.SetActive(true);
        //gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "∞";
        
        switch (ponct) {
            case ".":
                if (textManager.GetComponent<scrTextManager>().pointLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = textManager.GetComponent<scrTextManager>().pointLimit - GameObject.FindGameObjectsWithTag("Point").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ",":
                if (textManager.GetComponent<scrTextManager>().virguleLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = textManager.GetComponent<scrTextManager>().virguleLimit - GameObject.FindGameObjectsWithTag("Virgule").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            default:
                break;
        }
    }


    public void Update() {
        // CE CODE EST IMMONDE ET EST EXECUTED A CHAQUE FRAME, il faudrait le placer aux moments de créations ou suppression de ponctuation, mais c'est trop relou
        switch (ponct) {
            case ".":
                if (textManager.GetComponent<scrTextManager>().pointLimit != -1) { // if limit isn't infinite
                    if (GameObject.FindGameObjectsWithTag("Point").Length < textManager.GetComponent<scrTextManager>().pointLimit) {
                        // still visible
                        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        isActive = true;
                        numberLeft = textManager.GetComponent<scrTextManager>().pointLimit - GameObject.FindGameObjectsWithTag("Point").Length;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                    } else {
                        // locked
                        gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                        isActive = false;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                    }
                }
                break;
            case ",":
                if (textManager.GetComponent<scrTextManager>().virguleLimit != -1) { // if limit isn't infinite
                    if (GameObject.FindGameObjectsWithTag("Virgule").Length < textManager.GetComponent<scrTextManager>().virguleLimit) {
                        // still visible
                        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        isActive = true;
                        numberLeft = textManager.GetComponent<scrTextManager>().virguleLimit - GameObject.FindGameObjectsWithTag("Virgule").Length;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                    } else {
                        // locked
                        gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                        isActive = false;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                    }
                }
                break;
            default:
                break;
        }
        
        //Debug.Log(GameObject.FindGameObjectsWithTag("Point").Length);
    }


    public void CreatesBlock()
    {
        if (isActive) {
            obj = Instantiate(block);
            obj.transform.SetParent(canvas.transform);
            obj.GetComponent<scrDragAndDrop>().canBeMoved = true;
            obj.GetComponent<scrDragAndDrop>().canBeDeleted = true;

            obj.GetComponent<scrDragAndDrop>().StartDragUI();
        }
        
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
        if (obj != null) obj.GetComponent<scrDragAndDrop>().StopDragUI();
    }
}
