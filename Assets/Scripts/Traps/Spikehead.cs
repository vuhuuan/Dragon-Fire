using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header("Spikehead's attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float delay;
    [SerializeField] private AudioClip impactSound;

    private Rigidbody2D rb;
    private Vector2 destination;
    private bool attacking;

    private float delayTimer;

    private Vector2[] directions = new Vector2[4];

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Calm();
    }

    private void Update()
    {
        if (attacking)
        {
            rb.velocity = destination.normalized * speed;
        }
        else
        {
            delayTimer += Time.deltaTime;
            if (delayTimer > delay)
            {
                FindPlayer();
            }
        }
    }

    private void FindPlayer()
    {
        CalculateDirection();
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);
            if (hit.collider != null && !attacking)
            {
                destination = directions[i];
                attacking = true;
                delayTimer = 0;
            }
        }
    }

    private void Calm()
    {
        rb.velocity = Vector2.zero;
        attacking = false;
    }

    private void CalculateDirection()
    {
        directions[0] = Vector2.right * range;
        directions[1] = -Vector2.right * range;
        directions[2] = Vector2.up * range;
        directions[3] = -Vector2.up * range;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        SoundManager.instance.PlaySound(impactSound);
        if (collision.tag != "Player")
        {
            Calm();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Calm();
    }
}
