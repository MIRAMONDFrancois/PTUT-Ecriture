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

        switch (eventData.pointerDrag.name)
        {
            case "Casserole":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[0])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Cuillere":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[1])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Couteau":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[2])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Fouet":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[3])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Louche":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[4])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Passoire":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[5])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Poele":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[6])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Rouleau":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[7])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Rape":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[8])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            case "Spatule":
                if (eventData.pointerDrag != null && scrGlobal.Instance.endSceneItemsCanMove[9])
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                break;
            default:
                throw new System.Exception("Cet élément n'est pas un item.");
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
