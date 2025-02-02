using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


//Double jump, Wall jump

public class PlayerMovement : MonoBehaviour
{
    public Animator noMoreJumpAnim;
    //public static PlayerMovement Player;

    public Rigidbody2D RB;
    public float Speed = 5;
    public float jumpForce = 10;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;


    public float numberOfJumps = 3;
    private float jumps;

    private void Awake()
    {
        noMoreJumpAnim.enabled = false;
        //Player = this;
        jumps = numberOfJumps;
    }

    void Update()
    {
        NormalMove();
    }

    public void NormalMove()
    {
        Vector2 vel = RB.velocity;
        if (Input.GetKey(KeyCode.D))
            vel.x = Speed;
        else if (Input.GetKey(KeyCode.A))
            vel.x = -Speed;
        else
        {
            vel.x = 0;
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && jumps > 0)
        {
            jumps--;
            //vel.y = Speed;
            vel.y = jumpForce;
            RB.velocity = vel;
            if(jumps <= 0)
            {
                noMoreJumpAnim.enabled = true;
                noMoreJumpAnim.Play("noMoreJumps");
                //RB.gravityScale = 1;
            }
        }
        if (isGrounded())
        {
            jumps = numberOfJumps - 1;
        }
    }
    public bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else 
        {  
            return false; 
        }

    }
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


