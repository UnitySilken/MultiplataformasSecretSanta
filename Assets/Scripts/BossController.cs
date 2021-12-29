using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float speed = 1f;
    public float maxSpeed = 1f;
    private Rigidbody2D rb2;
    public int BossLife;
    public Text lifeCanvas;
    private Animator anim;
    private bool CanMove = true;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lifeCanvas.text = BossLife.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanMove)
        {
            rb2.AddForce(Vector2.right * speed);
            float LimitedSpeed = Mathf.Clamp(rb2.velocity.x, -maxSpeed, maxSpeed);
            rb2.velocity = new Vector2(LimitedSpeed, rb2.velocity.y);

            if (rb2.velocity.x < 0.01f && rb2.velocity.x > -0.01f)
            {
                speed *= -1;
                rb2.velocity = new Vector2(speed, rb2.velocity.y);
            }
            if (speed > 0)
            {
                transform.localScale = new Vector3(-5f, 5f, 1f);
            }
            else if (speed < 0)
            {
                transform.localScale = new Vector3(5f, 5f, 1f);
            }
        }
        else
        {
            rb2.velocity = Vector2.zero;
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float yOffset = 0.3f;
            if (transform.position.y + yOffset < collision.transform.position.y)
            {

                if (BossLife > 0)
                {
                    CanMove = false;
                    anim.SetBool("TakenDamage", true);
                    Invoke("Move", 1f);
                    --BossLife;
                    lifeCanvas.text = BossLife.ToString();
                }
                if (BossLife <= 0)
                {
                    Destroy(gameObject);
                    camera.GetComponent<CameraFollow>().maxCamPos.x=16;
                }
                collision.SendMessage("EnemyJump");
            }
            else
            {
                collision.SendMessage("EnemyKnockback", transform.position.x);
            }
        }
    }
    void Move()
    {
        CanMove = true;
        anim.SetBool("TakenDamage", false);
    }
}
