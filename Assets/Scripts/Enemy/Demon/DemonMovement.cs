using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using UnityEngine;
using UnityEngine.UI;

public class DemonMovement : EnemyMovement
{
    public GameObject cat;
    public RectTransform bloodCanvas;
    public Transform leftBound;
    public Transform rightBound;
    
    private Image blood1;
    private Image blood2;
    private Image blood3;
    private Image blood4;
    private Image blood5;
    
    private DemonAttack attack;
    private bool move;
    private float targetPos;
    private bool moveAttacking;
    private float moveInterval;
    private float targetDirection;
    private float explodeBlood;
    
    private void OnEnable()
    {
        bloodCanvas.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        bloodCanvas.gameObject.SetActive(false);
    }
    private new void Start()
    {
        GetComponentInParent<Room>().RegisterEnemy(gameObject);
        
        isdead = false;
        hurt = false;
        hurttime = 0;
        canHurt = true;
        render = GetComponent<SpriteRenderer>();
        attack = GetComponent<DemonAttack>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameManager.GetInstance().GetPlayer();
        faceright = false;
        walkspeed = 2f;
        move = false;
        maxBlood = 1000f;
        blood = maxBlood;
        explodeBlood = 800f;
        moveInterval = 20f;

        blood1 = bloodCanvas.Find("Board/Blood1").GetComponent<Image>();
        blood2 = bloodCanvas.Find("Board/Blood2").GetComponent<Image>();
        blood3 = bloodCanvas.Find("Board/Blood3").GetComponent<Image>();
        blood4 = bloodCanvas.Find("Board/Blood4").GetComponent<Image>();
        blood5 = bloodCanvas.Find("Board/Blood5").GetComponent<Image>();
        blood1.fillAmount = 1;
        blood2.fillAmount = 1;
        blood3.fillAmount = 1;
        blood4.fillAmount = 1;
        blood5.fillAmount = 1;
    }

    private new void Update()
    {
        if(isdead) return;
        if (moveInterval > 0) moveInterval -= Time.deltaTime;
        Alive();
        Move();
        Attack();
        ChangeBlood();
    }

    private void Move()
    {
        float direction = faceright ? 1f : -1f;
        if (move && !attack.IsAttacking()) rb.velocity = direction * walkspeed * Vector2.right;
        else rb.velocity = Vector2.zero;
        if (!move && DistanceBetweenPlayer() < 1f && moveInterval <= 0)
        {
            move = true;
            targetPos = direction * 10f + transform.position.x;
            Mathf.Clamp(targetPos, leftBound.position.x, rightBound.position.x);
            targetDirection = direction;
        }
        if(move && !attack.IsAttacking() && direction != targetDirection)
        {
            Flip();
        }
        else if (move && (transform.position.x - targetPos) * direction > 0 && !attack.IsAttacking())
        {
            moveInterval = 20f;
            moveAttacking = false;
            move = false;
            Flip();
        }
    }

    private new void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        faceright = !faceright;
    }
    
    private void Attack()
    {
        if (!attack.IsAttacking())
        {
            if (explodeBlood > 0 && blood <= explodeBlood)
            {
                attack.StartExplode();
                explodeBlood -= 200f;
                return;
            }
            float distance = DistanceBetweenPlayer();
            if (!move)
            {
                if (distance > 8f)
                {
                    attack.StartLaser();
                }
                else if (distance > 3f)
                {
                    attack.StartSummon();
                }
                else
                {
                    attack.StartEnergy();
                }
            }
            else
            {
                if (!moveAttacking)
                {
                    moveAttacking = true;
                    StartCoroutine(MoveAttack());
                }
            }
        }
    }

    private IEnumerator MoveAttack()
    {
        while (moveAttacking)
        {
            yield return new WaitForSeconds(1f);
            if (!moveAttacking || isdead) break;
            if (DistanceBetweenPlayer() < 0)
            {
                // Debug.Log("attack flip");
                Flip();
            }
            attack.StartSummon();
            yield return new WaitForSeconds(3f);
        }
    }

    private float DistanceBetweenPlayer()
    {
        return (player.transform.position.x - transform.position.x) * (faceright ? 1f : -1f);
    }

    public bool IsDead()
    {
        return isdead;
    }
    
    public override void getDamage(float damage, int direction)
    {
        blood -= damage;
        if (!isdead)
        {
            if (hurttime <= 0)
            {
                hurttime = 0.1f;
            }
            PoolManager.GetInstance().GetDamageText(transform.position, damage);
        }
    }

    protected override void Alive()
    {
        if (blood <= 0)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("Recover", true);
            GetComponentInParent<Room>().DelEnemy(gameObject);
            bloodCanvas.gameObject.SetActive(false);
            isdead = true;
        }
        else
        {
            hurttime -= Time.deltaTime;
            if (hurttime > 0)
            {
                render.material.SetFloat("_FlashAmount", 1);
            }
            else
            {
                render.material.SetFloat("_FlashAmount", 0);
                hurt = false;
            }
        }
    }
    
    private void ChangeBlood()
    {
        blood5.fillAmount = blood > 800 ? ((blood - 800) / 200) : 0;
        blood4.fillAmount = blood > 300 ? ((blood - 600) / 200) : 0;
        blood3.fillAmount = blood > 200 ? ((blood - 400) / 200) : 0;
        blood2.fillAmount = blood > 100 ? ((blood - 400) / 200) : 0;
        blood1.fillAmount = blood > 0 ? (blood / 200) : 0;

    }

    private void Transform()
    {
        cat.transform.position = transform.position;
        cat.GetComponent<Collider2D>().enabled = false;
        cat.SetActive(true);
        gameObject.SetActive(false);
    }
}
