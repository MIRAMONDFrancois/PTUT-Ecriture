using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarotteBehavour : MonoBehaviour
{
    private Rigidbody2D rb;
    //Taille
    public string attribut;
    public bool premierplan;
    public bool tokkiplan;
    public float ratioCarotte;
    //Orientation
    public float sensCarotte;
    //Rotation
    public float deltaRotation;
    //Deplacement
    public float speed;
    //Couleur
    public float couleur;
    //Ombre
    public float ombre_x;
    public float ombre_y;

    public List<Component> components;
    

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        taille();
        orientation();
        rotation();
        deplacement();
        ombre_init();
        ombre();
        
        if(premierplan)
        {
            this.transform.SetParent(GameObject.Find("Canvas").transform);
        }else if(tokkiplan)
        {
            this.transform.SetParent(GameObject.Find("Tokki").transform);
        }else
        {
            this.transform.SetParent(GameObject.Find("Derriere").transform);
        }
        
    }

    void Update()
    {
        if(transform.position.y < -(Screen.height/2) )Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation + deltaRotation);
        ombre();
    }

    private void taille()
    {
        int repartition = Random.Range(1,101);
        float petit =.2f;
        float grand = 4f;

        if(repartition <= 20)//20% Minuscule
        {
            attribut = "minuscule";
            petit =.2f;
            grand =.5f;
            
            if(repartition <= 5)//25%
            {
                premierplan = true;
                tokkiplan = true;
            }else if(repartition <= 15)//50%
            {
                premierplan = false;
                tokkiplan = true;
            }else
            {
                premierplan = false;
                tokkiplan = false;
            }
            
            
        }else if(repartition <= 45)//25% Petite
        {
            attribut = "petit";
            petit =.5f;
            grand =1f;
            
            if(repartition <= 30)//35%
            {
                premierplan = true;
                tokkiplan = true;
            }else if(repartition <= 35)//30%
            {
                premierplan = false;
                tokkiplan = true;
            }else
            {
                premierplan = false;
                tokkiplan = false;
            }
        }
        else if(repartition <= 85)//40% Normal
        {
            attribut = "normal";
            petit =1f;
            grand =2f;

            if(repartition <= 73)//70%
            {
                premierplan = true;
                tokkiplan = true;
            }else if(repartition <= 81)//20%
            {
                premierplan = false;
                tokkiplan = true;
            }else
            {
                premierplan = false;
                tokkiplan = false;
            }
        }
        else if(repartition <= 95)//10% Grande
        {
            attribut = "grand";
            tokkiplan = true;
            petit =2f;
            grand =3f;
            
            if(repartition <= 92)//60%
            {
                premierplan = true;
            }else
            {
                premierplan = false;
            }
        }
        else//5% Immense
        {
            attribut = "immense";
            premierplan = true;
            tokkiplan = true;
            petit =3f;
            grand =4f;
        }

        ratioCarotte = Random.Range(petit,grand);
        Vector2 tailleCarotte = transform.GetComponent<RectTransform>().sizeDelta;

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(tailleCarotte.x*ratioCarotte,tailleCarotte.y*ratioCarotte);
    }

    private void orientation()
    {
        sensCarotte = Random.Range(-180,180);
        transform.rotation = Quaternion.Euler(Vector3.forward * sensCarotte);
    }

    private void rotation()
    {
        int sens = 1;
        if(Random.value<0.5f)sens = -1;

        float vitesse = 3f;

        if(sensCarotte <= 15 && sensCarotte >= -15)
        {
            deltaRotation = 0f;
            return;
        }
        else
        {
            switch(attribut)
            {
                case "minuscule":
                    vitesse = 9f;
                break;
                case "petit":
                    vitesse = 6f;
                break;
                case "normal":
                    vitesse = 3f;
                break;
                case "grand":
                    vitesse = 2f;
                break;
                case "immense":
                    vitesse = 1f;
                break;
            }

            deltaRotation = Random.Range(0.5f*sens,vitesse*sens);
            return;
        }
        
        
    }

    private void deplacement()
    {
        float decalage = 0f;

        switch(attribut)
        {
            case "minuscule":
                speed = Random.Range(14f,27f);
            break;
            case "petit":
                speed = Random.Range(14f,26f);
            break;
            case "normal":
                speed = Random.Range(15f,24f);
            break;
            case "grand":
                speed = Random.Range(16f,22f);
            break;
            case "immense":
                speed = Random.Range(18f,20f);
            break;
        }

        if(premierplan)
        {
            speed += 2;
        }

        if(tokkiplan)
        {
            speed += 1;
        }

        if(deltaRotation == 0)
        {
            decalage = sensCarotte/2;
            speed = speed *1.5f;
        }

        rb.velocity = new Vector2(decalage*50,-speed*50);
    }

    private void ombre_init()
    {
        switch(attribut)
        {
            case "minuscule":
                ombre_x = 25;
                ombre_y = 25;
            break;
            case "petit":
                ombre_x = 50;
                ombre_y = 50;
            break;
            case "normal":
                ombre_x = 100;
                ombre_y = 100;
            break;
            case "grand":
                ombre_x = 150;
                ombre_y = 150;
            break;
            case "immense":
                ombre_x = 200;
                ombre_y = 200;
            break;
        }
    }

    private void ombre()
    {
        
        float angle = transform.localEulerAngles.z;
        bool x_100 = false;
        float x = 100;
        float y = 100f;
        float diff = 0f;
        float x_cent = 100f/45f;
        float y_cent = 50f/45f;
        float x_plus = 1f;
        float y_plus = 1f;

        if(angle < 45)
        {
            x_100 = false;
            
            diff = angle;
            x = 100f;
            y = 100f;
            x_plus = 1f;
            y_plus = -1f;

        }else if(angle < 90)
        {
            x_100 = false;
            x_plus = -1f;
            y_plus = -1f;

            diff = angle-45f;
            x = 150f;
            y = 0f;

        }
        else if(angle < 135)
        {
            x_100 = true;
            x_plus = -1f;
            y_plus = -1f;

            diff = angle-90f;
            x = 100f;
            y = -100f;
            
        }
        else if(angle < 180)
        {
            x_100 = true;
            x_plus = -1f;
            y_plus = 1f;

            diff = angle-135f;
            x = 0f;
            y = -150f;
            
        }
        else if(angle < 225)
        {
            x_100 = false;
            x_plus = -1f;
            y_plus = 1f;

            diff = angle-180f;
            x = -100f;
            y = -100f;

        }
        else if(angle < 270)
        {
            x_100 = false;
            x_plus = 1f;
            y_plus = 1f;

            diff = angle-225f;
            x = -150f;
            y = 0f;
            
        }
        else if(angle < 315)
        {
            x_100 = true;
            x_plus = 1f;
            y_plus = 1f;

            diff = angle-270f;
            x = -100f;
            y = 100f;
        }
        else if(angle < 360)
        {
            x_100 = true;
            x_plus = 1f;
            y_plus = -1f;

            diff = angle-315f;
            x = 0f;
            y = 150f;
        }

        if(!x_100)
        {
            x_cent = 50f/45f;
            y_cent = 100f/45f; 
        }

        x = x + (diff * x_cent * x_plus);
        y = y + (diff * y_cent * y_plus);

        
        GetComponent<Shadow>().effectDistance = new Vector2(x,y);
    }
}
