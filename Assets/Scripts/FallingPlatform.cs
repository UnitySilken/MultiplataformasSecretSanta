using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    public float fallDelay=1f;
    public float respawnDelay = 5f;

    private Rigidbody2D rb2;
    private PolygonCollider2D pc2;
    private Vector3 origen; 
    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        pc2 = GetComponent<PolygonCollider2D>();
        origen = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Fall", fallDelay);
            Invoke("Respawn", fallDelay + respawnDelay);
        }
    }
    void Fall()
    {
        rb2.isKinematic = false;
        pc2.isTrigger = true;
    }
    void Respawn()
    {
        rb2.isKinematic = true;
        pc2.isTrigger = false;
        rb2.velocity = Vector2.zero;
        transform.position = origen;
    }
}
