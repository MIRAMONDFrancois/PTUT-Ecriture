using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrEnd : MonoBehaviour
{
    public TextMeshProUGUI textFin;
    public GameObject menuButton;
    public GameObject boxItems;
    public GameObject tokkiSprite;
    public AudioSource endSound;
    private bool endSoundPlayed;

    private void Start()
    {
        checkIndices();
        endSoundPlayed = false;
    }

    private void Update()
    {
        Debug.Log("Nombre drags : " + scrGlobal.Instance.nbrDrag);

        if(scrGlobal.Instance.nbrDrag > 9)
        {
            if(!endSoundPlayed)
            {
                endSound.Play();
                endSoundPlayed = true;
            }
            textFin.gameObject.SetActive(true);
            menuButton.SetActive(true);
            boxItems.SetActive(false);
            tokkiSprite.SetActive(true);
        }
        
       
    }

    public void checkIndices()
    {
        switch (scrGlobal.Instance.nbIndices)
        {
            case 0:
                textFin.text = "Grâce à ton aide, Tokki est devenue une cuisinière surprenante !";
                break;
            case 1:
                textFin.text = "Grâce à ton aide, Tokki est devenue une cuisinière remarquable !";
                break;
            case 2:
                textFin.text = "Grâce à ton aide, Tokki est devenue une cuisinière épatante !";
                break;
            case 3:
                textFin.text = "Grâce à ton aide, Tokki est devenue une cuisinière géniale !";
                break;
            case 4:
                textFin.text = "Grâce à ton aide, Tokki est devenue une cuisinière formidable !";
                break;
            case 5:
                textFin.text = "Grâce à ton aide, Tokki a réalisé son rêve ultime ; elle est devenue une excellente cuisinière !";
                break;
            default:
                throw new System.Exception("Erreur Nombre Indices --- WTF");
        }
    }
}
