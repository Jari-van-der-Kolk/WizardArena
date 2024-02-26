using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.HashGrid;
using Unity.AI.Navigation;
using static UnityEditor.PlayerSettings;

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
        #region Singleton
        private static NodeGenerator instance;

        public static NodeGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<NodeGenerator>();

                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject("NodeGeneratorSingleton");
                        instance = singletonObject.AddComponent<NodeGenerator>();
                    }
                }

                return instance;
            }
        }

        #endregion

        [Header("Bake settings")]

        public LayerMask groundMask;
        [Range(1, 20)]public int hashGridSize = 5;
        [SerializeField] float nodeSize = 1f;
        [SerializeField] float nodeGroundSeperation = 0.75f;

        private GameObject navigationNodesHolder;
        private GameObject keyNodesHolder;
        public List<HashNode> navigationNodes = new List<HashNode>();
        public List<OriginHashNode> keyNodes = new List<OriginHashNode>();
        public SpatialHashGrid<HashNode> hashGrid { get; private set; }

        #region debug

    #if UNITY_EDITOR
        //put this variable here since its on the bottom anyway
        [Space] public bool debugPoints = false;
         public bool debugHashgrid = false;

        void OnDrawGizmos()
        {
            if (debugPoints) {

                if (navigationNodes == null)
                    return;

                DebugNodes();
            }

            if(debugHashgrid)
            {
                if (hashGrid == null)
                    return;

                hashGrid.Debug();
            }
            
        }

        private void DebugNodes()
        {
            for (int i = 0; i < navigationNodes.Count; i++)
            {
                Gizmos.DrawCube(navigationNodes[i].transform.position, Vector3.one * .2f);
                //Gizmos.DrawLine(hashNodes[i].transform.position, hashNodes[i].transform.position + Vector3.down);

            }
        }

#endif

        #endregion


        private void OnValidate()
        {
            if(navigationNodes != null && navigationNodes.Count > 0)
            {
                hashGrid = new SpatialHashGrid<HashNode>(hashGridSize);
                AddValues(navigationNodes);
                AddValues(keyNodes);
            }
        }
        private void Start()
        {
            if (navigationNodes != null && navigationNodes.Count > 0)
            {
                hashGrid = new SpatialHashGrid<HashNode>(hashGridSize);
                AddValues(navigationNodes);
                AddValues(keyNodes);
            }
        }

     

        //this method is used as a button for giving the output data it creates for the hashNodes list
        //when the button is pressed NodeGeneratorEditor calls this method and executes it
        public void Bake()
        {
            hashGrid = new SpatialHashGrid<HashNode>(hashGridSize);

            navigationNodesHolder = new GameObject("Navigation Nodes Holder");
            keyNodesHolder = new GameObject("Key Nodes Holder");
            
            navigationNodes = GenerateNavigationNodes();
            AddValues(navigationNodes);

            keyNodes = GenerateKeyNodes();
            AddValues(keyNodes);

        }

     
        public void AddValues<T>(List<T> values) where T : HashNode
        {
            for (int i = 0; i < values.Count; i++)
            {
                hashGrid.Add(values[i], values[i].transform.position);
            }
        }

        List<HashNode> GenerateNavigationNodes()
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
                        if (Saxon.IsGrounded(point, nodeGroundSeperation, groundMask) && IsPointOnNavMesh(point, navMeshData))
                        {
                            // Instantiate a cube or any other object at the generated point
                            var ID = hashGrid.GetGridKey(pos);
                            var node = InstantiateNode<HashNode>(pos, ID, navigationNodesHolder);          
                            nodePointList.Add(node);   

                        }
                    }
                }
            }
            return nodePointList;
        }

        public List<OriginHashNode> GenerateKeyNodes()
        {
            var keyNodes = new List<OriginHashNode>();
            foreach (Vector3Int position in hashGrid.grid.Keys)
            {
                var cellCenter = hashGrid.GetCellCenter(position, hashGridSize);
                var node = InstantiateNode<OriginHashNode>(cellCenter, position, keyNodesHolder);  
                keyNodes.Add(node);
            }
            return keyNodes;
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
    
        T InstantiateNode<T>(Vector3 position, Vector3Int ID, GameObject parent) where T : HashNode
        {
            GameObject nodeObject = new GameObject("Node");
            var node = nodeObject.AddComponent<T>();
            node.SetID(ID);

            nodeObject.transform.position = position;
            nodeObject.transform.SetParent(parent.transform);

            return node;
        }
    }

}
