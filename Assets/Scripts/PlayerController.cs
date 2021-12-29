using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed=7f;
    public float maxSpeed = 10f;
    public bool grounded;
    public float jumpPower=6.5f;
    public Vector2 spawn;
    public Text lifeCanvas;
    public Canvas canvas;

    private int Lifes;
    private Animator anim;
    private Rigidbody2D rb2;
    private bool isJumping;
    private bool canDoubleJump;
    private bool movement=true;
    private string NextScene;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        Lifes = Singleton.instance.Lifes;
        ModifyLifes();
        movement = false;
        Invoke("EnableMovement", 0.6f);
        rb2 = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb2.velocity.x));
        anim.SetBool("Grounded", grounded);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (grounded)
            {
                isJumping = true;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                isJumping = true;
                canDoubleJump = false;
            }


        }
    }
    void FixedUpdate()
    {
        
        Vector3 fixedVelocity = rb2.velocity;
        fixedVelocity.x = fixedVelocity.x * 0.75f;
        if (grounded)
        {
            rb2.velocity = fixedVelocity;
            canDoubleJump = true;
        }
        float h = Input.GetAxis("Horizontal");
        if (!movement)
        {
            h = 0;
        }
        rb2.AddForce(Vector2.right*speed*h);
        float LimitedSpeed = Mathf.Clamp(rb2.velocity.x, -maxSpeed, maxSpeed);
        rb2.velocity = new Vector2(LimitedSpeed,rb2.velocity.y);
        if (h>0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if(h<-0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (isJumping)
        {
            rb2.velocity = new Vector2(rb2.velocity.x,0);
            rb2.AddForce(Vector2.up*jumpPower,ForceMode2D.Impulse);
            isJumping = false;
        }
    }
    void OnBecameInvisible()
    {
        
    }
    public void EnemyJump()
    {
        isJumping = true;
        FindObjectOfType<AudioManager>().Play("JumpOnEnemy");
    }
    public void EnemyKnockback(float EnemyPosX)
    {
        ReduceLife();
        isJumping = true;
        float side= Mathf.Sign(EnemyPosX-transform.position.x);
        rb2.AddForce(Vector2.left * side * jumpPower, ForceMode2D.Impulse);
        movement = false;
        anim.SetBool("TakenDamage",true);
        Invoke("EnableMovement", 0.7f);
    }
    void EnableMovement()
    {
        anim.SetBool("TakenDamage", false);
        movement = true;
    }
    public void DeathMethod()
    {
        if (isDead)
        {
            return;
        }
        ReduceLife();
        rb2.velocity = Vector2.zero;
        movement = false;
        anim.SetBool("TakenDamage", true);
        Invoke("EnableMovement", 0.6f);
        Invoke("Respawn",0.6f);
        isJumping = true;
        isDead = true;
    }
    void Respawn()
    {
        transform.position = new Vector3(spawn.x, spawn.y, 0);
        isDead = false;
    }
    public void WinMethod(object Scene)
    {
        movement = false;
        anim.SetBool("Win", true);
        NextScene = Scene.ToString();
        Singleton.instance.Lifes = Lifes;
        rb2.velocity = Vector2.zero;
        Invoke("ChangeScene",1);
    }
    void ChangeScene()
    {
        SceneManager.LoadScene(NextScene);
    }
    void ModifyLifes()
    {
        lifeCanvas.text = Lifes.ToString();
    }
    public void AddLife()
    {
        FindObjectOfType<AudioManager>().Play("Life");
        Lifes++;
        ModifyLifes();
    }   
    void ReduceLife()
    {
        Lifes--;
        FindObjectOfType<AudioManager>().Play("Damage");
        ModifyLifes();
        //if (Lifes <= 0)
        //{
        //    canvas.enabled = true;
        //    Invoke("reset", 1);
        //}
    }
    private void Reset()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
        Singleton.instance.Lifes = 5;
        SceneManager.LoadScene("FistLevel");
    }
}
