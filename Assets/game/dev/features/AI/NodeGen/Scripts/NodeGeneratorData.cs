using System.Collections.Generic;
using UnityEngine;
using Saxon.HashGrid;


namespace Saxon.NodePositioning
{
    [CreateAssetMenu(fileName = "NodeGeneratorData")]
    public class NodeGeneratorData : ScriptableObject
    {
        public List<Vector3> nodePointList;
    }

}

