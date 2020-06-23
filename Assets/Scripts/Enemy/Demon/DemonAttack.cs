 using System;
 using System.Collections;
 using DG.Tweening;
 using UnityEngine;

public class DemonAttack : MonoBehaviour
{
    private const string Summon = "Summon";
    private const string Laser = "Laser";
    private const string Energy = "Energy";
    private const string Explode = "Explode";
    
    private Animator anim;
    private DemonMovement movement;
    private PlayerMovement player;
    private GameObject laser;
    private GameObject fireBall;
    private GameObject explode;
    private GameObject warning;
    
    private bool isSummon;
    private bool isLaser;
    private bool isExplode;
    private bool isEnergy;

    private float laserInterval;
    private float summonInterval;
    private float energyInterval;

    private void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<DemonMovement>();
        player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerMovement>();
        laser = transform.GetChild(0).gameObject;
        laser.SetActive(false);
        fireBall = Resources.Load<GameObject>("Prefabs/FX/BigFireBall");
        explode = Resources.Load<GameObject>("Prefabs/FX/Explode");
        warning = Resources.Load<GameObject>("Prefabs/FX/Warning");
        laserInterval = 0;
        summonInterval = 0;
        energyInterval = 0;
    }

    private void Update()
    {
        if (laserInterval > 0) laserInterval -= Time.deltaTime;
        if (summonInterval > 0) summonInterval -= Time.deltaTime;
        if (energyInterval > 0) energyInterval -= Time.deltaTime;
    }

    public bool IsAttacking()
    {
        return isEnergy || isExplode || isLaser || isSummon;
    }

    public void StartExplode()
    {
        isExplode = true;
        anim.SetBool(Explode, isExplode);
        transform.DOMove(transform.position + Vector3.up, 1f).OnComplete(() =>
        {
            StartCoroutine(Explosion());
        });
    }

    private IEnumerator Explosion()
    {
        for (int i = 0; i < 20; i++)
        {
            var pos = player.transform.position;
            Instantiate(warning, pos, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            Instantiate(explode, pos, Quaternion.identity);
        }
        EndExplode();
    }

    private void EndExplode()
    {
        anim.SetBool(Explode, false);
        transform.DOMove(transform.position + Vector3.down, 1f);
    }
    
    public void StartSummon()
    {
        if (summonInterval > 0) return;
        summonInterval = 2f;
        isSummon = true;
        anim.SetBool(Summon, isSummon);
    }

    private void SummonFireBall()
    {
        float x = transform.position.x - transform.localScale.x * 0.8f;
        float y = transform.position.y + 0.3f;
        for (int i = 0; i < 3; i++)
        {
            var go = Instantiate(fireBall, new Vector3(x, y - 0.5f * i, 0), Quaternion.identity);
            go.transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
        }
    }

    private void EndSummon()
    {
        isSummon = false;
        anim.SetBool(Summon, isSummon);
    }

    public void StartLaser()
    {
        if (laserInterval > 0) return;
        laserInterval = 10f;
        isLaser = true;
        anim.SetBool(Laser, isLaser);
    }

    private void EmitLaser()
    {
        Vector3 direction = Vector2.left * transform.localScale.x;
        laser.SetActive(true);
        laser.GetComponent<Laser>().SetDirection(direction);
        StartCoroutine(Emit());
    }

    private IEnumerator Emit()
    {
        yield return new WaitForSeconds(5f);
        laser.SetActive(false);
        anim.SetBool(Laser, false);
    }
    
    public void StartEnergy()
    {
        if (energyInterval > 0) return;
        energyInterval = 12f;
        isEnergy = true;
        anim.SetBool(Energy, isEnergy);
    }

    private void EnergyBegin()
    {
        StartCoroutine(FallStone());
    }
    
    private IEnumerator FallStone()
    {
        for (int i = 0; i < 5; i++)
        {
            var go = (GameObject)Instantiate(Resources.Load("Prefabs/FX/FallStone"));
            go.transform.position = new Vector3(player.transform.position.x, transform.position.y - 0.35f, 0);
            yield return new WaitForSeconds(1f);
        }
        anim.SetBool(Energy, false);
    }

    private void SkillStiff()
    {
        isEnergy = false;
        isLaser = false;
        isExplode = false;
        isSummon = false;
    }
}