using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrEnd : MonoBehaviour
{
    public TextMeshProUGUI textFin;
    public GameObject menuButton;
    public TextMeshProUGUI titreUI;
    public TextMeshProUGUI instructionsUI;

    private void Start()
    {
        checkIndices();
    }

    private void Update()
    {
       if(GameObject.Find("Global").GetComponent<scrGlobal>().nbrDrag > 9)
        {
            textFin.gameObject.SetActive(true);
            menuButton.SetActive(true);
            titreUI.gameObject.SetActive(true);
            instructionsUI.gameObject.SetActive(false);
        }
    }

    public void checkIndices()
    {
        switch (GameObject.Find("Global").GetComponent<scrGlobal>().nbIndices)
        {
            case 0:
                textFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière surprenante !";
                break;
            case 1:
                textFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière remarquable !";
                break;
            case 2:
                textFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière épatante !";
                break;
            case 3:
                textFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière géniale !";
                break;
            case 4:
                textFin.text = "Grâce à votre aide, Tokki est devenue une cuisinière formidable !";
                break;
            case 5:
                textFin.text = "Grâce à votre aide, Tokki a réussi à réaliser son rêve ultime ; elle est devenue une excellente cuisinière";
                break;
            default:
                throw new System.Exception("Erreur");
        }
    }
}
