using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
    public static class GameObjectExtensions {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component {
            T component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }

        // Extension method for getting components in an area around a Transform
        public static List<T> GetComponentsInArea<T>(this Transform transform, float areaRadius, LayerMask layerMask, Collider[] targetColliders) where T : Component
        {
            List<T> detectedObjects = new List<T>();

            // Use OverlapSphereNonAlloc to avoid garbage collection
            int count = Physics.OverlapSphereNonAlloc(transform.position, areaRadius, targetColliders, layerMask, QueryTriggerInteraction.Collide);

            for (int i = 0; i < count; i++)
            {
                T obj = targetColliders[i].GetComponent<T>();
                if (obj != null)
                {
                    detectedObjects.Add(obj);
                }
            }

            return detectedObjects;
        }
    }
    
}