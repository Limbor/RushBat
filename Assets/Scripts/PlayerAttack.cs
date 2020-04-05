using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform damagePoint;
    public Transform energyPoint;
    public Transform dartPoint;
    public LayerMask enemyLayer;
    public GameObject energy;
    public GameObject dart;

    private float scope = 0.2f;
    private bool slowDown = false;
    private bool isSlashing = false;
    private float slowDownTime = 0.3f;
    private PlayerMovement player;
    private Rigidbody2D rb;

    private List<GameObject> damagesEnemies;
    private float pauseTime = 0f;
    private bool isTimeSlow = false;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, 0.35f);
    //}

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        damagesEnemies = new List<GameObject>();
    }

    private void Update()
    {
        if(slowDown && Mathf.Abs(rb.velocity.x) >= 0.1f)
        {
            rb.velocity = new Vector2(Mathf.Lerp(0f, rb.velocity.x, (slowDownTime-Time.deltaTime)/0.3f), 0);
        }
        else if(slowDown)
        {
            slowDown = false;
            slowDownTime = 0.3f;
        }

        if (isSlashing)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 0.35f, enemyLayer);
            foreach (Collider2D enemy in enemies)
            {
                if (!damagesEnemies.Contains(enemy.gameObject))
                {
                    pauseTime += 0.01f;
                    if (!isTimeSlow)
                    {
                        isTimeSlow = true;
                        Time.timeScale = 0.1f;
                        StartCoroutine(TimeStart());
                    }
                    damagesEnemies.Add(enemy.gameObject);
                    // TODO
                    Debug.Log("damage");
                }
            }
        }
    }

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(damagePoint.position, scope, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            // TODO
            Debug.Log("damage");
        }
    }

    public void Hadouken()
    {
        player.canMove = true;
        energy.transform.position = energyPoint.position;
        energy.SetActive(true);
    }

    public void Throw()
    {
        dart.transform.position = dartPoint.position;
        dart.SetActive(true);
    }

    public void Slash()
    {
        rb.velocity = new Vector2(player.transform.localScale.x * 8f, 0);
        isSlashing = true;
        PoolManager.GetInstance().GetDustObject(true);
        Camera.main.GetComponent<CameraController>().Shake();
        Camera.main.GetComponent<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
    }

    public void SlashEnd()
    {
        slowDown = true;
        isSlashing = false;
        damagesEnemies.Clear();
    }

    public void Recover()
    {
        player.canMove = true;
        player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }

    IEnumerator TimeStart()
    {
        yield return new WaitForSeconds(pauseTime);
        Time.timeScale = 1f;
        isTimeSlow = false;
    }
}
