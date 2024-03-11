using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool intialDirectionRight = false;
    private float directionMulti = 1f;
    public float speed = 1f;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Vector3 groundCheckOffestA, groundCheckOffestB, frontPlayerCol, backPlayerCol;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayers, playerLayers, enemyLayer;
    public bool turning = false;
    private bool turiningWall = false;
    private SpriteRenderer sp;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        CheckScales();
        if (!intialDirectionRight)
        {
            directionMulti = -1;
            sp.flipX = true;

        }
        else
        {
            directionMulti = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(speed * directionMulti, 0, 0);
    }
    private void OnDrawGizmos()
    {
        // Ground collision check
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position - groundCheckOffestA, groundCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - groundCheckOffestB, groundCheckRadius);

        // Collision check for front 
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position - frontPlayerCol, groundCheckRadius);

        // Collision Check for the back
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - backPlayerCol, groundCheckRadius);
    }

    void CheckScales()
    {
        groundCheckOffestA = -col.offset - new Vector2(col.size.x / 2f - (groundCheckRadius * 1.2f), -col.size.y / 2f);
        groundCheckOffestB = -col.offset - new Vector2(-col.size.x / 2f + (groundCheckRadius * 1.2f), -col.size.y / 2f);

        frontPlayerCol = -col.offset - new Vector2(col.size.x / 2f, 0);
        backPlayerCol = -col.offset - new Vector2(-col.size.x / 2f, 0);
    }

    void CalculateMovement()
    {
        bool platformed = CheckEndOfPlatform(groundCheckOffestA, groundCheckOffestB, groundLayers);
        bool hitWall = CheckPlayerOrWallCol(frontPlayerCol, backPlayerCol, groundLayers);
        bool hitPlayer = CheckPlayerOrWallCol(frontPlayerCol, backPlayerCol, playerLayers);


        if (!platformed && !turning || hitWall && !turiningWall || hitPlayer && !turiningWall)
        {
            directionMulti *= -1;
            turning = true;
            turiningWall = true;
            sp.flipX = !sp.flipX;
        }
        if (platformed && turning)
        {
            turning = false;
        }
        if (!hitWall && turiningWall && !hitPlayer)
        {
            turiningWall = false;
        }
        if (hitPlayer)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = Manager.lastCheckPoint;
            Manager.AddLives(-1);

        }
    }

    bool CheckEndOfPlatform(Vector3 a, Vector3 b, LayerMask l)
    {
        if (Physics2D.OverlapCircle(transform.position - a, groundCheckRadius, l) &&
            Physics2D.OverlapCircle(transform.position - b, groundCheckRadius, l))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    bool CheckPlayerOrWallCol(Vector3 a, Vector3 b, LayerMask l)
    {
        if (Physics2D.OverlapCircle(transform.position - a, groundCheckRadius, l) ||
            Physics2D.OverlapCircle(transform.position - b, groundCheckRadius, l))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
