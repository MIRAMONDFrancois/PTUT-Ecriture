using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollAreaManager : MonoBehaviour
{
    public GameObject NivBonusPrefab;
    public RectTransform Content;
    public Scrollbar ScrollBarValue;

    private Donnees _data;

    void Start()
    {
        _data = scrGlobal.Instance.GetData();

        foreach(NiveauxBonus niveauBonus in _data.niveauxBonus)
        {
            GameObject niveau = Instantiate(NivBonusPrefab);
            niveau.GetComponentInChildren<TextMeshProUGUI>().text = niveauBonus.nom;


            niveau.transform.SetParent(Content.transform);
            niveau.transform.localScale = Vector3.one;
        }

        ScrollBarValue.value = 1f;

    }
}
