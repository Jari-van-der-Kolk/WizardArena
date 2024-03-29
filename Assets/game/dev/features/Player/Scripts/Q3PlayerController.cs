﻿using UnityEngine;
using UnityEngine.Rendering;

namespace Movement
{
    /// <summary>
    /// This script handles Quake III CPM(A) mod style player movement logic.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class Q3PlayerController : MonoBehaviour
    {
        [System.Serializable]
        public class MovementSettings
        {
            public float MaxSpeed;
            public float Acceleration;
            public float Deceleration;

            public MovementSettings(float maxSpeed, float accel, float decel)
            {
                MaxSpeed = maxSpeed;
                Acceleration = accel;
                Deceleration = decel;
            }
        }
        
        [Header("Aiming")]
        [SerializeField] private Camera m_Camera;
        [SerializeField] private MouseLook m_MouseLook = new MouseLook();

        [Header("Movement")]
        [SerializeField] private float m_Friction = 6;
        [SerializeField] private float m_Gravity = 20;
        [SerializeField] private float m_JumpForce = 8;
        [Tooltip("How precise air control is")]
        [SerializeField] private float m_AirControl = 0.3f;
        [Tooltip("Automatically jump when holding jump button")]
        [SerializeField] private bool m_AutoBunnyHop = false;
        [SerializeField] private MovementSettings m_GroundSettings = new MovementSettings(7, 14, 10);
        [SerializeField] private MovementSettings m_AirSettings = new MovementSettings(7, 2, 2);
        [SerializeField] private MovementSettings m_StrafeSettings = new MovementSettings(1, 50, 50);

        [Header("SlopeSettings")]
        [SerializeField] private LayerMask m_GroundMask;
        [SerializeField] private LayerMask m_MovingPlatformMask;

        /// <summary>
        /// Returns player's current speed.
        /// </summary>
        public float Speed { get { return m_Character.velocity.magnitude; } }
        public Vector3 axisVelocity { get { return m_PlayerVelocity; } }

        private CharacterController m_Character;
        private Vector3 m_MoveDirectionNorm = Vector3.zero;
        private Vector3 m_PlayerVelocity = Vector3.zero;

        // Used to queue the next jump just before hitting the ground.
        private bool m_JumpQueued = false;

        // Used to display real time friction values.
        private float m_PlayerFriction = 0;

        private Vector3 m_MoveInput;
        private Transform m_Tran;
        private Transform m_CamTran;

        private Vector3 lastPlatformPosition;


        private void Start()
        {
            m_Tran = transform;
            m_Character = GetComponent<CharacterController>();

            if (!m_Camera)
                m_Camera = Camera.main;

            m_CamTran = m_Camera.transform;
            m_MouseLook.Init(m_Tran, m_CamTran);
        }

        private void Update()
        {
            m_MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            m_MouseLook.UpdateCursorLock();

            QueueJump();


            // Check if the player is grounded before movement.
            if (m_Character.isGrounded)
            {
                var slopeHit = RayGroundCheck(out var slopeResult, m_GroundMask);

                GroundMove();

                if(OnSlopeCheck(m_Character.slopeLimit, slopeResult))
                {
                    print("t");
                    m_PlayerVelocity.y = -m_Gravity;
                    m_JumpQueued = false;
                }
               
                if (OnSlopeCheck(1,slopeResult) || slopeHit)
                {
                    m_PlayerVelocity.y = -m_Gravity;                                  
                }

                Jump();
            }
            else
            {

                // Player is not grounded, so handle air movement.
                AirMove();
            }

            if (RayGroundCheck(out var hit, m_MovingPlatformMask))
            {
                MovingPlatform(hit);
            }

            // Rotate the character and camera.
            m_MouseLook.LookRotation(m_Tran, m_CamTran);

            // Move the character.
            

            m_Character.Move(m_PlayerVelocity * Time.deltaTime);
        }

        private void MovingPlatform(RaycastHit movingPlatform)
        {
            if (movingPlatform.transform != null)
            {
                // Calculate the platform's velocity based on its previous position
                Vector3 platformVelocity = (movingPlatform.transform.position - lastPlatformPosition) / Time.deltaTime;

                // Add the moving platform's velocity to the player's position
                m_Character.Move(platformVelocity * Time.deltaTime);

                // Update the last platform position for the next frame
                lastPlatformPosition = movingPlatform.transform.position;
            }
        }

        #region slope

       
        private bool OnSlopeCheck(float maxAngle,RaycastHit hit)
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            if (angle > maxAngle)
            {
                return true;
            }
            return false;
        }

        
        #endregion

