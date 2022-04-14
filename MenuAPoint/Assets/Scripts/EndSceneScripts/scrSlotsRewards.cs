using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scrSlotsRewards : MonoBehaviour, IDropHandler
{
    public static bool pointerIsOnSlot = false;
    public static string nameSlot;

    public void OnDrop(PointerEventData eventData)
    {
        nameSlot = name;

        switch (name)
        {
            case "SlotCasserole":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[0])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotCuillere":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[1])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotCouteau":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[2])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotFouet":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[3])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotLouche":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[4])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotPassoire":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[5])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotPoele":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[6])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotRouleau":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[7])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotRape":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[8])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "SlotSpatule":
                if (eventData.pointerDrag != null && GameObject.Find("Global").GetComponent<scrGlobal>().endSceneItemsCanMove[9])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            default:
                throw new System.Exception("Element not found");
        }
    }

    public void PointerOnSlot()
    {
        pointerIsOnSlot = true;
    }

    public void PointerOutSlot()
    {
        pointerIsOnSlot = false;
    }
}
