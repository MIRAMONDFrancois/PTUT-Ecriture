using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrEnd : MonoBehaviour
{
    public TextMeshProUGUI TextFin;

    private void Start()
    {
        checkIndices();
    }

    public void checkIndices()
    {
        switch (GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices)
        {
            case 0:
                TextFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière surprenante !";
                break;
            case 1:
                TextFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière remarquable !";
                break;
            case 2:
                TextFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière épatante !";
                break;
            case 3:
                TextFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière géniale !";
                break;
            case 4:
                TextFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière formidable !";
                break;
            case 5:
                TextFin.text = "Grâce à votre aide, Tokki a réussi à réaliser son rêve ultime ; elle est devenue une excellente cuisinière";
                break;
            default:
                throw new System.Exception("Erreur");
        }
    }
}
