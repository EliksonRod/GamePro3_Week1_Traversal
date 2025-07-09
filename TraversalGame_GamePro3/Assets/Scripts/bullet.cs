using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private LayerMask whatDestroysBullet;

    public float bulletSpeed = 15f;
    public float destroyTime = 5f;
    public float collisionDeathTime = 0.025f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        SetDestroyTime();

        //InitializeBullet();
    }

    void InitializeBullet()
    {
        rb.linearVelocity = transform.right * bulletSpeed;
    }

    void SetDestroyTime()
    {
        //destroy bullet after some time
        Destroy(gameObject, destroyTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //destroy bullet after hitting another collider
        Destroy(gameObject,collisionDeathTime);
    }

    private Vector3 direction;
    private float speed;

    public void Launch(float spd)
    {
        speed = spd;
    }

    void Update()
    {
        //transform.position += direction * speed * Time.deltaTime;
        rb.linearVelocity = transform.right * speed;
    }
}
