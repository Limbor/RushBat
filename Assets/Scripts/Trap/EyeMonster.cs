using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMonster : MonoBehaviour
{
    public float speed;
    public float alertArea;
    public Transform left;
    public Transform right;

    private Animator anim;
    private GameObject player;
    private int direction;
    private float leftRange;
    private float rightRange;
    private bool move;
    private bool onTheBorder;
    private bool isAttacking;
    private float lastAttackTime = -10f;
    private float coolDown = 5f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        move = true;
        onTheBorder = false;
        isAttacking = false;
        direction = 1;
        leftRange = left.position.x;
        rightRange = right.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking) return;
        if(Time.time > lastAttackTime + coolDown && player.transform.position.y < transform.position.y &&
            (player.transform.position.x - transform.position.x) * direction < alertArea &&
            (player.transform.position.x - transform.position.x) * direction >= 0)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("attack");
            isAttacking = true;
        }

        if (!onTheBorder && (transform.position.x > rightRange || transform.position.x < leftRange))
        {
            onTheBorder = true;
            move = false;
            StartCoroutine(Alert());
        }
        else if(transform.position.x < rightRange && transform.position.x > leftRange)
        {
            onTheBorder = false;
        }
        if (move)
        {
            transform.position = transform.position + Vector3.right * direction * speed * Time.deltaTime;
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    IEnumerator Alert()
    {
        yield return new WaitForSeconds(1f);
        direction = -direction;
        move = true;
    }

    public void End()
    {
        isAttacking = false;
    }

    public void CreateTear()
    {
        PoolManager.GetInstance().GetTearObject(transform.position);
    }
}
