using UnityEngine;
using Saxon.Sensor;
using Saxon.BT;

public class RotateTowardsCommand : Command
{
    private readonly Transform objectToRotate;
    private readonly Agent agent;
    private readonly float rotationSpeed;

    public RotateTowardsCommand(Transform objectToRotate, Agent agent, float rotationSpeed = 2f)
    {
        this.objectToRotate = objectToRotate;
        this.agent = agent;
        this.rotationSpeed = rotationSpeed;
    }

    public override void Enlisted()
    {
        base.Enlisted();
        agent.navMesh.updateRotation = false;
    }

    public override bool MoveNext()
    {
        if (isExecuted)
        {
            return false;
        }

        Vector3 direction = (agent.target.position - objectToRotate.position).normalized;
        if(direction == Vector3.zero)
        {
            return false;
        }
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        objectToRotate.rotation = Quaternion.Slerp(objectToRotate.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Mark the command as executed when the rotation is complete
        if (Quaternion.Angle(objectToRotate.rotation, targetRotation) <= 0.1f)
        {
            
            isExecuted = true;
        }

        //Debug.Log("rotating");

        return !isExecuted;
    }

    public override void Reset()
    {
        base.Reset();
        agent.navMesh.updateRotation = true;

    }


    public override object Current => null;
}
