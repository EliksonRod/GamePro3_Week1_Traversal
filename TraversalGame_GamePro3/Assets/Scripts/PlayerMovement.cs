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

    public float numberOfJumps = 3;
    private float jumps;

    private void Awake()
    {
        noMoreJumpAnim.enabled = false;
        RB = GetComponent<Rigidbody2D>();
        jumps = numberOfJumps;
    }

    void Update()
    {
        playerMovement();
    }

    private void playerMovement()
    {
        //Horizontal Movement
        RB.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, RB.velocity.y);

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            jumps--;
            RB.velocity = new Vector2(RB.velocity.x, Speed);

            //Play animation when out of jumps
            if (jumps <= 0)
            {
                noMoreJumpAnim.enabled = true;
                noMoreJumpAnim.Play("noMoreJumps");
            }
        }

        //Replenish jumps when on ground 
        if (isGrounded())
        {
            jumps = numberOfJumps - 1;
            Debug.Log("On ground");
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If I walk into the exit. . .
        if (other.gameObject.CompareTag("Exit"))
        {
            //Win the game!
            SceneManager.LoadScene("You Win");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        /*if (other.gameObject.CompareTag("Cliff") && CurrentCliff == other.gameObject.GetComponent<CliffController>())
        {
            CurrentCliff = null;
        }*/
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //If I walk into a monster or other hazard. . .
        if (other.gameObject.CompareTag("Hazard"))
        {
            //Lose the game!
            SceneManager.LoadScene("You Lose");
        }
    }
}


