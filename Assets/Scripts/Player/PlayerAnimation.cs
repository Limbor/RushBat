﻿using System.Collections;
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
    private int ySpeed;
    private int slide;
    private int attack;
    private int dash;
    private int glide;
    private int climb;
    private int skill3;
    private int skill2;
    private int skill4;
    private int die;
    private int hurt;
    private int acquire;
    private int roomIn;
    private int relive;

    private float fade;

    public GameObject smoke;

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
        ySpeed = Animator.StringToHash("ySpeed");
        slide = Animator.StringToHash("slide");
        attack = Animator.StringToHash("attack");
        dash = Animator.StringToHash("dash");
        glide = Animator.StringToHash("glide");
        climb = Animator.StringToHash("climb");
        skill3 = Animator.StringToHash("skill3");
        skill2 = Animator.StringToHash("skill2");
        skill4 = Animator.StringToHash("skill4");
        die = Animator.StringToHash("die");
        hurt = Animator.StringToHash("hurt");
        acquire = Animator.StringToHash("acquire");
        roomIn = Animator.StringToHash("roomIn");
        relive = Animator.StringToHash("relive");

        fade = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(speed, Mathf.Abs(player.xVelocity));
        animator.SetBool(ground, player.isOnGround);
        animator.SetBool(jump, player.isDoubleJumping);
        animator.SetFloat(verticalVelocity, rb.velocity.y);
        animator.SetFloat(ySpeed, player.yVelocity);
        animator.SetBool(slide, player.isSliding);
        animator.SetBool(attack, player.isAttacking);
        animator.SetBool(dash, player.isDashing);
        animator.SetBool(glide, player.isGliding);
        animator.SetBool(climb, player.isClimbing);
        if(!player.canMove) animator.SetBool(acquire, false);
    }

    public void EnterRoom()
    {
        animator.SetTrigger(roomIn);
    }

    public void Acquire()
    {
        if (!player.canMove) return;
        animator.SetBool(acquire, true);
        StartCoroutine(EndAcquire());
    }

    IEnumerator EndAcquire()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool(acquire, false);
    }
    
    public void StartSkill3()
    {
        animator.SetTrigger(skill3);
    }

    public void StartSkill2()
    {
        animator.SetTrigger(skill2);
    }

    public void StartSkill4()
    {
        animator.SetTrigger(skill4);
    }

    public void Die()
    {
        animator.SetTrigger(die);
    }

    public void Hurt()
    {
        animator.SetTrigger(hurt);
    }

    public void StartSmoke()
    {
        smoke.SetActive(true);
    }

    public void Disappear()
    {
        PlayerProperty property = GetComponent<PlayerProperty>();
        if (property.HaveEquipment("GoldenApple") && !Player.GetInstance().hasRelived)
        {
            AudioManager.GetInstance().PlayRecoverAudio();
            Player.GetInstance().hasRelived = true;
            animator.SetTrigger(relive);
            property.isDead = false;
            property.SetHealth(4);
            return;
        }
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Material material = GetComponent<SpriteRenderer>().material;
        do
        {
            yield return new WaitForSeconds(0.1f);
            fade -= 0.1f;
            material.SetFloat("_Fade", fade);
        } while (fade > 0);
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<Collider2D>().enabled = false;
        Player.GetInstance().Reset();
        GameManager.GetInstance().GameOver();
    }
}
