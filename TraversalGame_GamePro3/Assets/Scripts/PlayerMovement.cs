using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Animator noMoreJumpAnim;
    public Rigidbody2D RB;

    public float Speed = 5;
    public float jumpForce = 10;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    Vector2 movement;

    public float numberOfJumps;
    private float jumps;
    int buildIndex;

    private void Awake()
    {
        noMoreJumpAnim.enabled = false;
        jumps = numberOfJumps;

        RB = GetComponent<Rigidbody2D>();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        playerMovement();
    }

    private void playerMovement()
    {
        //Horizontal Movement
        RB.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Speed, RB.linearVelocity.y);

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            jumps--;
            RB.linearVelocity = new Vector2(RB.linearVelocity.x, jumpForce);

            //Play animation when out of jumps
            if (jumps <= 0 && !isGrounded())
            {
                noMoreJumpAnim.enabled = true;
                noMoreJumpAnim.Play("noMoreJumps");
            }
        }

        //Replenish jumps when on ground 
        if (isGrounded())
        {
            jumps = numberOfJumps - 1;
        }
    }
    public bool isGrounded()
    {
        //Boxcast for detecting when player is on ground
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else 
        {  
            return false; 
        }
    }
    //Makes BoxCast in Unity Editor visible
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Exit")
        {
            SceneManager.LoadScene(buildIndex + 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Hazard")
        {
            Destroy(gameObject);

            //Reloads Current Scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}


