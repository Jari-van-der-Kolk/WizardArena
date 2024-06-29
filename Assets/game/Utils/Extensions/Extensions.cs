using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
    public static class Extensions 
    {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component {
            T component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }

        // Extension method for getting components in an area around a Transform
        public static List<T> GetComponentsInArea<T>(this Transform transform, float areaRadius) where T : Component
        {
            List<T> detectedObjects = new List<T>();

            // Use OverlapSphereNonAlloc to avoid garbage collection
            Collider[] hits = Physics.OverlapSphere(transform.position, areaRadius);

            for (int i = 0; i < hits.Length; i++)
            {
                T obj = hits[i].GetComponent<T>();
                if (obj != null)
                {
                    detectedObjects.Add(obj);
                }
            }

            return detectedObjects;
        }
        public static T Click<T>(this Transform origin, float checkDistance) where T : Component
        {
            Ray ray = new Ray(origin.position, origin.forward);
            if (Physics.Raycast(ray, out var hitInfo, checkDistance))
            {
                if (hitInfo.collider.TryGetComponent<T>(out var component))
                {
                    // Do something with the component, or call a method on it
                    // For example, let's just log its name here
                    Debug.Log($"Component found: {component.name}");
                    return component;
                }
            }
            return null;
        }

        public static void DebugType<T>(this Transform origin, object obj) where T : Component
        {
            Debug.Log(obj.ToString());
        }

        public static bool CheckIncrementalAngle(this Transform transform, Transform target , int numSegments = 8)
        {

            Ray ray;
            Vector3 dir;

            float distance = Vector3.Distance(transform.position, target.position); 

            float angleIncrement = 360f / numSegments;

            for (int i = 0; i < numSegments; i++)
            {
                // Calculate the current angle in degrees
                float currentAngle = i * angleIncrement;

                // Convert angle to radians for trigonometric functions
                float angleRad = currentAngle * Mathf.Deg2Rad;

                // Calculate the direction vector from the angle
                dir = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad));

                // Optionally, round the direction vector and scale it
                dir = new Vector3(Mathf.Round(dir.x), Mathf.Round(dir.y), Mathf.Round(dir.z));

                ray = new Ray(transform.position, dir);

                Physics.Raycast(ray, out var hit, distance);

                if(target == hit.transform)
                {
                    return true;
                }

                // Set the rotation of the object to the direction

                // Draw a debug ray in the direction
                Debug.DrawRay(transform.position, transform.up * 5f, Color.red);
                return true;

            }

            return false; 

        }


    }

}