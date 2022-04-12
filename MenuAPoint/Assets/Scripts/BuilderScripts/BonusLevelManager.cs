using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusLevelManager : MonoBehaviour
{
    public GameObject NivBonusPrefab;
    public RectTransform Content;
    public Scrollbar ScrollBarValue;

    void Start()
    {
        for(int a=0;a<52;a++)
        {
            GameObject niveau = Instantiate(NivBonusPrefab);
            niveau.transform.SetParent(Content.transform);
            niveau.transform.localScale = Vector3.one;
        }

        ScrollBarValue.value = 1f;

    }
}
