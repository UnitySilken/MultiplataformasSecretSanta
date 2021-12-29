using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;
    public float maxSpeed = 1f;
    private Rigidbody2D rb2;

    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2.AddForce(Vector2.right * speed);
        float LimitedSpeed = Mathf.Clamp(rb2.velocity.x, -maxSpeed, maxSpeed);
        rb2.velocity = new Vector2(LimitedSpeed, rb2.velocity.y);

        if (rb2.velocity.x<0.01f && rb2.velocity.x>-0.01f)
        {
            speed *= -1;
            rb2.velocity = new Vector2(speed, rb2.velocity.y);
        }
        if (speed > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }else if (speed < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            float yOffset = 0.3f;
            if (transform.position.y + yOffset < collision.transform.position.y)
            {
                Destroy(gameObject);
                collision.SendMessage("EnemyJump");
            }
            else
            {
                collision.SendMessage("EnemyKnockback",transform.position.x);
            }
        }    
    }
}
