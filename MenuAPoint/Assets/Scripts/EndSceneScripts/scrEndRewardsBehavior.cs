using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scrEndRewardsBehavior : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;
    Vector2 initialPosition;
    public static string nameDrag;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.anchoredPosition;
        nameDrag = name;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        Debug.Log(scrSlotsRewards.nameSlot);
        switch (name)
        {
            case "Casserole":
                if (scrSlotsRewards.nameSlot != "SlotCasserole")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Cuillere":
                if (scrSlotsRewards.nameSlot != "SlotCuillere")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Couteau":
                if (scrSlotsRewards.nameSlot != "SlotCouteau")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Fouet":
                if (scrSlotsRewards.nameSlot != "SlotFouet")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Louche":
                if (scrSlotsRewards.nameSlot != "SlotLouche")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Passoire":
                if (scrSlotsRewards.nameSlot != "SlotPassoire")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Poele":
                if (scrSlotsRewards.nameSlot != "SlotPoele")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Rouleau":
                if (scrSlotsRewards.nameSlot != "SlotRouleau")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Rape":
                if (scrSlotsRewards.nameSlot != "SlotRape")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            case "Spatule":
                if (scrSlotsRewards.nameSlot != "SlotSpatule")
                    rectTransform.anchoredPosition = initialPosition;
                if (!scrSlotsRewards.pointerIsOnSlot)
                    rectTransform.anchoredPosition = initialPosition;
                break;
            default:
                throw new System.Exception("Element not found");
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
    }
}
