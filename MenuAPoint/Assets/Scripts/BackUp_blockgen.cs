using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class backup_scrBlockGenerator : MonoBehaviour
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
            case "!":
                if (textManager.GetComponent<scrTextManager>().exclamationLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = textManager.GetComponent<scrTextManager>().exclamationLimit - GameObject.FindGameObjectsWithTag("Exclamation").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case "?":
                if (textManager.GetComponent<scrTextManager>().interrogationLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = textManager.GetComponent<scrTextManager>().interrogationLimit - GameObject.FindGameObjectsWithTag("Interrogation").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ":":
                if (textManager.GetComponent<scrTextManager>().deuxpointsLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = textManager.GetComponent<scrTextManager>().deuxpointsLimit - GameObject.FindGameObjectsWithTag("Deux Points").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ";":
                if (textManager.GetComponent<scrTextManager>().pointvirguleLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = textManager.GetComponent<scrTextManager>().pointvirguleLimit - GameObject.FindGameObjectsWithTag("Point Virgule").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            default:
                break;
        }
    }


    /*public void Update() {
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
            case "!":
                if (textManager.GetComponent<scrTextManager>().exclamationLimit != -1) { // if limit isn't infinite
                    if (GameObject.FindGameObjectsWithTag("Exclamation").Length < textManager.GetComponent<scrTextManager>().exclamationLimit) {
                        // still visible
                        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        isActive = true;
                        numberLeft = textManager.GetComponent<scrTextManager>().exclamationLimit - GameObject.FindGameObjectsWithTag("Exclamation").Length;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                    } else {
                        // locked
                        gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                        isActive = false;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                    }
                }
                break;
            case "?":
                if (textManager.GetComponent<scrTextManager>().interrogationLimit != -1) { // if limit isn't infinite
                    if (GameObject.FindGameObjectsWithTag("Interrogation").Length < textManager.GetComponent<scrTextManager>().interrogationLimit) {
                        // still visible
                        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        isActive = true;
                        numberLeft = textManager.GetComponent<scrTextManager>().interrogationLimit - GameObject.FindGameObjectsWithTag("Interrogation").Length;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                    } else {
                        // locked
                        gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                        isActive = false;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                    }
                }
                break;
            case ":":
                if (textManager.GetComponent<scrTextManager>().deuxpointsLimit != -1) { // if limit isn't infinite
                    if (GameObject.FindGameObjectsWithTag("Deux Points").Length < textManager.GetComponent<scrTextManager>().deuxpointsLimit) {
                        // still visible
                        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        isActive = true;
                        numberLeft = textManager.GetComponent<scrTextManager>().deuxpointsLimit - GameObject.FindGameObjectsWithTag("Deux Points").Length;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                    } else {
                        // locked
                        gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                        isActive = false;
                        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                    }
                }
                break;
            case ";":
                if (textManager.GetComponent<scrTextManager>().pointvirguleLimit != -1) { // if limit isn't infinite
                    if (GameObject.FindGameObjectsWithTag("Point Virgule").Length < textManager.GetComponent<scrTextManager>().pointvirguleLimit) {
                        // still visible
                        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        isActive = true;
                        numberLeft = textManager.GetComponent<scrTextManager>().pointvirguleLimit - GameObject.FindGameObjectsWithTag("Point Virgule").Length;
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
    }*/


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
