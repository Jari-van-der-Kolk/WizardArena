using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDetection")]
public class ObjectDetectionData : ScriptableObject
{
    [Tooltip("The amount of seconds until the agent continues to the next action when he has lost the target out of sight")]
    public float lostPlayerDuration = 10f;

    [Space]

    public float distance = 10f;
    public float angle = 30f;
    public float height = 1.0f;
    public Color meshColor = Color.blue;
    [Tooltip("Objects with this layer are able to block the field of view when in the way\n Objects which don't have this layer, and block the field of view, will still see and detect the objects that you're looking for")]
    public LayerMask occlusionLayers;
    public LayerMask detectionLayers;

}
