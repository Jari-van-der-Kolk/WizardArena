using UnityEngine;
using System.Collections;

public class BackAndForthAnimation : MonoBehaviour
{
    public Transform targetObject;
    public float moveDistance = 5.0f;
    public float moveDuration = 2.0f;

    void Start()
    {
        targetObject = transform;
        // Start the initial movement
        MoveObject();
    }

    void MoveObject()
    {
        // Move the object to the right
        LTDescr moveRight = LeanTween.moveX(targetObject.gameObject, targetObject.position.x + moveDistance, moveDuration);

        // When the first movement is complete, call the MoveBack function to move back to the original position
        moveRight.setOnComplete(MoveBack);
    }

    void MoveBack()
    {
        // Move the object back to its original position
        LTDescr moveLeft = LeanTween.moveX(targetObject.gameObject, targetObject.position.x - moveDistance, moveDuration);

        // When the movement back is complete, call MoveObject again to start moving to the right
        moveLeft.setOnComplete(MoveObject);
    }

   
}
