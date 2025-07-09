using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Animator noMoreJumpAnim;
    Rigidbody2D rb;

    public float currentSpeed;
    public float jumpForce = 10;
    [SerializeField]float normalSpeed = 8.2f;
    [SerializeField]float fastSpeed = 11.5f;
    bool isInvincible = false;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    Vector2 movement;

    public float numberOfJumps;
    private float jumps;
    int buildIndex;

    float backToNormalSpeedTimer = 0.1f;

    [Header("Direction")]
    private float movementInput;
    private bool facingRight = true;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Dash Settings")]
    public float dashForce = 70f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public int maxDashesPerJump = 1;
    public float dragDuringDash = 10f;

    //private int facingDirection = 1; // 1 = right, -1 = left
    public bool canDash = true;
    private bool isDashing = false;
    private float originalDrag;

    [Header("TrackedPlayerStats")]
    static public int jumpsUsed;
    static public int dashesUsed;


    public enum PlayerState
    {
        Normal,
        Dashing,
        Fast,
        Stunned
        
    }
    public PlayerState playerState;

    private void Awake()
    {
        playerState = PlayerState.Normal;
        noMoreJumpAnim.enabled = false;
        jumps = numberOfJumps;

        rb = GetComponent<Rigidbody2D>();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        originalDrag = rb.linearDamping;
    }

    void Update()
    {
        if (isDashing != true)
        PlayerControls();

        CharacterFlip();
    }

    void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                currentSpeed = normalSpeed;
                isInvincible = false;
                break;
            case PlayerState.Dashing:
                isInvincible = true;
                break;
            case PlayerState.Fast:
                currentSpeed = fastSpeed;
                isInvincible = false;
                break;
        }
    }

    private void PlayerControls()
    {
        movementInput = Input.GetAxisRaw("Horizontal");

        //Horizontal Movement
        rb.linearVelocity = new Vector2(movementInput * currentSpeed, rb.linearVelocity.y);

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            jumps--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            //Play animation when out of jumps
            if (jumps <= 0 && !isGrounded())
            {
                noMoreJumpAnim.enabled = true;
                noMoreJumpAnim.Play("noMoreJumps");
            }
        }

        //Dashing
        if (Input.GetKeyDown(dashKey) && canDash == true)
        {
            StartCoroutine(Dash());
        }

        //Replenish jumps when on ground 
        if (isGrounded())
        {
            //Offset with - 1 because the boxcast will detect still on ground sometime after jumping
            jumps = numberOfJumps - 1;
        }

        //Fast after dash
        if (playerState == PlayerState.Fast)
        {
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                StartCoroutine(switchToNormal());
                //playerState = PlayerState.Normal;
            }
        }
    }

    private void CharacterFlip()
    {
        if (facingRight && movementInput < 0f || !facingRight && movementInput > 0f)
        {
            Vector3 localScale = transform.localScale;
            facingRight = !facingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        playerState = PlayerState.Dashing;
        dashesUsed++;

        float initialGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float dashDirection = facingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection * dashForce, 0f);
        playerState = PlayerState.Fast;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = initialGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator switchToNormal()
    {
        yield return new WaitForSeconds(backToNormalSpeedTimer);

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            playerState = PlayerState.Normal;
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
    
    private void OnDrawGizmos()
    {
        //Makes BoxCast visible in Unity Editor
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
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


