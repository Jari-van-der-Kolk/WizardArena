using UnityEditor;
using UnityEngine;

public class ObjectDetectionDebug
{
    private readonly Transform _transform;
    private readonly ObjectDetectionData _data;
    private Mesh _mesh;
    private Collider[] _vieldOfViewColliders;

    public ObjectDetectionDebug(Transform transform, ObjectDetectionData data, Collider[] vieldOfViewColliders)
    {
        _transform = transform;
        _data = data;
        _vieldOfViewColliders = vieldOfViewColliders;
        _mesh = CreateFieldOfViewWedgeMesh();
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
        Vector3 bottomLeft = Quaternion.Euler(0, -_data.angle, 0) * Vector3.forward * _data.distance;
        Vector3 bottomRight = Quaternion.Euler(0, _data.angle, 0) * Vector3.forward * _data.distance;

        Vector3 topCenter = bottomCenter + Vector3.up * _data.height;
        Vector3 topRight = bottomRight + Vector3.up * _data.height;
        Vector3 topLeft = bottomLeft + Vector3.up * _data.height;

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

        float currentAngle = -_data.angle;
        float deltaAngle = (_data.angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {

            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _data.distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * _data.distance;

            topRight = bottomRight + Vector3.up * _data.height;
            topLeft = bottomLeft + Vector3.up * _data.height;

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

    public void DrawGizmos(int count)
    {
        if (_data == null)
        {
            Debug.LogError("ObjectDetectionDebug does not contain ObjectDetectionData " + _transform.name);
            return;
        }

        if (_mesh)
        {
            Gizmos.color = _data.meshColor;
            Gizmos.DrawMesh(_mesh, _transform.position, _transform.rotation);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_transform.position, _data.distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(_vieldOfViewColliders[i].transform.position, 0.2f);
        }
    }

#if UNITY_EDITOR
    public void DrawAttackRanges()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(_transform.position, Vector3.up, _data.closeRangeAttackDistance);

        Handles.color = Color.green;
        Handles.DrawWireDisc(_transform.position, Vector3.up, _data.midRangeAttackDistance);

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(_transform.position, Vector3.up, _data.longRangeAttackDistance);

        Gizmos.DrawLine(_transform.position + Vector3.down * _data.distance, _transform.position + Vector3.up * _data.distance);
    }
#endif
}
