using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Collider2D standingCollider, crouchingCollider;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;

    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpPower = 500f;
    [SerializeField] int totalJumps;
    int availableJumps; 
    float horizontalValue;
    float runSpeedModifier = 2f;
    float crouchSpeedModifier = 0.5f;
    bool facingRight = true;
    bool isRunning;
    bool isGrounded;
    bool multipleJump;
    bool isCrouch;
    bool coyoteJump;
    bool isDead;

    public bool disabledByWin; 
    void Start()
    {
        availableJumps = totalJumps;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
       AudioManager.instance.PlayMusic("ambient");
    }
    void Update()
    {
        if(CanMove() == false)
        {
            horizontalValue = 0; 
            isRunning = false;
            isCrouch = false;
            return;
        }

        if(transform.position.y < -20)
        {
            Die();
        }
        horizontalValue = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonDown("Crouch"))
        {
            isCrouch = true;

        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouch = false;
        }
        animator.SetFloat("yVelocity", rb.velocity.y);

    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, isCrouch);
    }

    bool CanMove()
    {
        bool can = true;
        
        if(FindObjectOfType<InteractionSystem>().isExamining)
        {
            can = false;
        }
        if(FindObjectOfType<InventorySystem>().isOpen)
        {
            can = false;
        }
        if(isDead)
        {
            can = false;
        }
        if(disabledByWin)
        {
            can = false;
        }
        return can;
    }
    void GroundCheck()
    {
        bool wasGrounded = isGrounded; 
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0) 
        {
            isGrounded = true;
            if(!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;
            }

            foreach(var c in colliders)
            {
                if(c.tag == "MovingPlatform")
                {
                    transform.parent = c.transform;
                }
            }
        }
        else
        {
            transform.parent = null;
            if(wasGrounded)
            {
                StartCoroutine(CoyoteJumpDelay());
            }
        }

            animator.SetBool("Jump", !isGrounded);
    }

    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }
    void Jump()
    {
        if (isGrounded)
        {
            multipleJump = true;
            availableJumps--;

            rb.velocity = Vector2.up * jumpPower;
            animator.SetBool("Jump", true);
        }
        else
        {
            if(coyoteJump)
            {
                multipleJump = true;
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }
            if(multipleJump && availableJumps > 0)
            {
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }
        }
    }
    void Move(float dir, bool crouchFlag)
    {
        #region Crouch


        if(!crouchFlag)
        {
            if(Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
            {
                crouchFlag = true;
            }
        }

        animator.SetBool("Crouch", crouchFlag);
        standingCollider.enabled = !crouchFlag;
        crouchingCollider.enabled = crouchFlag;
        #endregion

        #region Move & Run
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        if (isRunning) 
        { 
            xVal *= runSpeedModifier;
        }
        if (isCrouch)
        {
            xVal *= crouchSpeedModifier;
        }
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;


        if(facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-5, 5, 5);
            facingRight = false;
        }
        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
            facingRight = true;
        }



        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion


    }

    public void Die()
    {
        isDead = true;
        FindObjectOfType<LevelManager>().Restart();

    }

}
