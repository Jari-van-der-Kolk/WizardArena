using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FloatingObject : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider capsuleCollider;

    [SerializeField] private float rayDistance = 0.6f; // The distance to check for slopes
    [SerializeField] private float rideHeight;
    [SerializeField] private float floatSpringStrength;
    [SerializeField] private float floatSpringDamper;
    [SerializeField] private LayerMask groundMask;

    RaycastHit _groundCheckHit = new RaycastHit();

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        capsuleCollider = GetComponent<CapsuleCollider>();  
    }

    private void FixedUpdate()
    {
        Float();
    }


    private void Float()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        var rayCheckDistance = rayDistance + (capsuleCollider.height * .5f);

        if (Physics.Raycast(ray, out RaycastHit hit, rayCheckDistance, groundMask))
        {
            Vector3 vel = rb.velocity;
            Vector3 rayDir = transform.TransformDirection(ray.direction);


            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;
            if(hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = hit.distance - rideHeight;

            float springForce = (x * floatSpringStrength) - (relVel * floatSpringDamper);
            print(rayDir * springForce);

            Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.red);

            rb.AddForce(rayDir * springForce);

            if(hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * springForce, hit.point);
            }
        }
    }

}
