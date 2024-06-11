using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
    public static class GameObjectExtensions {
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
    }
    
}