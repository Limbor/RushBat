using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bodyCollider;

    private bool doubleJump = true;
    private int jumpCount;

    private float lastDashTime = -10f;
    private float dashColdDown = 2f;
    private float dashTime = 0.15f;
    private float dashTimeLeft;
    
    private bool jumpPressed = false;
    private float jumpPressTime;
    private bool dashPressed = false;
    private float dashPressTime;
    private bool attackHeld;

    [Header("Movement")]
    public float speed;
    public float dashSpeed;
    public float climbSpeed;
    public float jumpForce;
    public float slideSpeed;

    [Header("Environment")]
    public LayerMask groundLayer;
    public float footOffset = 0.1f;
    public float footHeight = 0.25f;
    public float handOffset = 0.15f;
    public float groundDistance = 0.1f;

    [Header("State")]
    public float xVelocity;
    public float yVelocity;
    public bool isDoubleJumping;
    public bool isOnGround;
    public bool isSliding;
    public bool isAttacking;
    public bool isDashing;
    public bool isClimbing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();

        jumpCount = doubleJump ? 2 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            jumpPressTime = Time.time + Time.fixedDeltaTime;
        }
        if (jumpPressed && Time.time >= jumpPressTime) jumpPressed = false;
        if (Input.GetButtonDown("Dash"))
        {
            dashPressed = true;
            dashPressTime = Time.time + Time.fixedDeltaTime;
        }
        if (dashPressed && Time.time >= dashPressTime) dashPressed = false;
        attackHeld = Input.GetMouseButton(0);
    }

    void FixedUpdate()
    {
        yVelocity = rb.velocity.y;
        EnvironmentCheck();
        Dash();
        Attack();
        GroundMove();
        AirMove();
    }

    private void Dash()
    {
        if (isSliding) return;
        if (dashPressed && Time.time > lastDashTime + dashColdDown)
        {
            lastDashTime = Time.time;
            dashTimeLeft = dashTime;
            float direction = transform.localScale.x;
            isDashing = true;
            dashPressed = false;
            rb.velocity = new Vector2(direction * Time.fixedDeltaTime * dashSpeed, 0);
            rb.gravityScale = 0f;
        }
        if (isDashing)
        {
            if(dashTimeLeft <= 0)
            {
                isDashing = false;
                rb.gravityScale = 1f;
            }
            dashTimeLeft -= Time.fixedDeltaTime;
            PoolManager.getInstance().GetObject();
        }
    }

    private void Attack()
    {
        if (isSliding || isDashing) return;
        isAttacking = attackHeld;
    }

    private void EnvironmentCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, -footHeight), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, -footHeight), Vector2.down, groundDistance, groundLayer);
        isOnGround = (leftCheck || rightCheck);
        if (isOnGround)
        {
            isDoubleJumping = false;
            jumpCount = doubleJump ? 2 : 1;
        }

        float direction = transform.localScale.x;
        RaycastHit2D handCheck = Raycast(new Vector2(direction * handOffset, 0),
            Vector2.right * direction, groundDistance, groundLayer);
        RaycastHit2D footCheck = Raycast(new Vector2(direction * handOffset, -footHeight),
            Vector2.right * direction, groundDistance, groundLayer);
        if (!isOnGround && !isAttacking && !isDashing && handCheck && footCheck && rb.velocity.y < 0f)
        {
            if (!isSliding)
            {
                jumpCount = 1;
                isDoubleJumping = false;
                Vector2 pos = transform.position;
                pos.x += handCheck.distance * direction - 0.05f * direction;
                transform.position = pos;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = new Vector2(0, -slideSpeed);
            }
            isSliding = true;
        }
        else if(isSliding)
        {
            
            isSliding = false;
            jumpCount = doubleJump ? 2 : 1;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void GroundMove()
    {
        if (isSliding || isDashing) return;
        xVelocity = Input.GetAxis("Horizontal");
        if (isAttacking && isOnGround)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else rb.velocity = new Vector2(xVelocity * speed * Time.fixedDeltaTime, rb.velocity.y);
        FlipDirection();
    }

    private void FlipDirection()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        if(direction != 0)
            transform.localScale = new Vector3(direction, 1, 1);
    }

    private void AirMove()
    {
        if (isDashing) return;
        if (isSliding)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                Jump();
                isSliding = false;
            }
            return;
        }
        if (jumpPressed && jumpCount > 0)
        {
            if (doubleJump && jumpCount == 1) isDoubleJumping = true;
            Jump();
        }
    }

    private RaycastHit2D Raycast(Vector2 offset, Vector2 direction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, direction, length, layer);
        Debug.DrawRay(pos + offset, direction * length, hit ? Color.red : Color.green);
        return hit;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        jumpCount--;
        jumpPressed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }
}
