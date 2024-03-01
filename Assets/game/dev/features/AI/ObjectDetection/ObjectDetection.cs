using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Saxon.Sensor
{

    public class ObjectDetection 
    {
        public ObjectDetectionData data { get; private set; }
        Transform _Tran;
        Collider[] _vieldOfViewColliders = new Collider[50];
        Collider[] _vicinityColliders = new Collider[50];
        Collider[] _targetColliders = new Collider[50];
        Mesh _mesh;
        int _count;
        float _scanTimer;
        float _lostTimer;

        internal Transform target { get; private set; }
        internal List<GameObject> detectedTargets {  get; private set; }
        internal List<GameObject> vicinityTargets {  get; private set; }
        public bool hasTargetInSight {  get; private set; }
        public bool noVisualsOnTarget { get; private set; }
        public bool targetRecentlyLost { get; private set; }
        public bool lostTarget {  get; private set; }


        public ObjectDetection(Transform transform, ObjectDetectionData data)
        {
            target = transform;
            this._Tran = transform;
            this.data = data;
            vicinityTargets = new List<GameObject>();
            lostTarget = true;
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
            _mesh = CreateFieldOfViewWedgeMesh();
        }


        public void DrawGizmo()
        {
            if (data == null)
            {
                Debug.LogError("ObjectDetection does not contain ObjectDetectionData " + _Tran.name);
                return;
            }

            if (_mesh)
            {
                Gizmos.color = data.meshColor;
                Gizmos.DrawMesh(_mesh, _Tran.position, _Tran.rotation);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_Tran.position, data.distance);
            for (int i = 0; i < _count; i++)
            {
                Gizmos.DrawSphere(_vieldOfViewColliders[i].transform.position, 0.2f);
            }



        }

#if UNITY_EDITOR
        public void DrawAttackRanges()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(_Tran.position, Vector3.up, data.closeRangeAttackDistance);

            Handles.color = Color.green;
            Handles.DrawWireDisc(_Tran.position, Vector3.up, data.midRangeAttackDistance);

            Handles.color = Color.yellow;
            Handles.DrawWireDisc(_Tran.position, Vector3.up, data.longRangeAttackDistance);

            Gizmos.DrawLine(_Tran.position + Vector3.down * data.distance, _Tran.position + Vector3.up * data.distance);
        }
#endif
        #endregion

        public void TimeStepUpdate(float timestep)
        {
            float time = Time.time;

            if (time - _scanTimer > timestep)
            {
                detectedTargets = Scan(_Tran.forward, out var inVicinity);
                vicinityTargets = inVicinity;


                var previousVisualState = hasTargetInSight;
                hasTargetInSight = detectedTargets.Count > 0;
                noVisualsOnTarget = detectedTargets.Count == 0;

     
                if (!hasTargetInSight && previousVisualState)
                {
                    targetRecentlyLost = true;
                    lostTarget = true;
                }

                if (hasTargetInSight)
                {
                    //make a priority target system in the future for this line of code 
                    target = detectedTargets[0].transform;
                    targetRecentlyLost = false;
                    lostTarget = false;
                    _lostTimer = time;
                }

                if (targetRecentlyLost && time - _lostTimer > data.lostPlayerDuration)
                {
                    targetRecentlyLost = false;
                }

                _scanTimer = time;

            }

        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void ToggleTargetRecentlyLost(bool onOff)
        {
            targetRecentlyLost = onOff;
        }

        public void ResetRecentlyLostTimer()
        {
            _scanTimer = Time.time;
        }


        public List<T> GetComponentsInArea<T>(float areaRadius) where T : Component
        {
            List<T> detectedObjects = new List<T>();
            int count = Physics.OverlapSphereNonAlloc(_Tran.position, areaRadius, _targetColliders, data.targetLayers, QueryTriggerInteraction.Collide);

            for (int i = 0; i < count; i++)
            {
                T obj = _targetColliders[i].GetComponent<T>();
                detectedObjects.Add(obj);
            }

            return detectedObjects;
        }

        public static List<T> GetComponentsInAreaNonAlocc<T>(Transform transform, float areaRadius, Collider[] colliders, LayerMask layer) where T : Component
        {
            List<T> detectedObjects = new List<T>();
            int count = Physics.OverlapSphereNonAlloc(transform.position, areaRadius, colliders, layer, QueryTriggerInteraction.Collide);

            for (int i = 0; i < count; i++)
            {
                T obj = colliders[i].GetComponent<T>();
                detectedObjects.Add(obj);
            }

            return detectedObjects;
        }

        public bool HasOcclusionWithTarget()
        {
            if (target == null)
            {
                return false;   
            }

            if (Physics.Linecast(_Tran.position, target.position, data.occlusionLayers))
            {
                // The target is occluded
                return true;
            }
            return false;
        }
      

        public bool HasObjectInSight(GameObject target)
        {
            return detectedTargets.Contains(target);
        }

        private List<GameObject> Scan(Vector3 scanDirection, out List<GameObject> gameObjects)
        {
            gameObjects = new List<GameObject>();
            List<GameObject> detectedObjects = new List<GameObject>();


            int vieldOfViewTargets = Physics.OverlapSphereNonAlloc(_Tran.position, data.distance, _vieldOfViewColliders, data.VieldOfViewLayers, QueryTriggerInteraction.Collide);
            for (int i = 0; i < vieldOfViewTargets; i++)
            {
                GameObject obj = _vieldOfViewColliders[i].gameObject;
                if (IsObjectInSight(data, _Tran.position, scanDirection, obj))
                {
                    detectedObjects.Add(obj);
                }
            }

            int vicinityCount = Physics.OverlapSphereNonAlloc(_Tran.position, data.distance, _vicinityColliders, data.targetLayers, QueryTriggerInteraction.Collide);
            for (int i = 0; i < vicinityCount; i++)
            {
                GameObject obj = _vicinityColliders[i].gameObject;
                gameObjects.Add(obj);

            }

            return detectedObjects;
        }


        static bool IsObjectInSight(ObjectDetectionData data, Vector3 origin, Vector3 lookDir, GameObject obj)
        {
            Vector3 dest = obj.transform.position;
            Vector3 dir = dest - origin;
            if (dir.y < 0 || dir.y > data.height)
            {
                return false;
            }

            dir.y = 0;
            float deltaAngle = Vector3.Angle(dir, lookDir);
            if (deltaAngle > data.angle)
            {
                return false;
            }

            origin.y += data.height / 2;
            dest.y = origin.y;
            if (Physics.Linecast(origin, dest, data.occlusionLayers))
            {
                return false;
            }

            return true;
        }

     
    }
}
