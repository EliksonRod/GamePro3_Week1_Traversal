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
    public SpriteRenderer SR;
    public static PlayerMovement Player;

    public Rigidbody2D RB;
    public float Speed = 5;
    //public ProjectileController BulletPrefab;


    public CliffController CurrentCliff;

    public bool Climbing;


    private void Awake()
    {
        //The one thing I do that's a little fancy is
        //I record the player to a static variable
        //so they're easy to find
        Player = this;
        //SR = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (Climbing)
        {
            //ClimbMove();
        }
        else
        {
            NormalMove();
        }
        if (Climbing && CurrentCliff == null)
        {
            SetClimbing(false);
        }


    }


    public void SetClimbing(bool value)
    {
        if (value == Climbing) return;
        Climbing = value;
        if (Climbing)
        {
            SR.color = Color.yellow;
            AttachTo(CurrentCliff);
            RB.gravityScale = 0;
        }
        else
        {
            SR.color = Color.white;
            transform.parent = null;
            RB.gravityScale = 1;
        }
    }


    public void AttachTo(CliffController cliff)
    {
        CurrentCliff = cliff;
        transform.parent = cliff.transform;
        Vector3 pos = transform.position;
        pos.y = cliff.transform.position.y;
        transform.position = pos;
    }


    public void NormalMove()
    {
        //You've seen this movement code before
        Vector2 vel = RB.velocity;
        if (Input.GetKey(KeyCode.D))
            vel.x = Speed;
        else if (Input.GetKey(KeyCode.A))
            vel.x = -Speed;
        else
        {
            vel.x = 0;
        }
        if (Input.GetKeyDown(KeyCode.W))
            vel.y = Speed; RB.velocity = vel;
        if (Input.GetKeyDown(KeyCode.Space) && CurrentCliff != null)
        {
            SetClimbing(true);
        }
    }


    /*public void ClimbMove()
    {
        Vector2 vel = Vector2.zero;
        if (Input.GetKey(KeyCode.D))
            vel.x = Speed;
        else if (Input.GetKey(KeyCode.A))
            vel.x = -Speed;
        else
        {
            vel.x = 0;
        }
        RB.velocity = vel;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetClimbing(false);
        }
        if (Input.GetKeyDown(KeyCode.W) && CurrentCliff.AboveMe != null)
        {

            if (transform.position.x > CurrentCliff.AboveMe.Coll.bounds.min.x &&
               transform.position.x < CurrentCliff.AboveMe.Coll.bounds.max.x)
                AttachTo(CurrentCliff.AboveMe);
        }
        if (Input.GetKeyDown(KeyCode.S) && CurrentCliff.BelowMe != null)
        {
            if (transform.position.x > CurrentCliff.BelowMe.Coll.bounds.min.x &&
               transform.position.x < CurrentCliff.BelowMe.Coll.bounds.max.x)
                AttachTo(CurrentCliff.BelowMe);
        }
    }*/


    private void OnTriggerEnter2D(Collider2D other)
    {
        //If I walk into the exit. . .
        if (other.gameObject.CompareTag("Exit"))
        {
            //Win the game!
            SceneManager.LoadScene("You Win");
        }

        if (other.gameObject.CompareTag("Cliff"))
        {
            CurrentCliff = other.gameObject.GetComponent<CliffController>();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cliff") && CurrentCliff == other.gameObject.GetComponent<CliffController>())
        {
            CurrentCliff = null;
        }
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


