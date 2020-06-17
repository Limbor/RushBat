using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimation anim;
    private PlayerProperty property;

    private bool doubleJump;
    private int jumpCount;
    public bool avoidDamage = false;
    
    private float dashCoolDown;
    private float dashTime = 0.15f;
    private float dashTimeLeft;

    private float skill1CoolDown;
    private float skill2CoolDown;
    private float skill3CoolDown;

    private bool jumpPressed = false;
    private float jumpPressTime;
    private bool dashPressed = false;
    private float dashPressTime;
    private bool glideHeld;
    private bool attackHeld;

    [Header("Movement")]
    public float speed;
    public float dashSpeed;
    public float climbSpeed;
    public float jumpForce;
    public float slideSpeed;
    public float glideGravity;

    [Header("Environment")]
    public LayerMask groundLayer;
    public float footOffset = 0.1f;
    public float footHeight = 0.25f;
    public float handOffset = 0.15f;
    public float headOffset = 0.2f;
    public float groundDistance = 0.2f;

    [Header("State")]
    public float xVelocity;
    public float yVelocity;
    public float airSpeed;
    public bool canMove = true;
    public bool isDoubleJumping;
    public bool isInAir;
    public bool isOnGround;
    public bool isSliding;
    public bool isGliding;
    public bool isAttacking;
    public bool isDashing;
    public bool isClimbing;
    public bool isOnLadder;
    public bool isHurting;

    private Player player;
    private void Awake()
    {
        GameManager.GetInstance().RegisterPlayer(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.GetInstance();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimation>();
        property = GetComponent<PlayerProperty>();

        dashCoolDown = player.dashCoolDown;
        skill1CoolDown = player.skill1CoolDown;
        skill2CoolDown = player.skill2CoolDown;
        skill3CoolDown = player.skill3CoolDown;
        UIManager.GetInstance().ResetDashTime(dashCoolDown / property.dashCoolDown);
        UIManager.GetInstance().ResetSkillTime(0, skill1CoolDown / property.skill1CoolDown);
        UIManager.GetInstance().ResetSkillTime(1, skill2CoolDown / property.skill2CoolDown);
        UIManager.GetInstance().ResetSkillTime(2, skill3CoolDown / property.skill3CoolDown);
    }

    // Update is called once per frame
    void Update()
    {
        if (property.isDead) return;
        if (!doubleJump)
        {
            doubleJump = property.HaveEquipment("Wing");
        }
        GetMoveButton();
        GetSkillButton();
        SkillCoolDown();
    }

    private void SkillCoolDown()
    {
        if (skill1CoolDown > 0)
        {
            skill1CoolDown -= Time.deltaTime;
        }
        if (skill2CoolDown > 0)
        {
            skill2CoolDown -= Time.deltaTime;
        }
        if (skill3CoolDown > 0)
        {
            skill3CoolDown -= Time.deltaTime;
        }
        if (dashCoolDown > 0)
        {
            dashCoolDown -= Time.deltaTime;
        }
        
        player.dashCoolDown = dashCoolDown;
        player.skill1CoolDown = skill1CoolDown;
        player.skill2CoolDown = skill2CoolDown;
        player.skill3CoolDown = skill3CoolDown;
    }
    
    private void GetSkillButton()
    {
        if (isDashing || !canMove || isSliding || isGliding || isClimbing) return;
        if (InputManager.GetButtonDown("Skill3") && skill2CoolDown <= 0)
        {
            UIManager.GetInstance().ResetSkillTime(1);
            rb.velocity = new Vector2(0, rb.velocity.y);
            canMove = false;
            anim.StartSkill3();
            skill2CoolDown = property.skill2CoolDown;
        }
        if (InputManager.GetButtonDown("Skill2") && skill1CoolDown <= 0)
        {
            UIManager.GetInstance().ResetSkillTime(0);
            anim.StartSkill2();
            skill1CoolDown = property.skill1CoolDown;
        }
        if (InputManager.GetButtonDown("Skill4") && skill3CoolDown <= 0)
        {
            UIManager.GetInstance().ResetSkillTime(2);
            rb.velocity = new Vector2(0, 0);
            canMove = false;
            anim.StartSkill4();
            skill3CoolDown = property.skill3CoolDown;
            rb.gravityScale = 0f;
        }
    }

    private void GetMoveButton()
    {
        xVelocity = InputManager.GetAxis("Horizontal");
        yVelocity = InputManager.GetAxis("Vertical");
        glideHeld = InputManager.GetButton("Jump");
        if (InputManager.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            jumpPressTime = Time.time + Time.fixedDeltaTime;
        }
        if (jumpPressed && Time.time >= jumpPressTime) jumpPressed = false;
        if (InputManager.GetButtonDown("Dash"))
        {
            dashPressed = true;
            dashPressTime = Time.time + Time.fixedDeltaTime;
        }
        if (dashPressed && Time.time >= dashPressTime) dashPressed = false;
        attackHeld = InputManager.GetMouseButton(0);
    }

    void FixedUpdate()
    {
        if (property.isDead) return;
        airSpeed = rb.velocity.y;
        if (Mathf.Abs(airSpeed) >= 3f) isInAir = true;
        EnvironmentCheck();
        Climb();
        Dash();
        Attack();
        GroundMove();
        AirMove();
    }

    public void Hurt(int damage, Vector2 force, string source)
    {
        if (avoidDamage) return;
        if (source.Equals(GameManager.GroundTrap) && property.HaveEquipment("Helmet")) return;
        if (source.Equals(GameManager.FlyingTrap) && property.HaveEquipment("SubmarineHelmet")) return;
        property.Hurt(damage);
        UIManager.GetInstance().Hurt();
        Camera.main.GetComponent<CameraController>().Shake();
        if (!property.isDead)
        {
            isHurting = true;
            anim.Hurt();
            canMove = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(force, ForceMode2D.Impulse);
            avoidDamage = true;
            StartCoroutine(Recover());
        }
    }

    public void EnterDoor(bool sideDoor, float orientation, int roomId)
    {
        canMove = false;
        avoidDamage = true;
        rb.gravityScale = 0f;
        if (!sideDoor)
        {
            rb.velocity = Vector2.zero;
            anim.EnterRoom();
        }
        GameManager.GetInstance().NextRoom(orientation, roomId);
        gameObject.AddComponent<ActionTimer>().SetTimer(1.5f, () =>
        {
            canMove = true;
            avoidDamage = false;
            rb.gravityScale = 1f;
        });
    }
    
    private void Climb()
    {
        if (!canMove || isDashing || isHurting) return;
        if(isOnLadder && isOnGround)
        {
            if(!isClimbing && Mathf.Abs(yVelocity) != 0)
            {
                isClimbing = true;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, yVelocity * climbSpeed * Time.fixedDeltaTime);
            if (!isOnLadder)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isClimbing = false;
            }
        }
    }

    private void Dash()
    {
        if (isSliding || isGliding || isClimbing || isHurting) return;
        if (dashPressed && dashCoolDown <= 0 && canMove)
        {
            UIManager.GetInstance().ResetDashTime();
            PoolManager.GetInstance().GetDustObject(true);
            AudioManager.GetInstance().PlayDashAudio();
            Camera.main.GetComponent<CameraController>().Shake();
            Camera.main.GetComponent<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            dashCoolDown = property.dashCoolDown;
            dashTimeLeft = dashTime;
            float direction = transform.localScale.x;
            isDashing = true;
            dashPressed = false;
            rb.velocity = new Vector2(direction * Time.fixedDeltaTime * dashSpeed, 0);
            rb.gravityScale = 0f;
            avoidDamage = true;
        }
        if (isDashing)
        {
            if(dashTimeLeft <= 0)
            {
                isDashing = false;
                rb.gravityScale = 1f;
                avoidDamage = false;
            }
            dashTimeLeft -= Time.fixedDeltaTime;
            PoolManager.GetInstance().GetShadowObject();
        }
    }

    private void Attack()
    {
        if (isSliding || isDashing || isClimbing || isHurting) return;
        isAttacking = attackHeld;
    }

    private void EnvironmentCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, -footHeight), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, -footHeight), Vector2.down, groundDistance, groundLayer);
        isOnGround = (leftCheck || rightCheck);
        if (isOnGround && airSpeed < 0.1f)
        {
            if(isInAir)
            {
                isInAir = false;
                PoolManager.GetInstance().GetDustObject(true);
            }
            isDoubleJumping = false;
            jumpCount = doubleJump ? 2 : 1;
        }

        float direction = transform.localScale.x;
        RaycastHit2D headCheck = Raycast(new Vector2(direction * handOffset, headOffset),
            Vector2.right * direction, groundDistance, groundLayer);
        RaycastHit2D handCheck = Raycast(new Vector2(direction * handOffset, 0),
            Vector2.right * direction, groundDistance, groundLayer);
        RaycastHit2D footCheck = Raycast(new Vector2(direction * handOffset, -footHeight),
            Vector2.right * direction, groundDistance, groundLayer);
        if (!isOnGround && !isAttacking && !isDashing && !isHurting && canMove
            && headCheck && handCheck && footCheck && rb.velocity.y < 0f)
        {
            if (!isSliding)
            {
                StartCoroutine(CreateDust(0.3f));
                jumpCount = 1;
                isDoubleJumping = false;
                if (isGliding)
                {
                    isGliding = false;
                    rb.gravityScale = 1f;
                }
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
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void GroundMove()
    {
        if (isSliding || isDashing || !canMove || isHurting) return;
        if (isAttacking && isOnGround)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else rb.velocity = new Vector2(xVelocity * speed * Time.fixedDeltaTime, rb.velocity.y);
        FlipDirection();
    }

    private void FlipDirection()
    {
        float direction = rb.velocity.x == 0f ? 0f : (rb.velocity.x > 0 ? 1f : -1f);
        if (direction != 0)
        {
            transform.localScale = new Vector3(direction, 1, 1);
        }
        else
        {
            direction = xVelocity == 0f ? 0f : (xVelocity > 0 ? 1f : -1f);
            if(direction != 0)
                transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    private void AirMove()
    {
        if (isDashing || isClimbing || isHurting) return;
        if (isSliding)
        {
            if (jumpPressed)
            {
                WallJump();
            }
            return;
        }
        if (jumpPressed && jumpCount > 0)
        {
            if (doubleJump && jumpCount == 1)
            {
                isDoubleJumping = true;
            }
            Jump();
        }
        if (isDoubleJumping && glideHeld && rb.velocity.y < -1f)
        {
            isGliding = true;
            rb.gravityScale = glideGravity;
        }
        else
        {
            isGliding = false;
            if(rb.gravityScale != 0f) rb.gravityScale = 1f;
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
        AudioManager.GetInstance().PlayJumpAudio();
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        jumpCount--;
        jumpPressed = false;
    }

    private void WallJump()
    {
        float direction = transform.localScale.x;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(6f * -direction, jumpForce), ForceMode2D.Impulse);
        jumpCount--;
        jumpPressed = false;
        isSliding = false;
        FlipDirection();
        StartCoroutine(DisableMovement(0.15f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            if (avoidDamage) return;
            if (property.HaveEquipment("Helmet")) return;
            Hurt(1, (Vector2.up + new Vector2(-transform.localScale.x, 0)).normalized * 2f, GameManager.GroundTrap);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("OneWayPlatform") && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            if (yVelocity < -0.1f)
            {
                other.gameObject.GetComponent<PlatformEffector2D>().colliderMask ^= 1 << LayerMask.NameToLayer("Player");
                other.gameObject.layer = 0;
                StartCoroutine(ThroughPlatform(other.gameObject));
            }
        }
    }

    IEnumerator ThroughPlatform(GameObject platform)
    {
        yield return new WaitForSeconds(0.5f);
        platform.GetComponent<PlatformEffector2D>().colliderMask |= 1 << LayerMask.NameToLayer("Player");
        platform.layer = LayerMask.NameToLayer("Platform");
    }

    IEnumerator DisableMovement(float duration)
    {
        canMove = false;
        yield return new WaitForSeconds(duration);
        canMove = true;
    }

    IEnumerator CreateDust(float duration)
    {
        do
        {
            PoolManager.GetInstance().GetDustObject(false);
            yield return new WaitForSeconds(duration);
        }while (isSliding) ;
    }

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.5f);
        isHurting = false;
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        float a = 0f;
        float duration = 2f;
        float interval = 0.15f;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        while(duration >= 0 && avoidDamage)
        {
            yield return new WaitForSeconds(interval);
            Color color = sprite.color;
            sprite.color = new Color(color.r, color.g, color.b, a);
            a = -a + 1;
            duration -= interval;
        }
        Color color1 = sprite.color;
        sprite.color = new Color(color1.r, color1.g, color1.b, 1);
        avoidDamage = false;
    }
}
