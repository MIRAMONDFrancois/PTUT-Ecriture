using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrIndice : MonoBehaviour
{
    private scrTextManager scriptText;
    public bool used = false;
    //private float normalScale = 1f;
    //private float upScale = 1.2f;
    private float speedScale = 0.04f;

    public bool limite = false;
    private int limiteScale = 5;//(upScale - normalScale) / speedScale -> 5 frames
    private int frames_mouse_over = 0;
    private int change = 1;
    private int _nbIndiceBuilder = 1;

    private int frames = 0;
    
    public GameObject Indice;
    public GameObject Indice1;
    public GameObject Indice2;
    public GameObject Indice3;
    public GameObject Indice4;
    public GameObject Indice5;

    void Start()
    {
        scriptText = GameObject.Find("GameManager").GetComponent<scrTextManager>();

        //events
        scriptText.OnTextLoad = Init;
    }

    void Init()
    {   
        level3unlocked();
        affichageIndices();

        if(scrGlobal.Instance.GetIndice())
        {
            used=true;
            scriptText.showIndice();
        }
    }

    public void decrementIndice()
    {
        if(scrGlobal.Instance.FromGameBuilder || scrGlobal.Instance.FromBonusLevel)
        {
            _nbIndiceBuilder = 0;
            affichageIndices();
            scriptText.showIndice();
            return;
        }


        if (scrGlobal.Instance.nbIndices > 0)
        {
            if(!used)
            {
                scrGlobal.Instance.nbIndices--;
                used = true;
                scrGlobal.Instance.SetIndice();
            }
            
            scriptText.showIndice();
            
        }

        affichageIndices();

        for(int a=0;a<scrGlobal.Instance.nbIndices;a++)
        {
            transform.GetChild(a).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            transform.GetChild(a).localScale = new Vector2(1f,1f);
        }
    }

    public void affichageIndices()
    {
        if(scrGlobal.Instance.FromGameBuilder || scrGlobal.Instance.FromBonusLevel)
        {
            if(_nbIndiceBuilder == 1)
            {
                Indice2.SetActive(false);
                Indice3.SetActive(false);
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                return;
            }

            Indice1.SetActive(false);
            return;
        }

        switch (scrGlobal.Instance.nbIndices)
        {
            case 0:
                Indice1.SetActive(false);
                Indice2.SetActive(false);
                Indice3.SetActive(false);
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                break;
            case 1:
                Indice2.SetActive(false);
                Indice3.SetActive(false);
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                break;
            case 2:
                Indice3.SetActive(false);
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                break;
            case 3:
                Indice4.SetActive(false);
                Indice5.SetActive(false);
                break;
            case 4:
                Indice5.SetActive(false);
                break;
            case 5:
                break;
            default:
                throw new System.Exception("Erreur indice");
        }
    }

    public void level3unlocked()
    {
        if(scrGlobal.Instance.FromGameBuilder || scrGlobal.Instance.FromBonusLevel)return;

        if (scrGlobal.Instance.levelNum <= 3)
            Indice.SetActive(false);
    }

    void FixedUpdate()
    {
        if(!used)
        {
            if (frames_mouse_over > 0)
            {
                change = 1;
                frames_mouse_over--;
            }
            else if(frames_mouse_over < 0)
            {
                change = -1;
                frames_mouse_over++;
            }else
            {
                change = 0;
            }

            for(int a=0;a<5;a++)
            {
                Vector3 newTrans = gameObject.transform.GetChild(a).localScale;
                newTrans.x += speedScale * change;
                newTrans.y += speedScale * change;
                gameObject.transform.GetChild(a).localScale = newTrans;
            } 

            
        }
        

        if(limite && !used)
        {
            frames++;
            indice_animation();
        }
        
    }

    public void SetTargeted(bool value) {

        if(value && frames_mouse_over == 0)// enter && début ou fin cycle petit
        {
            frames_mouse_over = limiteScale;
        }else if(!value && frames_mouse_over == 0)// exit && fin cycle grand
        {
            frames_mouse_over = -limiteScale;
        }
        else if(!value)//exit avant fin cycle
        {
            frames_mouse_over = -limiteScale + frames_mouse_over;
        }else//enter avant fin cycle
        {
            frames_mouse_over = limiteScale + frames_mouse_over;
        }
    }

    public void indice_animation()//pulse
    {
        switch(frames)
        {
            case 1:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x+speedScale,Indice1.transform.localScale.y+speedScale);
            break;
            case 2:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x+speedScale,Indice1.transform.localScale.y+speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x+speedScale,Indice2.transform.localScale.y+speedScale);
            break;
            case 3:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x+speedScale,Indice1.transform.localScale.y+speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x+speedScale,Indice2.transform.localScale.y+speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x+speedScale,Indice3.transform.localScale.y+speedScale);
            break;
            case 4:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x+speedScale,Indice1.transform.localScale.y+speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x+speedScale,Indice2.transform.localScale.y+speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x+speedScale,Indice3.transform.localScale.y+speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x+speedScale,Indice4.transform.localScale.y+speedScale);
            break;
            case 5:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x+speedScale,Indice1.transform.localScale.y+speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x+speedScale,Indice2.transform.localScale.y+speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x+speedScale,Indice3.transform.localScale.y+speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x+speedScale,Indice4.transform.localScale.y+speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x+speedScale,Indice5.transform.localScale.y+speedScale);
            break;
            case 6:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x-speedScale,Indice1.transform.localScale.y-speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x+speedScale,Indice2.transform.localScale.y+speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x+speedScale,Indice3.transform.localScale.y+speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x+speedScale,Indice4.transform.localScale.y+speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x+speedScale,Indice5.transform.localScale.y+speedScale);
            break;
            case 7:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x-speedScale,Indice1.transform.localScale.y-speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x-speedScale,Indice2.transform.localScale.y-speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x+speedScale,Indice3.transform.localScale.y+speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x+speedScale,Indice4.transform.localScale.y+speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x+speedScale,Indice5.transform.localScale.y+speedScale);
            break;
            case 8:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x-speedScale,Indice1.transform.localScale.y-speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x-speedScale,Indice2.transform.localScale.y-speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x-speedScale,Indice3.transform.localScale.y-speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x+speedScale,Indice4.transform.localScale.y+speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x+speedScale,Indice5.transform.localScale.y+speedScale);
            break;
            case 9:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x-speedScale,Indice1.transform.localScale.y-speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x-speedScale,Indice2.transform.localScale.y-speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x-speedScale,Indice3.transform.localScale.y-speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x-speedScale,Indice4.transform.localScale.y-speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x+speedScale,Indice5.transform.localScale.y+speedScale);
            break;
            case 10:
                Indice1.transform.localScale = new Vector2(Indice1.transform.localScale.x-speedScale,Indice1.transform.localScale.y-speedScale);
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x-speedScale,Indice2.transform.localScale.y-speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x-speedScale,Indice3.transform.localScale.y-speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x-speedScale,Indice4.transform.localScale.y-speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x-speedScale,Indice5.transform.localScale.y-speedScale);
            break;
            case 11:
                Indice2.transform.localScale = new Vector2(Indice2.transform.localScale.x-speedScale,Indice2.transform.localScale.y-speedScale);
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x-speedScale,Indice3.transform.localScale.y-speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x-speedScale,Indice4.transform.localScale.y-speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x-speedScale,Indice5.transform.localScale.y-speedScale);
            break;
            case 12:
                Indice3.transform.localScale = new Vector2(Indice3.transform.localScale.x-speedScale,Indice3.transform.localScale.y-speedScale);
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x-speedScale,Indice4.transform.localScale.y-speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x-speedScale,Indice5.transform.localScale.y-speedScale);
            break;
            case 13:
                Indice4.transform.localScale = new Vector2(Indice4.transform.localScale.x-speedScale,Indice4.transform.localScale.y-speedScale);
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x-speedScale,Indice5.transform.localScale.y-speedScale);
            break;
            case 14:
                Indice5.transform.localScale = new Vector2(Indice5.transform.localScale.x-speedScale,Indice5.transform.localScale.y-speedScale);
            break;
            case 15:
                frames = -300;//60 -> 1sec
            break;

        }
    }
}