        #region Air
        // Queues the next jump.
        private void QueueJump()
        {
            if (m_AutoBunnyHop)
            {
                m_JumpQueued = Input.GetButton("Jump");
                return;
            }

            if (Input.GetButtonDown("Jump") && !m_JumpQueued)
            {
                m_JumpQueued = true;
            }

            if (Input.GetButtonUp("Jump"))
            {
                m_JumpQueued = false;
            }
        }

       


        // Handle air movement.
        private void AirMove()
        {
            float accel;

            var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
            wishdir = m_Tran.TransformDirection(wishdir);

            float wishspeed = wishdir.magnitude;
            wishspeed *= m_AirSettings.MaxSpeed;

            wishdir.Normalize();
            m_MoveDirectionNorm = wishdir;

            // CPM Air control.
            float wishspeed2 = wishspeed;
            if (Vector3.Dot(m_PlayerVelocity, wishdir) < 0)
            {
                accel = m_AirSettings.Deceleration;
            }
            else
            {
                accel = m_AirSettings.Acceleration;
            }

            // If the player is ONLY strafing left or right
            if (m_MoveInput.z == 0 && m_MoveInput.x != 0)
            {
                if (wishspeed > m_StrafeSettings.MaxSpeed)
                {
                    wishspeed = m_StrafeSettings.MaxSpeed;
                }

                accel = m_StrafeSettings.Acceleration;
            }

            Accelerate(wishdir, wishspeed, accel);
            if (m_AirControl > 0)
            {
                AirControl(wishdir, wishspeed2);
            }

            // Apply gravity
            m_PlayerVelocity.y -= m_Gravity * Time.deltaTime;
        }

        // Air control occurs when the player is in the air, it allows players to move side 
        // to side much faster rather than being 'sluggish' when it comes to cornering.
        private void AirControl(Vector3 targetDir, float targetSpeed)
        {
            // Only control air movement when moving forward or backward.
            if (Mathf.Abs(m_MoveInput.z) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)
            {
                return;
            }

            float zSpeed = m_PlayerVelocity.y;
            m_PlayerVelocity.y = 0;
            /* Next two lines are equivalent to idTech's VectorNormalize() */
            float speed = m_PlayerVelocity.magnitude;
            m_PlayerVelocity.Normalize();

            float dot = Vector3.Dot(m_PlayerVelocity, targetDir);
            float k = 32;
            k *= m_AirControl * dot * dot * Time.deltaTime;

            // Change direction while slowing down.
            if (dot > 0)
            {
                m_PlayerVelocity.x *= speed + targetDir.x * k;
                m_PlayerVelocity.y *= speed + targetDir.y * k;
                m_PlayerVelocity.z *= speed + targetDir.z * k;

                m_PlayerVelocity.Normalize();
                m_MoveDirectionNorm = m_PlayerVelocity;
            }

            m_PlayerVelocity.x *= speed;
            m_PlayerVelocity.y = zSpeed; // Note this line
            m_PlayerVelocity.z *= speed;
        }

        #endregion

        #region Surface

        // Handle ground movement.
        private void GroundMove()
        {
            // Do not apply friction if the player is queueing up the next jump
            if (!m_JumpQueued)
            {
                ApplyFriction(1.0f);
            }
            else
            {
                ApplyFriction(0);
            }

            var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
            wishdir = m_Tran.TransformDirection(wishdir);
            wishdir.Normalize();
            m_MoveDirectionNorm = wishdir;

            var wishspeed = wishdir.magnitude;
            wishspeed *= m_GroundSettings.MaxSpeed;

            
            Accelerate(wishdir, wishspeed, m_GroundSettings.Acceleration);

            // Reset the gravity velocity
            m_PlayerVelocity.y = -m_Gravity * Time.deltaTime;


           
        }

        private void Jump()
        {
            if (m_JumpQueued)
            {
                m_PlayerVelocity.y = m_JumpForce;
                m_JumpQueued = false;
            }
        }

        private void ApplyFriction(float t)
        {
            // Equivalent to VectorCopy();
            Vector3 vec = m_PlayerVelocity; 
            vec.y = 0;
            float speed = vec.magnitude;
            float drop = 0;

            // Only apply friction when grounded.
            if (m_Character.isGrounded)
            {
                float control = speed < m_GroundSettings.Deceleration ? m_GroundSettings.Deceleration : speed;
                drop = control * m_Friction * Time.deltaTime * t;
            }

            float newSpeed = speed - drop;
            m_PlayerFriction = newSpeed;
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }

            if (speed > 0)
            {
                newSpeed /= speed;
            }

