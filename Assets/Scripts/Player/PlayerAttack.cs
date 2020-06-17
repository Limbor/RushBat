using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Damage Judgement")]
    public Transform damagePoint;
    public Transform energyPoint;
    public Transform dartPoint;
    public LayerMask enemyLayer;

    [Header("Damage Number")] 
    public float attackDamage, attackFloatRange;
    public float slashDamage, slashFloatRange;

    private GameObject energy;
    private GameObject bigDust;

    private float scope = 0.2f;
    private bool slowDown;
    private bool isSlashing;
    private float slowDownTime = 0.3f;
    private PlayerMovement player;
    private PlayerProperty property;
    private Rigidbody2D rb;

    private List<GameObject> damagesEnemies;
    private float pauseTime;
    private bool isTimeSlow;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        property = GetComponent<PlayerProperty>();
        rb = GetComponent<Rigidbody2D>();

        energy = PoolManager.GetInstance().transform.GetChild(0).gameObject;
        bigDust = PoolManager.GetInstance().transform.GetChild(1).gameObject;

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
                    float damage = slashDamage + Random.Range(-slashFloatRange, slashFloatRange);
                    Damage(enemy.gameObject, damage, transform.localScale.x);
                }
            }
        }
    }

    /// <summary>
    /// 对敌人造成伤害
    /// </summary>
    /// <param name="enemy">敌人对象</param>
    /// <param name="baseDamage">基础伤害</param>
    /// <param name="direction">伤害方向</param>
    /// <param name="extraDamage">额外伤害</param>
    public void Damage(GameObject enemy, float baseDamage, float direction, float extraDamage = 0)
    {
        var enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (!enemyMovement.canGetDamage()) return;
        int type = 1;
        baseDamage += property.GetAttack();
        if (property.HaveEquipment("SkullSword"))
        {
            baseDamage += property.GetCoinNumber() / 20 * 2;
        }
        if (property.HaveEquipment("WizardSword") && Random.Range(0, 1f) < 0.05f)
        {
            property.SetShield(1);
        }
        if (property.HaveEquipment("ThiefMask") && Random.Range(0, 1f) < 0.1f)
        {
            GameObject coin = Resources.Load<GameObject>("Prefabs/Item/SilverCoin");
            Instantiate(coin).GetComponent<Item>().Emit(enemy.transform.position, false);
        }
        if (property.HaveEquipment("SamuraiSoul") && Random.Range(0, 1f) < 0.3f)
        {
            baseDamage *= 2;
            type += 2;
        }
        if (extraDamage > 0) type++;
        if (property.HaveEquipment("ShadowBlade") && direction * enemy.transform.localScale.x > 0)
        {
            extraDamage += 10;
            if (type != 2 && type != 4) type++;
        }
        if (property.HaveEquipment("EnergyDrink"))
        {
            if (Random.Range(0, 1f) < 0.35f)
            {
                extraDamage += Random.Range(10, 51);
                if (type != 2 && type != 4) type++;
            }
        }
        if (property.HaveEquipment("BloodyFangs"))
        {
            if (!enemy.TryGetComponent(out BloodyFangs bloodyFangs))
            {
                bloodyFangs = enemy.AddComponent<BloodyFangs>();
            }
            bloodyFangs.AddHurtNumber();
            extraDamage += bloodyFangs.GetExtraDamage();
            if (extraDamage > 0 && type != 2 && type != 4) type++;
        }
        float damage = baseDamage + extraDamage;
        enemyMovement.getDamage(damage, (int)direction);
        PoolManager.GetInstance().GetDamageText(enemy.transform.position, damage, type);
    }
    
    public void Attack()
    {
        // transform.Translate(transform.localScale.x * 0.1f, 0f, 0f);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(damagePoint.position, scope, enemyLayer);
        if (enemies.Length > 0) AudioManager.GetInstance().PlayHitAudio();
        float extraDamage = 0;
        if (enemies.Length > 1 && property.HaveEquipment("SojinSpear")) extraDamage = enemies.Length * 10; 
        foreach (Collider2D enemy in enemies)
        {
            // var direction = (enemy.transform.position.x > transform.position.x) ? 1 : -1;
            float damage = attackDamage + Random.Range(-attackFloatRange, attackFloatRange);
            Damage(enemy.gameObject, damage, transform.localScale.x, extraDamage);
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
        PoolManager.GetInstance().GetDart(dartPoint.position);
    }

    public void Slash()
    {
        player.avoidDamage = true;
        rb.velocity = new Vector2(player.transform.localScale.x * 8f, 0);
        isSlashing = true;
        bigDust.SetActive(true);
        bigDust.transform.position = transform.position;
        bigDust.transform.localScale = transform.localScale;
        AudioManager.GetInstance().PlaySlashAudio();
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
        player.avoidDamage = false;
        player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        player.canMove = true;
    }

    IEnumerator TimeStart()
    {
        yield return new WaitForSeconds(pauseTime);
        Time.timeScale = 1f;
        isTimeSlow = false;
    }
}
