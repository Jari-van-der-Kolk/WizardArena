using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.HashGrid;
using Unity.AI.Navigation;

//Later improvements for when revisiting this code

// shiet een grid van raycasts met hulp van de navmeshsurface bounds
// gebruik de y bounds als hoogte om de raycast naar beneden te laten schieten
// als de positie van de raycast geen navmesh raakt dan word daar geen DecisionNode geplaatst

//DecisionNodes zijn grid positions die een waarde hebben voor bij het besluiten van zijn  de AI agents volgende beslissingen

namespace Saxon.NodePositioning
{

    [ExecuteInEditMode]
    public class NodeGenerator : MonoBehaviour
    {
        [Header("Bake settings")]

        public LayerMask groundMask;
        [Range(1, 20)]public int hashGridSize = 5;
        public float nodeSize = 1f;


        private GameObject nodesHolder;
        public List<HashNode> hashNodes = new List<HashNode>();
        public SpatialHashGrid<HashNode> hashGrid { get; private set; }

        #region debug

    #if UNITY_EDITOR
        //put this variable here since its on the bottom anyway
        [Space] public bool debug = false;

        void OnDrawGizmos()
        {
           
            if (debug) {

                if (hashNodes == null)
                    return;

                DebugNodes();

                if (hashGrid == null)
                    return;

                hashGrid.Debug();
            }

            
        }

        private void DebugNodes()
        {
            for (int i = 0; i < hashNodes.Count; i++)
            {
                Gizmos.DrawCube(hashNodes[i].transform.position, Vector3.one * .2f);
                Gizmos.DrawLine(hashNodes[i].transform.position, hashNodes[i].transform.position + Vector3.down);

            }
        }

    #endif

        #endregion


        //this method is used as a button for giving the output data it creates for the hashNodes list
        //when the button is pressed NodeGeneratorEditor calls this method and executes it
        public void Bake()
        {
            nodesHolder = new GameObject("NodeHolder");
            hashNodes = GenerateGridNodes();
        }

        private static NodeGenerator instance;

        // Public property to access the singleton instance
        public static NodeGenerator Instance
        {
            get
            {
                // If the instance is null, try to find it in the scene
                if (instance == null)
                {
                    instance = FindObjectOfType<NodeGenerator>();

                    // If no instance is found, create a new GameObject and add the script to it
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject("NodeGeneratorSingleton");
                        instance = singletonObject.AddComponent<NodeGenerator>();
                    }
                }

                return instance;
            }
        }


        private void Start()
        {
            hashGrid = new SpatialHashGrid<HashNode>(hashNodes, hashGridSize);
        }

        List<HashNode> GenerateGridNodes()
        {
            var nodePointList = new List<HashNode>();


            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
    
            Bounds navMeshBounds = CalculateNavMeshBounds(navMeshData.vertices);
    
            float navMeshSizeX = Mathf.CeilToInt(navMeshBounds.size.x / nodeSize);
            float navMeshSizeY = Mathf.CeilToInt(navMeshBounds.size.y * 2f / nodeSize);
            float navMeshSizeZ = Mathf.CeilToInt(navMeshBounds.size.z / nodeSize);
    
            for (int x = 0; x < navMeshSizeX; x++)
            {
                for (int y = 0; y < navMeshSizeY; y++)
                {
                    for (int z = 0; z < navMeshSizeZ; z++)
                    {
                        Vector3 point = new Vector3(x * nodeSize, y * nodeSize, z * nodeSize) + navMeshBounds.min;
                        Vector3 pos = point + (Vector3.up * 1f);

                        // Make sure the point is on the NavMesh
                        if (IsPointOnNavMesh(point, navMeshData) && Saxon.IsGrounded(point, .75f, groundMask))
                        {
                            // Instantiate a cube or any other object at the generated point
                            //:p
                            var node = InstantiateNode(pos, new Vector3(x,y,z), nodesHolder);          
                            nodePointList.Add(node);   

                        }
                    }
                }
            }
            return nodePointList;
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
    
        HashNode InstantiateNode(Vector3 position, Vector3 ID, GameObject parent)
        {
            GameObject nodeObject = new GameObject("Node");
            var node = nodeObject.AddComponent<HashNode>();
            node.SetID(ID);

            nodeObject.transform.position = position;
            nodeObject.transform.SetParent(parent.transform);

            return node;
        }
    }

}
