using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class Equipment : MonoBehaviour
{
    private bool pick = false;
    private float floatScope = 0.05f;
    private PlayerProperty player;
    private float pos;
    private float direction = 1f;
    
    public String equipmentName;
    public String description;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerProperty>();
        pos = transform.position.y;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (pick)
        {
            transform.position = player.transform.position + Vector3.up * 0.5f;
        }
        else
        {
            transform.Translate(0, direction * 0.1f * Time.deltaTime, 0);
            if (transform.position.y > pos + floatScope || transform.position.y < pos - floatScope)
            {
                direction = -direction;
            }
        }
    }
    void Pick()
    {
        pick = true;
        AudioManager.GetInstance().PlayPowerUpAudio();
        player.Equip(equipmentName);
        Destroy(gameObject, 1.5f);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!pick && other.CompareTag("Player"))
        {
            Pick();
        }
    }
}
