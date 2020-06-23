using System;
using UnityEngine;
public class Laser : MonoBehaviour
{
    private ParticleSystem particles;
    private GameObject laserHit;
    private PlayerMovement player;

    private Vector3 direction;
    private float rotateSpeed = 6f;

    private void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        laserHit = transform.parent.GetChild(1).gameObject;
        laserHit.SetActive(false);
    }

    private void OnEnable()
    {
        laserHit.SetActive(true);
    }

    private void Start()
    {
        if(player == null) player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.fixedDeltaTime, Space.Self);
        laserHit.transform.Rotate(Vector3.forward, rotateSpeed * Time.fixedDeltaTime, Space.Self);
        var angle = transform.localEulerAngles.z;
        var y = Mathf.Sin(angle / 180f * Mathf.PI);
        var x = Mathf.Cos(angle / 180f * Mathf.PI);
        var orientation = new Vector3(x, y, 0) * direction.x;
        var hit = Physics2D.Raycast(transform.position, orientation, float.MaxValue, 
            1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Player"));
        if (hit)
        {
            // Debug.DrawRay(transform.position, orientation * hit.distance);
            if (hit.collider.CompareTag("Player"))
            {
                player.Hurt(1, direction, GameManager.Enemy);
            }
            transform.localScale = new Vector3(hit.distance, 1, 1);
            var shape = particles.shape;
            shape.scale = new Vector3(hit.distance, shape.scale.y, 1);
            laserHit.transform.position = transform.position + orientation * (hit.distance - 0.2f);
        }
    }

    private void OnDisable()
    {
        transform.localRotation = Quaternion.identity;
        laserHit.transform.localRotation = Quaternion.identity;
        laserHit.transform.position = transform.position;
        laserHit.SetActive(false);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
}