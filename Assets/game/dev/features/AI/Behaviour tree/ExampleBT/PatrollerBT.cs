using System;
using System.Collections;
using System.Collections.Generic;
using JBehaviourTree;
using UnityEditor;
using UnityEngine;

    public class PatrollerBT : OnHit
    {
        [SerializeField] private float patrolSpeed = 2.35f;
        [SerializeField] private Vector2 knockbackStrength;


        [Header("Patrol Ground check")]
        [SerializeField] private Vector2 ledgeCheckOffset;
        [SerializeField] private LayerMask groundMask;
        private Vector2 ledgeCheckOffsetDefault;

        [Header("GroundCheck")]
        [SerializeField] private Collider2D groundCheck;

        [SerializeField] private bool touchingGround;

        [Header("WallCheck")] 
        [SerializeField] private Collider2D wallCheck;
        
        [SerializeField]private bool isFlipped;
        
        private bool isHit;
        private FunctionNode knockback;
        private WaitNode wait;
        private GroundCheckNode hitGround;
        private FunctionNode partol;
        

        private SequenceNode patrolling;
        private RepeatNode root;
        private RootNode tree;


        private void Start()
        {
            ledgeCheckOffsetDefault = ledgeCheckOffset;
            
            partol = new FunctionNode(HitCheck);
            knockback = new FunctionNode(Knockback);
            hitGround = new GroundCheckNode(groundCheck, groundMask);
            wait = new WaitNode(.3f);

            patrolling = new SequenceNode(new List<Node>
            {
                partol,knockback, wait ,hitGround
            });
            
            
            root = new RepeatNode(patrolling);
            tree = new RootNode(root);
        }
        private void Update()
        {
            tree.Update();

            touchingGround = groundCheck.IsTouchingLayers(groundMask);
            
            
            if (rb.velocity.x >= 0)
            {
                isFlipped = false;
            }else if (rb.velocity.x < 0)
            {
                isFlipped = true;
            }
            
            if (!touchingGround)
            {
                //improve transform.position
                ledgeCheckOffset = Vector2.zero;
            }
            else
            {
                if (patrolSpeed > 0)
                {
                    ledgeCheckOffset = new Vector2(ledgeCheckOffsetDefault.x , ledgeCheckOffsetDefault.y);
                }

                if (patrolSpeed < 0)
                {
                    ledgeCheckOffset = new Vector2(-ledgeCheckOffsetDefault.x , ledgeCheckOffsetDefault.y);
                }
            }
        }

        private Node.State HitCheck()
        {
            if (isHit)
            {
                rb.velocity = Vector2.zero;
                return Node.State.Success;
            }
            Patrol();
            return Node.State.Failure;
        }
        
        private Node.State Knockback()
        {
            isHit = false;
            return Node.State.Success;
        }

        private void Patrol()
        {                            
            if (!Physics2D.OverlapCircle( transform.position + (Vector3)ledgeCheckOffset, .2f, groundMask) || wallCheck.IsTouchingLayers(groundMask))
            {
                patrolSpeed *= -1;
                ledgeCheckOffset = new Vector2(ledgeCheckOffset.x * -1, ledgeCheckOffset.y);
            }
            
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
        }

        public override void Hit(float dot)
        {
            isHit = true;
            //health.ModifyHealth(-1);
            if (dot > 0)
            {
                rb.velocity = Vector2.up;
                if (gameObject.activeSelf)
                {
                    StartCoroutine(ApplyKnockBack(.01f, true));
                }
            }
            if (dot <= 0)
            {
                rb.velocity = Vector2.up;
                if (gameObject.activeSelf)
                {
                    StartCoroutine(ApplyKnockBack(.01f, false));
                }
            }
        }

        IEnumerator ApplyKnockBack(float delay, bool rightOrLeft)
        {
           
            
            yield return new WaitForSeconds(delay);
            if (rightOrLeft)
            {
                EnemyFunctions.KnockBack(rb, new Vector2(knockbackStrength.x, knockbackStrength.y));
                yield break;
            }
            EnemyFunctions.KnockBack(rb, new Vector2(-knockbackStrength.x, knockbackStrength.y));
        }

        private void OnDrawGizmosSelected()
        {
            #if UNITY_EDITOR
            Handles.DrawWireDisc( transform.position + (Vector3)ledgeCheckOffset, Vector3.forward, .2f);
            #endif
        }
    }
