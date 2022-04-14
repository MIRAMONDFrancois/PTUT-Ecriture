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
        switch(name)
        {
            case "Casserole":
                rectTransform.sizeDelta = new Vector2(170, 300);
                break;
            case "Cuillere":
                rectTransform.sizeDelta = new Vector2(80, 260);
                rectTransform.rotation = Quaternion.Euler(0f, 0f, -12f);
                break;
            case "Couteau":
                rectTransform.sizeDelta = new Vector2(150, 210);
                rectTransform.rotation = Quaternion.Euler(0f, 0f, 216f);
                break;
            case "Fouet":
                rectTransform.sizeDelta = new Vector2(190, 375);
                rectTransform.rotation = Quaternion.Euler(0f, 0f, 95f);
                break;
            case "Louche":
                rectTransform.sizeDelta = new Vector2(200, 350);
                break;
            case "Passoire":
                rectTransform.sizeDelta = new Vector2(260, 260);
                break;
            case "Poele":
                rectTransform.sizeDelta = new Vector2(250, 470);
                break;
            case "Rouleau":
                rectTransform.sizeDelta = new Vector2(260, 260);
                break;
            case "Rape":
                rectTransform.sizeDelta = new Vector2(290, 366);
                break;
            case "Spatule":
                rectTransform.sizeDelta = new Vector2(175, 300);
                rectTransform.rotation = Quaternion.Euler(0f, 0f, 150f);
                break;
            default:
                throw new System.Exception("Element not found");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        switch (name)
        {
            case "Casserole":
                if (scrGlobal.Instance.endSceneItemsCanMove[0])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Cuillere":
                if (scrGlobal.Instance.endSceneItemsCanMove[1])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Couteau":

                if (scrGlobal.Instance.endSceneItemsCanMove[2])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Fouet":

                if (scrGlobal.Instance.endSceneItemsCanMove[3])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Louche":
                if (scrGlobal.Instance.endSceneItemsCanMove[4])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Passoire":
                if (scrGlobal.Instance.endSceneItemsCanMove[5])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Poele":
                if (scrGlobal.Instance.endSceneItemsCanMove[6])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Rouleau":
                if (scrGlobal.Instance.endSceneItemsCanMove[7])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Rape":
                if (scrGlobal.Instance.endSceneItemsCanMove[8])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            case "Spatule":
                if (scrGlobal.Instance.endSceneItemsCanMove[9])
                {
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                break;
            default:
                throw new System.Exception("Element not found");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        //canvasGroup.blocksRaycasts = true;
        Debug.Log(scrSlotsRewards.nameSlot);
        switch (name)
        {
            case "Casserole":
                if ((scrSlotsRewards.nameSlot != "SlotCasserole" && scrGlobal.Instance.endSceneItemsCanMove[0] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(120, 200);
                }

                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[0] = false;
                }

                break;
            case "Cuillere":
                if ((scrSlotsRewards.nameSlot != "SlotCuillere" && scrGlobal.Instance.endSceneItemsCanMove[1] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(80, 200);
                    rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[1] = false;
                }
                break;
            case "Couteau":
                if ((scrSlotsRewards.nameSlot != "SlotCouteau" && scrGlobal.Instance.endSceneItemsCanMove[2] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(150, 200);
                    rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[2] = false;
                }
                break;
            case "Fouet":
                if ((scrSlotsRewards.nameSlot != "SlotFouet" && scrGlobal.Instance.endSceneItemsCanMove[3] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(100, 200);
                    rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[3] = false;
                }

                break;
            case "Louche":
                if ((scrSlotsRewards.nameSlot != "SlotLouche" && scrGlobal.Instance.endSceneItemsCanMove[4] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(100, 200);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[4] = false;
                }

                break;
            case "Passoire":
                if ((scrSlotsRewards.nameSlot != "SlotPassoire" && scrGlobal.Instance.endSceneItemsCanMove[5] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(200, 200);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[5] = false;
                }
                break;
            case "Poele":
                if ((scrSlotsRewards.nameSlot != "SlotPoele" && scrGlobal.Instance.endSceneItemsCanMove[6] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(130, 200);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[6] = false;
                }
                break;
            case "Rouleau":
                if ((scrSlotsRewards.nameSlot != "SlotRouleau" && scrGlobal.Instance.endSceneItemsCanMove[7] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(150, 200);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[7] = false;
                }
                break;
            case "Rape":
                if ((scrSlotsRewards.nameSlot != "SlotRape" && scrGlobal.Instance.endSceneItemsCanMove[8] == true) || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(150, 200);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[8] = false;
                }
                break;
            case "Spatule":
                if ((scrSlotsRewards.nameSlot != "SlotSpatule") || !scrSlotsRewards.pointerIsOnSlot)
                {
                    rectTransform.anchoredPosition = initialPosition;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.sizeDelta = new Vector2(110, 200);
                    rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                if (rectTransform.anchoredPosition != initialPosition)
                {
                    scrGlobal.Instance.nbrDrag++;
                    scrGlobal.Instance.endSceneItemsCanMove[9] = false;
                    canvasGroup.blocksRaycasts = true;
                }
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
