using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Saxon.Sensor
{

    public class ObjectDetection 
    {
        [SerializeField]private ObjectDetectionData data;
        public ObjectDetection(Transform transform ,ObjectDetectionData data)
        {
            this.m_Tran = transform;
            this.data = data;
        }
    
        Transform m_Tran;
        Collider[] colliders = new Collider[50];
        Mesh mesh;
        int count;
        float scanTimer;

        internal List<GameObject> detectedObjects;
    
        public void TimeStepUpdate(float timestep)
        {
            float time = Time.time;
            if (time - scanTimer > timestep)
            {
                detectedObjects = Scan(m_Tran.forward);
                scanTimer = time;
                
            }
        }
        public bool HasObjectInSight(GameObject target)
        {
            return detectedObjects.Contains(target);
        }
       
    
        public List<GameObject> Scan(Vector3 scanDirection)
        {
            List<GameObject> detectedObjects = new List<GameObject>();
            int count = Physics.OverlapSphereNonAlloc(m_Tran.position, data.distance, colliders, data.detectionLayers, QueryTriggerInteraction.Collide);
            
            detectedObjects.Clear();
            for(int i = 0; i < count; i++)
            {
                GameObject obj = colliders[i].gameObject;
                if (IsObjectInSight(data ,m_Tran.position ,m_Tran.forward, obj))
                {
                    detectedObjects.Add(obj);
                }
            }
    
            return detectedObjects;
        }

        public static List<T> Scan<T>(ObjectDetectionData data, Vector3 origin, Vector3 scanDir, Collider[] colliders) where T : Component
        {
            List<T> detectedObjects = new List<T>();
            int count = Physics.OverlapSphereNonAlloc(origin, data.distance, colliders, data.detectionLayers, QueryTriggerInteraction.Collide);

            detectedObjects.Clear();
            for (int i = 0; i < count; i++)
            {
                T objComponent = colliders[i].GetComponent<T>();
                if (objComponent != null && IsObjectInSight(data, origin, scanDir.normalized, objComponent.gameObject))
                {
                    detectedObjects.Add(objComponent);
                }
            }

            return detectedObjects;
        }
        public Vector3 GetRandomPositionInsideMesh()
        {
            if (mesh == null)
            {
                return Vector3.zero;
            }

            // Generate a random point within the mesh
            Vector3[] vertices = mesh.vertices;
            int randomVertexIndex = Random.Range(0, vertices.Length);
            Vector3 randomVertex = vertices[randomVertexIndex];

            // Transform the random vertex to world space
            Vector3 randomPosition = m_Tran.TransformPoint(randomVertex);

            return randomPosition;
        }


        static bool IsObjectInSight(ObjectDetectionData data ,Vector3 origin , Vector3 lookDir ,GameObject obj)
        {
            Vector3 dest = obj.transform.position;
            Vector3 dir = dest - origin;
            if(dir.y < 0 || dir.y > data.height) {
                return false;
            }
    
            dir.y = 0;
            float deltaAngle = Vector3.Angle(dir, lookDir);
            if(deltaAngle > data.angle) {
                return false;
            }
    
            origin.y += data.height / 2;
            dest.y = origin.y;
            if(Physics.Linecast(origin, dest, data.occlusionLayers)) {
                return false;
            }
    
            return true;
        }
    
        #region Debug
        Mesh CreateFieldOfViewWedgeMesh()
        {
            Mesh mesh = new Mesh();
    
            int segments = 10;
            int numTriangles = (segments * 4) + 2 + 2;
            int numVertices = numTriangles * 3;
    
            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];
    
            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -data.angle, 0) * Vector3.forward * data.distance;
            Vector3 bottomRight = Quaternion.Euler(0, data.angle, 0) * Vector3.forward * data.distance;
    
            Vector3 topCenter = bottomCenter + Vector3.up * data.height;
            Vector3 topRight = bottomRight + Vector3.up * data.height;
            Vector3 topLeft = bottomLeft + Vector3.up * data.height;
    
            int vert = 0;
    
            //left side 
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;
    
            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;
    
            //right side 
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;
    
            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;
    
            float currentAngle = -data.angle;
            float deltaAngle = (data.angle * 2) / segments;
    
            for (int i = 0; i < segments; i++)
            {
    
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * data.distance;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * data.distance;
    
                topRight = bottomRight + Vector3.up * data.height;
                topLeft = bottomLeft + Vector3.up * data.height;
    
                //far side 
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;
    
                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;
    
    
                //top 
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;
    
    
                //bottom
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;
    
                currentAngle += deltaAngle;
            }
    
            for (int i = 0; i < numVertices; i++)
            {
                triangles[i] = i;
            }
    
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
    
            return mesh;
        }
    
        public void Validate()
        {
            mesh = CreateFieldOfViewWedgeMesh();
        }
       
    
        public void Debug()
        {
            if(data == null)
            {
                return;
            }
    
            if (mesh)
            {
                Gizmos.color = data.meshColor;
                Gizmos.DrawMesh(mesh, m_Tran.position, m_Tran.rotation);
            }
    
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_Tran.position, data.distance);
            for (int i = 0; i < count; i++)
            {
                Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
            }
        }
        #endregion
    }
}
