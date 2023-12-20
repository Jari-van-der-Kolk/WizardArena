using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon
{

    public class Saxon
    {
        public static void GetPlayer(out GameObject playerObject)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        public static bool IsInDistance(Vector3 origin, Vector3 target, float inDistanceLength) 
        {
            return Vector3.Distance(origin, target) < inDistanceLength;
        }


        public static bool IsGrounded(Vector3 origin, float distance, LayerMask mask)
        {               
            // Cast a ray downward from the specified origin point
            Ray ray = new Ray(origin, Vector3.down);

            // Check if the ray hits something
            if (Physics.Raycast(ray, distance, mask))
            {
                // Ground hit detected
                return true;
            }

            // No ground hit
            return false;
        }

    }

}