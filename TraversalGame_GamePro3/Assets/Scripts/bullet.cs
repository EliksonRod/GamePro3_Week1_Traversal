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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        SetDestroyTime();

        InitializeBullet();
    }

    private void InitializeBullet()
    {
        rb.linearVelocity = transform.right * bulletSpeed;
    }
      
    private void SetDestroyTime()
    {
    //destroy bullet after some time
        Destroy(gameObject, destroyTime);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
    //destroy bullet after hitting another collider
        Destroy(gameObject);
    }
}
