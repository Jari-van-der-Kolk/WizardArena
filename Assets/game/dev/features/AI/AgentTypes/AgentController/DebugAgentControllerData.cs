using UnityEngine;

namespace Saxon.BT.AI.Controller
{
    [System.Serializable]
    public class DebugAgentControllerData
    {
        [Header("Config")]
        [Space]
        public bool debug;
        public bool showDecisionRanges;
        [Header("Mutable")]
        public bool hasTargetInSight;
        public bool isTargetRecentlyLost;
        public bool lostTarget;
        public bool occlusion;
        public bool navmeshRotate;
        public Transform target;
    }

}