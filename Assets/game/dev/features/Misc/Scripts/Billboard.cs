using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        // Make the object face the camera
        transform.LookAt(Camera.main.transform);
        // Optionally, rotate the object by 180 degrees on the y-axis to face correctly
        transform.Rotate(0, 180, 0);
    }
}
