using Saxon.BT.AI.Controller;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private ObjectDetectionData data;

    [Header("Debug")]
    [Space]
    [SerializeField] private bool showFieldOfView = false;
    [SerializeField] private bool showRanges = false;


    private Collider[] _vieldOfViewColliders = new Collider[50];
    private Collider[] _vicinityColliders = new Collider[50];
    private Collider[] _targetColliders = new Collider[50];
    private ObjectDetectionDebug _debug;
    private int _count;
    private float _lostTimer;

    public ObjectDetectionData Data
    {
        get { return data; }
        private set { } // Provide a body for the setter
    }

    internal Transform target { get; private set; }
    internal List<GameObject> detectedTargets { get; private set; }
    internal List<GameObject> vicinityTargets { get; private set; }
    public bool hasTargetInSight { get; private set; }
    public bool noVisualsOnTarget { get; private set; }
    public bool targetRecentlyLost { get; private set; }
    public bool lostTarget { get; private set; }

    //Shortcuts

    private void Awake()
    {
        detectedTargets = new List<GameObject>();
        vicinityTargets = new List<GameObject>();
        lostTarget = true;
        _debug = new ObjectDetectionDebug(transform, data, _vieldOfViewColliders); // Use transform
    }

    private void OnValidate()
    {
        _debug = new ObjectDetectionDebug(transform, data, _vieldOfViewColliders); // Use transform

    }

    private void Start()
    {
        target = transform; 
    }

    private void OnDrawGizmos()
    {
        if(!showFieldOfView)
        {
            return;
        }
        _debug.DrawGizmos(_count);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(!showRanges)
        {
            return;
        }
        _debug.DrawAttackRanges();
    }
#endif

    public void UpdataOD()
    {
        float time = Time.time;

        detectedTargets = Scan(transform.forward, out var inVicinity); // Use transform
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
            // Make a priority target system in the future for this line of code 
            target = detectedTargets[0].transform;
            targetRecentlyLost = false;
            lostTarget = false;
            _lostTimer = time;
        }

        if (targetRecentlyLost && time - _lostTimer > data.lostPlayerDuration)
        {
            targetRecentlyLost = false;
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

  
    public List<T> GetComponentsInArea<T>(float areaRadius) where T : Component
    {
        List<T> detectedObjects = new List<T>();
        int count = Physics.OverlapSphereNonAlloc(transform.position, areaRadius, _targetColliders, data.targetLayers, QueryTriggerInteraction.Collide); // Use transform

        for (int i = 0; i < count; i++)
        {
            T obj = _targetColliders[i].GetComponent<T>();
            detectedObjects.Add(obj);
        }

        return detectedObjects;
    }

    public static List<T> GetComponentsInAreaNonAlloc<T>(Transform transform, float areaRadius, Collider[] colliders, LayerMask layer) where T : Component
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

        if (Physics.Linecast(transform.position, target.position, data.occlusionLayers)) // Use transform
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

        int vieldOfViewTargets = Physics.OverlapSphereNonAlloc(transform.position, data.distance, _vieldOfViewColliders, data.VieldOfViewLayers, QueryTriggerInteraction.Collide); // Use transform
        for (int i = 0; i < vieldOfViewTargets; i++)
        {
            GameObject obj = _vieldOfViewColliders[i].gameObject;
            if (IsObjectInSight(data, transform.position, scanDirection, obj)) // Use transform
            {
                detectedObjects.Add(obj);
            }
        }

        int vicinityCount = Physics.OverlapSphereNonAlloc(transform.position, data.distance, _vicinityColliders, data.targetLayers, QueryTriggerInteraction.Collide); // Use transform
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
