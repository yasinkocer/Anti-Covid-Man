using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    private float movementInputDirection;

    private int amountOfJumpsLeft;

    public int Puan;
    public int Hasar;
    public int Asi;

    public Text PuanSayisi;
    public Text AsiSayisi;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
    private bool Dustuu;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public float movementSpeed;
    public float jumpForce;
    public float groundCheckRadius;

    public Transform GroundCheck;

    public LayerMask whatisGround;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Hasar == 2 || Dustuu)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        PuanSayisi.text = "" + Puan;
        AsiSayisi.text = "" + Asi;

        if ( Puan == 158  && Asi==158)
        {
            SceneManager.LoadScene(1);
        }

    }


    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatisGround);
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <=0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
      
        if(amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection<0)
        {
            Flip();
        }
        else if(!isFacingRight&& movementInputDirection>0)
        {
            Flip();
        }

        if(rb.velocity.x !=0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocty", rb.velocity.y);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
        
    }
    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius);
    }
    private void OnTriggerEnter2D(Collider2D nesne)
    {
        if (nesne.gameObject.tag == "Puan")
        {
            Puan++;
           Destroy(nesne.gameObject);
        }

        if (nesne.gameObject.tag == "Asi")
        {
            Asi++;
            Destroy(nesne.gameObject);
        }



        if (nesne.gameObject.tag == "Tuzak")
        {

            Hit();
        }

        if (nesne.gameObject.tag == "Dustu")
        {
            Dustuu = true;
        }

    }

    private void Hit()
    {
        if (canJump)
        {
            Hasar++;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

}
