using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[ExecuteInEditMode]
public class ObjectDetection : MonoBehaviour
{
    [SerializeField] private float distance = 10f;
    [SerializeField] private float angle = 30f;
    [SerializeField] private float height = 1.0f;
    [SerializeField] private Color meshColor = Color.blue;
    [SerializeField] private int scanFrequency = 30;
    [SerializeField] private LayerMask layers;


    [HideInInspector] public List<GameObject> detectedObjects = new List<GameObject>();

    
    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;


    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    protected void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);
        
        detectedObjects.Clear();
        for(int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsObjectInSight(obj))
            {
                detectedObjects[i] = obj;
            }
        }
    }

    protected bool IsObjectInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;


        return true;
    }

    private Mesh CreateFieldOfViewWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

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

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {

            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

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

    private void OnValidate()
    {
        mesh = CreateFieldOfViewWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in detectedObjects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
