using UnityEngine;
using Saxon.Sensor;

public class RotateTowardsCommand : Command
{
    private readonly Transform objectToRotate;
    private readonly ObjectDetection detection;
    private readonly float rotationSpeed;

    public RotateTowardsCommand(Transform objectToRotate, ObjectDetection detection, float rotationSpeed = 2f)
    {
        this.objectToRotate = objectToRotate;
        this.detection = detection;
        this.rotationSpeed = rotationSpeed;
    }
   

    public override bool MoveNext()
    {
        if (isExecuted)
        {
            return false;
        }

        Vector3 direction = (detection.target.position - objectToRotate.position).normalized;
        if(direction == Vector3.zero)
        {
            return false;
        }
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        objectToRotate.rotation = Quaternion.Slerp(objectToRotate.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Mark the command as executed when the rotation is complete
        if (Quaternion.Angle(objectToRotate.rotation, targetRotation) <= 0.1f)
        {
            Debug.Log("rotated complete");
            isExecuted = true;
        }

        //Debug.Log("rotating");

        return !isExecuted;
    }


    public override object Current => null;
}