            m_PlayerVelocity.x *= newSpeed;
            // playerVelocity.y *= newSpeed;
            m_PlayerVelocity.z *= newSpeed;
        }

        #endregion

        // Calculates acceleration based on desired speed and direction.
        private void Accelerate(Vector3 targetDir, float targetSpeed, float accel)
        {
            float currentspeed = Vector3.Dot(m_PlayerVelocity, targetDir);
            float addspeed = targetSpeed - currentspeed;
            if (addspeed <= 0)
            {
                return;
            }

            float accelspeed = accel * Time.deltaTime * targetSpeed;
            if (accelspeed > addspeed)
            {
                accelspeed = addspeed;
            }

            m_PlayerVelocity.x += accelspeed * targetDir.x;
            m_PlayerVelocity.z += accelspeed * targetDir.z;
        }

        private bool RayGroundCheck(out RaycastHit result, LayerMask layerMask)
        {
            Ray ray = new Ray(m_Tran.position, Vector3.down);
            var groundCheckDistance = 1f + (m_Character.height * .5f);
            Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundCheckDistance));
            if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, layerMask))
            {
                result = hit;
                return true;
            }
            result = new RaycastHit();
            return false;
        }

        private bool RayCheck(LayerMask layerMask)
        {
            Ray ray = new Ray(m_Tran.position, Vector3.down);
            var groundCheckDistance = .4f + (m_Character.height * .5f);
            if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, layerMask))
            {
                return true;
            }
            return false;
        }

    }
    /* private void foo(RaycastHit ground)
       {

           Vector3 groundNormal = ground.normal;

           // Calculate the movement direction based on input and ground normal
           Vector3 wishdir = Vector3.ProjectOnPlane(new Vector3(m_MoveInput.x, 0, m_MoveInput.z), groundNormal).normalized;
           m_MoveDirectionNorm = wishdir;

           // Calculate wishspeed based on the ground's slope
           var wishspeed = wishdir.magnitude;
           wishspeed *= m_GroundSettings.MaxSpeed;

           // Apply friction
           ApplyFriction(1.0f);

           // Apply acceleration
           Accelerate(wishdir, wishspeed, m_GroundSettings.Acceleration);

           // Reset the gravity velocity
           m_PlayerVelocity.y = -m_Gravity * .5f;


       }
*/

    /*    private void AdjustSlopeVelocity()
        {
            Vector3 vec = m_PlayerVelocity;
            Ray ray = new Ray(m_Tran.position, Vector3.down);
            var slopeCheckDistance = .4f + (m_Character.height * .5f);

            Debug.DrawLine(transform.position, transform.position + (Vector3.down * slopeCheckDistance));

            if (Physics.Raycast(ray, out RaycastHit hit, slopeCheckDistance, m_GroundMask))
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                if (angle > 0)
                {
                    Vector3 adjustedVelocity = Vector3.ProjectOnPlane(vec, hit.normal).normalized;
                    Debug.DrawLine(transform.position, transform.position + (adjustedVelocity.normalized * 5f));
                    
                    
                    vec = adjustedVelocity;
                    vec.Normalize();
                    //vec.y = -m_Gravity;
                }
            }

            m_PlayerVelocity += vec;
        }
*/

    /* private void AdjustSlopeVelocity()
      {
          Vector3 vec = m_PlayerVelocity;
          Ray ray = new Ray(m_Tran.position, Vector3.down);
          var slopeCheckDistance = .4f + (m_Character.height * .5f);

          Debug.DrawLine(transform.position, transform.position + (Vector3.down * slopeCheckDistance));

          if (Physics.Raycast(ray, out RaycastHit hit, slopeCheckDistance, m_GroundMask))
          {
              float angle = Vector3.Angle(Vector3.up, hit.normal);
              if (angle > 0)
              {
                  Vector3 adjustedVelocity = Vector3.ProjectOnPlane(vec, hit.normal).normalized;
                  Debug.DrawLine(transform.position, transform.position + (adjustedVelocity.normalized * 5f));


                  vec = adjustedVelocity;
                  vec.Normalize();
                  //vec.y = -m_Gravity;
              }
          }

          m_PlayerVelocity += vec;
      }*/

    /*var slopeDir = Vector3.ProjectOnPlane(m_MoveInput, hit.normal).normalized;
                        var newMag = slopeDir * m_GroundSettings.MaxSpeed;

                        float slopeSpeed = newMag.magnitude;

                        if(slopeSpeed > m_GroundSettings.MaxSpeed)
                        {
                            print("fpp");
                            m_PlayerVelocity = m_PlayerVelocity.normalized * m_GroundSettings.MaxSpeed;
                        }*/

}
