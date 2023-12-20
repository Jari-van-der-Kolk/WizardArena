using UnityEditor;
using UnityEngine;
using Saxon.NodePositioning;

[CustomEditor(typeof(NodeGenerator))]
public class NodeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NodeGenerator targetScript = (NodeGenerator)target;

        // Add a button to the inspector
        if (GUILayout.Button("Bake"))
        {
            // Call the method when the button is pressed
            targetScript.Bake();
        }

       
    }
}
