using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header ("Movement Parameters")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpPower = 10;

    [SerializeField] private float gravityScale = 3f;

    private bool facingLeft = false;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    [SerializeField] private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpCoolDown = 0.6f;
    private float wallJumpCounter;
    private bool encounterWall = false;


    [Header("Layer")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [Header ("SFX")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] ParticleSystem dust;
    //[SerializeField] private AudioClip fireballSound;
    //[SerializeField] private AudioClip fireballSound;


    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    Animator animator;

    bool grounded = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wallJumpCounter = wallJumpCoolDown;

        animator = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();

        rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // player flip to move left-right
        if (horizontalInput > 0.01f)
        {
            if (facingLeft)
            {
                facingLeft = false;
                if (isGrounded())
                {
                    dust.Play();
                }
            }
            transform.localScale = Vector3.one;
        } else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (!facingLeft)
            {
                facingLeft = true;
                if (isGrounded())
                {
                    dust.Play();
                }
            }
        }

        // set animator parameters
        animator.SetBool("walk", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());

        // jump logic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // early release space
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        }

        // quick fall
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * 1.6f;
        } else
        {
            rb.gravityScale = gravityScale;
        }

        // wall Logic
        if (onWall() && !isGrounded())
        {
            animator.SetBool("wall", true);
            if (!encounterWall)
            {
                encounterWall = true;
            }
            coyoteCounter = coyoteTime;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -1, float.MaxValue));
        }
        else
        {
            animator.SetBool("wall", false);

            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
            
            if (isGrounded())
            {
                // always can jump if is ground;
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            }
            else
            {
                coyoteCounter -= Time.deltaTime;
            }
        }
        wallJumpCounter -= Time.deltaTime;
    }

    void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0)
        {
            return;
        }
        jumpCounter -= 1;
        Debug.Log("Hello!");

        if (onWall())
        {
            if (wallJumpCounter <= 0)
            {
                wallJumpCounter = wallJumpCoolDown;
                WallJump();
            }
        } else
        {
            if (isGrounded() )
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                SoundManager.instance.PlaySound(jumpSound);
                dust.Play();

            }
            else
            {
                // can jump in the air while coyote still > 0;
                if (coyoteCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    SoundManager.instance.PlaySound(jumpSound);
                    dust.Play();

                }
                else
                {
                    // can jump in the air if jumpCounter still > 0;

                    if (jumpCounter > 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                        SoundManager.instance.PlaySound(jumpSound);
                        dust.Play();

                    }
                }
            }
        }
        coyoteCounter = 0;
    }

    private void WallJump()
    {
        SoundManager.instance.PlaySound(jumpSound);
        dust.Play();

        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * jumpPower * 2, jumpPower);

        rb.gravityScale = gravityScale;
        encounterWall = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    bool touchGround = true;
    bool isGrounded()
    {

        RaycastHit2D raycasthit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        if (raycasthit.collider)
        {
            if (!touchGround)
            {
                touchGround = true;
                dust.Play();
            }
        } else
        {
            touchGround= false;
        }

        return raycasthit.collider != null;
    }

    private bool onWall()
    {
        float horizontal = Input.GetAxis("Horizontal");

        RaycastHit2D raycasthit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector3 (boxCollider.bounds.size.x, boxCollider.bounds.size.y + 0.2f, boxCollider.bounds.size.z), 0, new Vector2(horizontal, 0), 0.1f, wallLayer);

        return raycasthit.collider != null;
    }


    bool isFalling()
    {
        return rb.velocity.y <= 0;
    }

    public bool canAttack()
    {
        return isGrounded() && !onWall();
    }

    public void Die()
    {
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }
}
