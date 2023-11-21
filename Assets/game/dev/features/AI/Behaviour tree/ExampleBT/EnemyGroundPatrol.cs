using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundPatrol : OnHit
{
    
    [SerializeField] private float walkSpeed;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Vector2 knockback;

    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private Vector2 groundCheckSize;

    [SerializeField] private Collider2D groundCheck;

    [SerializeField] private LayerMask mask;


    [SerializeField] private bool isFlipped;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void Hit(float dot)
    {
        rb.velocity = Vector2.zero;

        if (dot > 0)
        {
            Debug.Log("a");
            rb.AddForce(new Vector2(knockback.x, knockback.y));
        }
        if (dot < 0)
        {
            Debug.Log("b");
            rb.AddForce(new Vector2(-knockback.x, knockback.y));
        }
        //health.ModifyHealth(-1);
    }

    
    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            Patrol();
            if (CheckForFlip())
            {
                Flip();
            }
        }
    }
    void Patrol()
    {
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    bool CheckForFlip()
    {
        if (IsGrounded())
            return Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundMask) ||
                   bodyCollider.IsTouchingLayers(groundMask);
        return false;
    }

    bool IsGrounded()
    {
        return groundCheck.IsTouchingLayers(mask);
    }
    public void Flip()
    {
        isFlipped = !isFlipped;
        sr.flipX = isFlipped;
        walkSpeed = -walkSpeed;
    }
}
