using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal movement")]
    [SerializeField] Rigidbody r_body;    
    [SerializeField] [Range(0, 0.7f)] float f_horizontalDampening;
    [SerializeField] float f_horizontalInput;
    [SerializeField] float f_inputDirection;

    [Header("Vertical movement")]
    [SerializeField] float f_jumpForce = 5f;
    [SerializeField] [Range(0, 5)] float f_cutJumpHeight = 0.5f;
    [SerializeField] Transform t_groundCheckPoint;
    [SerializeField] float f_groundCheckRadius = 0.5f;
    [SerializeField] LayerMask l_whatIsGround;
    [SerializeField] bool b_Grounded, b_canJump;

    [Header("Wall Jump")]
    [SerializeField] Transform t_leftWallCheckPoint;
    [SerializeField] Transform t_rightWallCheckPoint;
    [SerializeField] LayerMask l_whatIsWall;
    [SerializeField] float f_wallFallSpeed = 2f;
    [SerializeField] Vector2 v_wallClimb;
    [SerializeField] Vector2 v_wallJumpOff;
    [SerializeField] Vector2 v_wallLeap;
    [SerializeField] float f_wallhangTime = 0.25f;
    [SerializeField] int i_wallDirection;    
    [SerializeField] float f_MaximumSafefallSpeed;
    float f_wallhangCounter, m_fallSpeed;
    bool b_tounchingAWall, b_touchingWallOnTheLeft, b_touchingWallOnTheRight, b_wallsliding;

    public bool b_dead;

    private void Awake()
    {
        b_dead = false; 
        r_body = GetComponent<Rigidbody>();
    }

    void Update()
    {        
        if(!b_dead)
        {
            Movement();
            Jump();
            WallSlide();
        }
        
               
    }

    void Die()
    {
        //Break body activate ui
    }
       
    
    void WallSlide()
    {
        f_inputDirection = Input.GetAxisRaw("Horizontal");
        b_touchingWallOnTheLeft = Physics.CheckSphere(t_leftWallCheckPoint.position, f_groundCheckRadius, l_whatIsWall);
        b_touchingWallOnTheRight = Physics.CheckSphere(t_rightWallCheckPoint.position, f_groundCheckRadius, l_whatIsWall);
        b_tounchingAWall = b_touchingWallOnTheLeft || b_touchingWallOnTheRight;
        b_wallsliding = b_tounchingAWall && !b_Grounded && !Input.GetButton("Jump");
        if (b_wallsliding)
        {           
            r_body.velocity = new Vector3(r_body.velocity.x, -f_wallFallSpeed, 0f);
            if(f_wallhangCounter > 0)
            {
                r_body.velocity = new Vector3(0, r_body.velocity.y, 0);
                if (f_inputDirection != i_wallDirection && f_inputDirection != 0)
                {
                    f_wallhangCounter -= Time.deltaTime;
                }
                else
                {
                    f_wallhangCounter = f_wallhangTime;
                }
            }
            else
            {
                f_wallhangCounter = f_wallhangTime;
            }
        }
        if(b_tounchingAWall)
        {
            i_wallDirection = (b_touchingWallOnTheLeft) ? -1 : 1;           
        }
        if (!b_wallsliding)
        {
            f_wallhangCounter = f_wallhangTime;
        }
    }
        
    void Movement()
    {
        f_horizontalInput = r_body.velocity.x;
        f_horizontalInput += Input.GetAxisRaw("Horizontal");
        f_horizontalInput *= Mathf.Pow(1f - f_horizontalDampening, Time.deltaTime * 10f);
        r_body.velocity = new Vector3(f_horizontalInput, r_body.velocity.y, 0f);         
        
    }

    void Jump()
    {
        b_Grounded = Physics.CheckSphere(t_groundCheckPoint.position, f_groundCheckRadius, l_whatIsGround);
        b_canJump = b_Grounded || b_wallsliding;
        if (Input.GetButtonDown("Jump") && b_canJump)
        {
            r_body.velocity = new Vector3(r_body.velocity.x, 0, 0);
            if (b_wallsliding)
            {
                if(i_wallDirection == f_inputDirection)
                {
                    r_body.velocity = new Vector3(-i_wallDirection* v_wallClimb.x, v_wallClimb.y, 0);
                }
                else if(f_inputDirection == 0)
                {
                    r_body.velocity = new Vector3(-i_wallDirection * v_wallJumpOff.x, v_wallJumpOff.y, 0);
                }
                else
                {
                    r_body.velocity = new Vector3(-i_wallDirection * v_wallLeap.x, v_wallLeap.y, 0);
                }
            }
            else if(b_Grounded)
            {
                r_body.velocity = new Vector3(r_body.velocity.x, f_jumpForce, 0);
            }            
        }
        if(Input.GetButtonUp("Jump"))
        {
            if(r_body.velocity.y > 0)
            {
                r_body.velocity = new Vector3(r_body.velocity.x, r_body.velocity.y * f_cutJumpHeight);
            }
        }

        
        if (r_body.velocity.y <= f_MaximumSafefallSpeed)
        {
            m_fallSpeed = r_body.velocity.y;
        }
    }
}
