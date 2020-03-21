using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bodyCollider;

    private bool jumpPressed = false;
    private float validTime;
    private bool attackHeld;

    [Header("Movement")]
    public float speed;
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
    public bool isJumping;
    public bool isOnGround;
    public bool isSliding;
    public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            validTime = Time.time + Time.fixedDeltaTime;
        }
        if (jumpPressed && Time.time >= validTime) jumpPressed = false;
        attackHeld = Input.GetMouseButton(0);
    }

    void FixedUpdate()
    {
        yVelocity = rb.velocity.y;
        EnvironmentCheck();
        Attack();
        Move();
        Jump();
    }

    private void Attack()
    {
        if (isSliding) return;
        isAttacking = attackHeld;
    }

    private void EnvironmentCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, -footHeight), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, -footHeight), Vector2.down, groundDistance, groundLayer);
        isOnGround = (leftCheck || rightCheck);

        float direction = transform.localScale.x;
        RaycastHit2D handCheck = Raycast(new Vector2(direction * handOffset, 0),
            Vector2.right * direction, groundDistance, groundLayer);
        RaycastHit2D footCheck = Raycast(new Vector2(direction * handOffset, -footHeight),
            Vector2.right * direction, groundDistance, groundLayer);
        if (!isOnGround && handCheck && footCheck && rb.velocity.y < 0f)
        {
            if (!isSliding)
            {
                isJumping = false;
                Vector2 pos = transform.position;
                pos.x += handCheck.distance * direction - 0.05f * direction;
                transform.position = pos;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = new Vector2(0, -slideSpeed);
            }
            isSliding = true;
        }
        else
        {
            isSliding = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void Move()
    {
        if (isSliding) return;
        if (isAttacking && isOnGround)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        xVelocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xVelocity * speed * Time.fixedDeltaTime, rb.velocity.y);
        FlipDirection();
    }

    private void FlipDirection()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        if(direction != 0)
            transform.localScale = new Vector3(direction, 1, 1);
    }

    private void Jump()
    {
        if (isSliding)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.AddForce(new Vector2(3f, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
                jumpPressed = false;
                isSliding = false;
            }
            return;
        }
        if (isJumping && isOnGround) isJumping = false;
        if (jumpPressed && isOnGround)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            jumpPressed = false;
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 direction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, direction, length, layer);
        Debug.DrawRay(pos + offset, direction * length, hit ? Color.red : Color.green);
        return hit;
    }
}
