using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetLayers", menuName = "Magic System/HitLayers/CustomtargetLayer")]
public class TargetLayerData : ScriptableObject
{
    public LayerMask targetedLayers;
}
