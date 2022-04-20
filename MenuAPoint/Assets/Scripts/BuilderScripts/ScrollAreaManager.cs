using System;
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

    private DonneesBonus _data;
    private List<GameObject> niveaux = new List<GameObject>();
    public Action OnNiveauSelected;

    void Start()
    {
        scrGlobal.Instance.RefreshNiveauxBonus();
        _data = scrGlobal.Instance.GetDataBonus();
        SuppNiveauxBonus();
        CreerNiveauxBonus();
        ScrollBarValue.value = 1f;//slide on top
    }

    private void CreerNiveauxBonus()
    {
        niveaux = new List<GameObject>();

        foreach(string keys in _data.niveaux.Keys)
        {
            GameObject niveau = Instantiate(NivBonusPrefab);
            
            niveau.GetComponentInChildren<TextMeshProUGUI>().text = keys;
            niveau.transform.SetParent(Content.transform);
            niveau.transform.localScale = Vector3.one;

            niveaux.Add(niveau);
        }

    }

    private void SuppNiveauxBonus()
    {
        foreach(GameObject go in niveaux)
        {
            go.GetComponent<BonusReussite>().Unsub(this);
            Destroy(go);
        }
    }

    public void LoadNouveau()
    {
        scrGlobal.Instance.GameBuilderText = null;
        scrGlobal.Instance.NameBuilderText = "NomDuTexte";
        OnNiveauSelected?.Invoke();
    }
}
