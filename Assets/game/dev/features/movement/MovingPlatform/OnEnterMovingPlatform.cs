using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnEnterMovingPlatform : MonoBehaviour, IAreaEnter
{
    private Transform originalParent; // Store the original parent

    CharacterController characterController;

    private Vector3 newPos;
    private Vector3 oldPos;

    bool skip;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalParent = transform.parent;
    }

    public void OnEnterArea(GameObject enteringObject)
    {
        newPos = Vector3.zero;
        oldPos = Vector3.zero;
        skip = true;
        print("Enter");

        //print("old: " + oldPos + " New: " + newPos);
    }

    public void OnExitArea(GameObject exitingObject)
    {
        newPos = Vector3.zero;
        oldPos = Vector3.zero;
        //transform.SetParent(null);  
    }

    void IAreaEnter.OnTriggerStayArea(GameObject utilityObject)
    {
        print("old: " + oldPos + " New: " + newPos);

        if(skip)
        {
            skip = false;
        }
        else
        {
            newPos = utilityObject.transform.position - oldPos;
        }

        characterController.SimpleMove(newPos);

        oldPos = utilityObject.transform.position;
    }

    public void MoveObjectUp(Transform targetObject)
    {
        if (targetObject != null)
        {
            int currentIndex = targetObject.GetSiblingIndex(); // Get the current sibling index
            int newIndex = Mathf.Clamp(currentIndex - 1, 0, targetObject.parent.childCount - 1); // Calculate the new index

            targetObject.SetSiblingIndex(newIndex); // Move the object up in the hierarchy
        }
        else
        {
            Debug.LogWarning("Target object is not assigned.");
        }
    }

    
}
