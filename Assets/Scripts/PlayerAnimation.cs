using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement player;
    private Animator animator;
    private Rigidbody2D rb;

    private int speed;
    private int ground;
    private int jump;
    private int verticalVelocity;
    private int slide;
    private int attack;
    private int dash;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        speed = Animator.StringToHash("speed");
        ground = Animator.StringToHash("ground");
        jump = Animator.StringToHash("jump");
        verticalVelocity = Animator.StringToHash("verticalVelocity");
        slide = Animator.StringToHash("slide");
        attack = Animator.StringToHash("attack");
        dash = Animator.StringToHash("dash");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(speed, Mathf.Abs(player.xVelocity));
        animator.SetBool(ground, player.isOnGround);
        animator.SetBool(jump, player.isDoubleJumping);
        animator.SetFloat(verticalVelocity, rb.velocity.y);
        animator.SetBool(slide, player.isSliding);
        animator.SetBool(attack, player.isAttacking);
        animator.SetBool(dash, player.isDashing);
    }
}
