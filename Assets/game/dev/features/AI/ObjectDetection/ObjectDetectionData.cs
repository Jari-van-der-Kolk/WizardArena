using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDetection")]
public class ObjectDetectionData : ScriptableObject
{
    public float distance = 10f;
    public float angle = 30f;
    public float height = 1.0f;
    public Color meshColor = Color.blue;
    [Tooltip("Objects with this layer are able to block the field of view when in the way\n Objects which dont have this layer, and block the field of view, will still see and detect the objects that you're looking for")]
    public LayerMask occlusionLayers;
    public LayerMask detectionLayers;

}
