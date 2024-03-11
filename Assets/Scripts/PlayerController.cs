using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Object Values
    private Rigidbody2D rb;
    private CapsuleCollider2D col; 
    private Animator anim;

    private Vector2 inputVector, moveVector;
    private float yVel;
    public float gravity = 9.81f;
    public float speed = 5f;
    public float jumpVel = 9.81f;
    public float climbVel = 9.81f;
    public float groundCheckRadius = 0.1f;

    //Animations Boolean
    bool jumping, climbing;

    // Input Boolean
    private bool jumpPressed;


    // Check Vairables
    private Vector3 groundCheckA, groundCheckB,ceilingCheckA, ceilingCheckB;

    // Layer Masks
    public LayerMask groundLayers, enemyLayer, ceilingLayer;

    // Check Boolean
    bool grounded, squishEnemy, extraJump, ceiling;
    public bool laddered, wasLaddered;

    //Audio variables
    public AudioManager am;
    float sinceLastFootstep;
    float timeBetweenFootsteps = 0.5f;

    private SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        am = FindObjectOfType<AudioManager>();
        CalculateScales();
        Manager.lastCheckPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        GetInput();
        CalculateMovement();
        ControlAnimation();

        
    }
    void GetInput()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jumpPressed = Input.GetButtonDown("Jump");
    }

    void ControlAnimation()
    {
        if (!Manager.gamePaused)
        {
            if (inputVector.x != 0f)
            {
                anim.SetBool("run", true);

                if (inputVector.x > 0f)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;
                }
            }
            else
            {
                anim.SetBool("run", false);

            }
            anim.SetBool("jump", !grounded);
            anim.SetBool("climb", climbing);
            anim.SetFloat("yVelocity", rb.velocity.y);
        }
        
    }


    void CalculateMovement()
    {
        if (!Manager.gamePaused)
        {
            grounded = CheckCollision(groundCheckA, groundCheckB, groundLayers);
            ceiling = CheckCollision(ceilingCheckA, ceilingCheckB, ceilingLayer);
            

            //Jumping Logic
            if (jumpPressed)
            {
                print("jumpressed");
                jumpPressed = false;
                if (grounded)
                {
                    jumping = true;
                    yVel = jumpVel;
                    am.AudioTrigger(AudioManager.SoundFXCat.Jump, transform.position, 1f);
                }
                if (extraJump)
                {
                    extraJump = false;
                    jumping = true;
                    //wallStuck = false;

                    yVel = jumpVel;
                }

            }

            // Jumping on Enemies Logic
            if (!grounded && yVel < 0f)
            {
                squishEnemy = CheckCollision(groundCheckA, groundCheckB, enemyLayer);
                if (squishEnemy)
                {
                    extraJump = true;
                    jumping = true;

                    yVel = jumpVel * 0.5f;
                }
            }

            //Disables G's when on ground
            if (grounded && yVel <= 0f || ceiling && yVel > 0f)
            {
                if(grounded && jumping)
                {
                    am.AudioTrigger(AudioManager.SoundFXCat.HitGround, transform.position, 1f);
                }
                if(ceiling && jumping)
                {
                    am.AudioTrigger(AudioManager.SoundFXCat.HitCeiling, transform.position, 1f);
                }

                yVel = 0f;
                jumping = false;
            }
            else
            {
                yVel -= gravity * Time.deltaTime;

            }

            //Ladder Logic
            if (laddered && !wasLaddered)
            {
                if (inputVector.y != 0f)
                {
                    climbing = true;
                    wasLaddered = true;
                }

            }

            if (wasLaddered && !laddered)
            {
                climbing = false;
                wasLaddered = false;
            }

            if (climbing)
            {
                yVel = climbVel * inputVector.y;

            }

            // Velocity change
            moveVector.y = yVel;
            moveVector.x = inputVector.x * speed;

            sinceLastFootstep += Time.deltaTime;
            if(moveVector.x != 0f && grounded)
            {
                if(sinceLastFootstep > timeBetweenFootsteps)
                {
                    sinceLastFootstep = 0f;
                    am.AudioTrigger(AudioManager.SoundFXCat.FootStepConcrete, transform.position, 1f);
                }

            }
            if(moveVector.y != 0f && laddered)
            {
                if(sinceLastFootstep > timeBetweenFootsteps)
                {
                    sinceLastFootstep = 0f;
                    am.AudioTrigger(AudioManager.SoundFXCat.FootStepWood, transform.position, 1f);
                }
            }


        }
       
    }

    bool CheckCollision(Vector3 a, Vector3 b, LayerMask l)
    {
        Collider2D colA = Physics2D.OverlapCircle(transform.position - a, groundCheckRadius, l);
        Collider2D colB = Physics2D.OverlapCircle(transform.position - b, groundCheckRadius, l);

        if (colA)
        {
            if(l == enemyLayer && yVel < 0f)
            {
                colA.gameObject.GetComponent<EnemyHealthSystem>().RecieveHit(1);
            }
            return true;
        }
        else if (colB)
        {
            if (l == enemyLayer && yVel < 0f)
            {
                colB.gameObject.GetComponent<EnemyHealthSystem>().RecieveHit(1);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    void CalculateScales()
    {
        groundCheckA = -col.offset - new Vector2(col.size.x / 2f - (groundCheckRadius * 1.2f), -col.size.y / 2.22f);
        groundCheckB = -col.offset - new Vector2(-col.size.x / 2f + (groundCheckRadius * 1.2f), -col.size.y / 2.22f);

        ceilingCheckA = -col.offset - new Vector2(col.size.x / 2f - (groundCheckRadius * 1.2f), col.size.y / 2.2f);
        ceilingCheckB = -col.offset - new Vector2(-col.size.x / 2f + (groundCheckRadius * 1.2f), col.size.y / 2.2f);
    }



    private void FixedUpdate()
    {
        rb.velocity = moveVector;
    }

    private void OnDrawGizmos()
    {
        // General Ground Check gizmos
        Gizmos.DrawWireSphere(transform.position - groundCheckA, groundCheckRadius);
        Gizmos.DrawWireSphere(transform.position - groundCheckB, groundCheckRadius);

        // Standard ceiling check gizmos
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position - ceilingCheckA, groundCheckRadius);
        Gizmos.DrawWireSphere(transform.position - ceilingCheckB, groundCheckRadius);
    }


}
