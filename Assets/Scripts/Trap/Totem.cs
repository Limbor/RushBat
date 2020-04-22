using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    [SerializeField]
    private float lastShootTime;
    private Animator anim;
    private int nextShootHead;
    private Transform[] shootPoint;

    public float coolDown = 5f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nextShootHead = 0;
        shootPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lastShootTime + coolDown)
        {
            anim.SetTrigger("shoot");
            lastShootTime = Time.time;
        }
    }

    public void Shoot()
    {
        PoolManager.GetInstance().GetSpikeObject(shootPoint[nextShootHead + 1]);
        nextShootHead = (nextShootHead + 1) % 3;
    }
}
