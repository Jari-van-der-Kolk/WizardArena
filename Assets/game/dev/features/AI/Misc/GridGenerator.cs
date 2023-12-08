using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

namespace Saxon.AI.BehaviouralPositioning
{

    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private bool debug;

        public float nodeSize = 1f;

        public static List<Vector3> DebugPoints;

        [SerializeField] private LayerMask groundMask;

        private void Start()
        {
            DebugPoints = GenerateGridNodes(nodeSize);   
        }

        private void OnDrawGizmos()
        {
            if (debug)
            {

                if (DebugPoints == null )
                    return;


                for (int i = 0; i < DebugPoints.Count; i++)
                {
                    Gizmos.DrawCube(DebugPoints[i], Vector3.one * .2f);
                    Gizmos.DrawLine(DebugPoints[i], DebugPoints[i] + Vector3.down);

                }
            }
        }
      
        List<Vector3> GenerateGridNodes(float nodeSize)
        {
            var points = new List<Vector3>();
            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
    
            // Calculate the grid size based on the bounds of the NavMesh
            Bounds navMeshBounds = CalculateNavMeshBounds(navMeshData.vertices);
    
            float navMeshSizeX = Mathf.CeilToInt(navMeshBounds.size.x / nodeSize);
            float navMeshSizeY = Mathf.CeilToInt(navMeshBounds.size.y * 2f / nodeSize);
            float navMeshSizeZ = Mathf.CeilToInt(navMeshBounds.size.z / nodeSize);
    
            print(navMeshSizeY);
    
            for (int x = 0; x < navMeshSizeX; x++)
            {
                for (int y = 0; y < navMeshSizeY; y++)
                {
                    for (int z = 0; z < navMeshSizeZ; z++)
                    {
                        Vector3 point = new Vector3(x * nodeSize, y * nodeSize, z * nodeSize) + navMeshBounds.min;
                        Vector3 pos = point + (Vector3.up * 1f);

                        // Make sure the point is on the NavMesh
                        if (IsPointOnNavMesh(point, navMeshData) && IsGrounded(point, .75f, groundMask))
                        {
                            // Instantiate a cube or any other object at the generated point
                            //InstantiateCube(pos, Vector3.one * .5f);
                            points.Add(pos);
                        }
                    }
                }
            }
    
            return points;
        }
    
        public bool IsGrounded(Vector3 origin, float distance, LayerMask mask)
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
    
        Bounds CalculateNavMeshBounds(Vector3[] vertices)
        {
            Vector3 min = vertices[0];
            Vector3 max = vertices[0];
    
            for (int i = 1; i < vertices.Length; i++)
            {
                min = Vector3.Min(min, vertices[i]);
                max = Vector3.Max(max, vertices[i]);
            }
    
            return new Bounds((max + min) * 0.5f, max - min);
        }
    
        bool IsPointOnNavMesh(Vector3 point, NavMeshTriangulation navMeshData)
        {
            for (int i = 0; i < navMeshData.indices.Length; i += 3)
            {
                Vector3 v0 = navMeshData.vertices[navMeshData.indices[i]];
                Vector3 v1 = navMeshData.vertices[navMeshData.indices[i + 1]];
                Vector3 v2 = navMeshData.vertices[navMeshData.indices[i + 2]];
    
                if (PointInTriangle(point, v0, v1, v2))
                {
                    return true;
                }
            }
    
            return false;
        }
    
        bool PointInTriangle(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
        {
            float dX = point.x - v2.x;
            float dY = point.z - v2.z;
            float dX21 = v2.x - v1.x;
            float dY12 = v1.z - v2.z;
            float d = dY12 * (v0.x - v2.x) + dX21 * (v0.z - v2.z);
            float s = dY12 * dX + dX21 * dY;
            float t = (v2.z - v0.z) * dX + (v0.x - v2.x) * dY;
    
            if (d < 0)
                return s <= 0 && t <= 0 && s + t >= d;
            else
                return s >= 0 && t >= 0 && s + t <= d;
        }
    
        void InstantiateCube(Vector3 position, Vector3 size)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.transform.localScale = size;
            cube.transform.SetParent(transform);
        }
    }

}
