using System;
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
    private GameObject globalManager;

    private GameObject obj;

    private bool isActive = true;
    private int numberLeft = 0;


    public void Start() {
        globalManager = GameObject.Find("Global");

        switch (ponct) {
            case ".":
                if (globalManager.GetComponent<scrGlobal>().pointLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = globalManager.GetComponent<scrGlobal>().pointLimit - GameObject.FindGameObjectsWithTag("Point").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ",":
                if (globalManager.GetComponent<scrGlobal>().virguleLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = globalManager.GetComponent<scrGlobal>().virguleLimit - GameObject.FindGameObjectsWithTag("Virgule").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case "!":
                if (globalManager.GetComponent<scrGlobal>().exclamationLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = globalManager.GetComponent<scrGlobal>().exclamationLimit - GameObject.FindGameObjectsWithTag("Exclamation").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case "?":
                if (globalManager.GetComponent<scrGlobal>().interrogationLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = globalManager.GetComponent<scrGlobal>().interrogationLimit - GameObject.FindGameObjectsWithTag("Interrogation").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ":":
                if (globalManager.GetComponent<scrGlobal>().deuxpointsLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = globalManager.GetComponent<scrGlobal>().deuxpointsLimit - GameObject.FindGameObjectsWithTag("Deux Points").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ";":
                if (globalManager.GetComponent<scrGlobal>().pointvirguleLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = globalManager.GetComponent<scrGlobal>().pointvirguleLimit - GameObject.FindGameObjectsWithTag("Point Virgule").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            default:
                break;
        }
        affichage(0);
    }

    public void affichage(int plus_ou_moins)
    {
        //GameObject gen_nombre = GameObject.Find(generator);
        int machin = Int32.Parse(transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
        machin+=plus_ou_moins;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = machin +"";

        //bloquer le generateur si 0, si infini -> décrémente à l'infini
        if(machin==0)
        {
            gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            isActive=false;
        }else
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            isActive = true;
        }
    }

    public void CreatesBlock()
    {
        if (isActive) {
            obj = Instantiate(block);
            obj.transform.SetParent(canvas.transform);
            obj.GetComponent<scrDragAndDrop>().canBeMoved = true;
            obj.GetComponent<scrDragAndDrop>().canBeDeleted = true;

            obj.GetComponent<scrDragAndDrop>().StartDragUI();
 
            affichage(-1);
        }
        
    }

    public void animBlock(Vector3 pos_slot)
    {
        obj = Instantiate(block);
        obj.transform.SetParent(canvas.transform);
        obj.GetComponent<scrDragAndDrop>().canBeMoved = true;
        obj.GetComponent<scrDragAndDrop>().canBeDeleted = true;

        obj.transform.position = pos_slot;

        affichage(-1);
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
