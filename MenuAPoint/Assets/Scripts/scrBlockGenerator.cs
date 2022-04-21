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

    private GameObject obj;

    private bool isActive = true;
    private int numberLeft = 0;

    public GameObject container;


    public void Start() {

        switch (ponct) {
            case ".":
                if (scrGlobal.Instance.pointLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = scrGlobal.Instance.pointLimit - GameObject.FindGameObjectsWithTag("Point").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ",":
                if (scrGlobal.Instance.virguleLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = scrGlobal.Instance.virguleLimit - GameObject.FindGameObjectsWithTag("Virgule").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case "!":
                if (scrGlobal.Instance.exclamationLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = scrGlobal.Instance.exclamationLimit - GameObject.FindGameObjectsWithTag("Exclamation").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case "?":
                if (scrGlobal.Instance.interrogationLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = scrGlobal.Instance.interrogationLimit - GameObject.FindGameObjectsWithTag("Interrogation").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ":":
                if (scrGlobal.Instance.deuxpointsLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = scrGlobal.Instance.deuxpointsLimit - GameObject.FindGameObjectsWithTag("Deux Points").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case ";":
                if (scrGlobal.Instance.pointvirguleLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = scrGlobal.Instance.pointvirguleLimit - GameObject.FindGameObjectsWithTag("Point Virgule").Length;
                    gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberLeft + "";
                }
                break;
            case "…":
                if (scrGlobal.Instance.suspensionLimit != -1) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    numberLeft = scrGlobal.Instance.suspensionLimit - GameObject.FindGameObjectsWithTag("Suspension").Length;
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

        //bloquer le generateur si 0. Si infini -> décrémente à l'infini
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
            obj.transform.SetParent(container.transform);
            obj.GetComponent<scrDragAndDrop>().canBeMoved = true;
            obj.GetComponent<scrDragAndDrop>().canBeDeleted = true;

            obj.GetComponent<scrDragAndDrop>().StartDragUI();
            
            affichage(-1);
        }
        
    }

    public void demarrageBlock(GameObject slot)
    {
        obj = Instantiate(block);
        obj.transform.SetParent(container.transform);
        obj.GetComponent<scrDragAndDrop>().canBeMoved = true;
        obj.GetComponent<scrDragAndDrop>().canBeDeleted = true;
        obj.GetComponent<scrDragAndDrop>().willSnap = true;
        obj.GetComponent<scrDragAndDrop>().col = slot.GetComponent<Collider2D>();
        
        obj.GetComponent<scrDragAndDrop>().StartDragUI();
        obj.GetComponent<scrDragAndDrop>().snapPos = slot.GetComponent<Collider2D>().transform.position;
        obj.GetComponent<scrDragAndDrop>().estsortie = 1;
        obj.GetComponent<scrDragAndDrop>().StopDragUI();

        

        affichage(-1);
    }

    public void DouillePointerUp()
    {
        if (obj != null) obj.GetComponent<scrDragAndDrop>().StopDragUI();
    }
}
