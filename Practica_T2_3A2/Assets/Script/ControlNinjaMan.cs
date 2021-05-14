using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNinjaMan : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator _animator;
    private SpriteRenderer sr;
    private Transform _transform;

    private float speed = 15f;
    private float Saltar = 30f;
    private int numSalto = 0;
    private int vidas = 3;

    public GameObject KunaiRigth;
    public GameObject KunaiLeft;

    public VidaText Vida;
    public ControlPuntaje Puntaje;


    private bool trepar = false;
    private bool muerte = false;
    private bool planear = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        //Movimientos
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            AnimacionCorrer();
            sr.flipX = false;
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            AnimacionCorrer();
            sr.flipX = true;
        }
        else
        {
            AnimacionQuieto();
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        //Para saltar
        if (Input.GetKeyDown(KeyCode.UpArrow) && numSalto < 2)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(rb2d.velocity.x, Saltar), ForceMode2D.Impulse);
            AnimacionSaltar();
            numSalto++;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
           AnimacionAgachar();
        }

        if (trepar)
        {
            rb2d.gravityScale = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            AnimacionTrepar();
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, speed);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -speed);
            }
        }

        if (!trepar)
            rb2d.gravityScale = 10;

        if (Input.GetKeyUp(KeyCode.A))
        {
            AnimacionAtacar();
            if (!sr.flipX)
            {
                var KunaiPosition = new Vector2(_transform.position.x + 3f, _transform.position.y);
                Instantiate(KunaiRigth, KunaiPosition, KunaiRigth.transform.rotation);
            }

            if (sr.flipX)
            {
                var KunaiPosition = new Vector2(_transform.position.x - 3f, _transform.position.y);
                Instantiate(KunaiLeft, KunaiPosition, KunaiLeft.transform.rotation);
            }
        }

        if (muerte)
            AnimacionMuere();

        if (Input.GetKey(KeyCode.C) && planear)
        {
            rb2d.gravityScale = 1;
            numSalto = 2;
            AnimacionPlaneando();
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb2d.velocity = new Vector2(speed, -speed);
                sr.flipX = false;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb2d.velocity = new Vector2(-speed, -speed);
                sr.flipX = true;
            }
        }
        //CaidaMuere();
        Debug.Log(rb2d.velocity.y);
        if (rb2d.velocity.y < -65)
            muerte = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemigo")
        {
            vidas--;
            if (vidas == 0) 
                muerte = true;
            if (vidas >= 0)
            {
                Vida.QuitarVida(1);
            }

        }

        if (other.gameObject.layer == 8)
        {
            numSalto = 0;
        }

        if (other.gameObject.layer == 8)
        {
            numSalto = 0;
            planear = false;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Plataforma")
        {
            planear = true;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Escalera")
        {
            trepar = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Escalera")
        {
            trepar = false;
        }
    }

    //Animaciones

    private void AnimacionQuieto()
    {
        _animator.SetInteger("Estado", 0);
    }
    
    private void AnimacionCorrer()
    {
        _animator.SetInteger("Estado", 1);
    }
    
    
    private void AnimacionSaltar()
    {
        _animator.SetInteger("Estado", 2);
    }
    
    private void AnimacionAtacar()
    {
        _animator.SetInteger("Estado", 3);
    }
    
    private void AnimacionAgachar()
    {
        _animator.SetInteger("Estado", 4);
    }
    
    private void AnimacionTrepar()
    {
        _animator.SetInteger("Estado", 5);
    }
    
    private void AnimacionPlaneando()
    {
        _animator.SetInteger("Estado", 6);
    }
    private void AnimacionMuere()
    {
        _animator.SetInteger("Estado", 7);
    }
}
