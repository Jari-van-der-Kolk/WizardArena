using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnterUtility : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        // Get all IAreaEnter components on the other object
        IAreaEnter[] areaEnterComponents = other.GetComponents<IAreaEnter>();

        foreach (IAreaEnter areaEnter in areaEnterComponents)
        {
            // Call the OnEnterArea method for each IAreaEnter component and pass the entering object
            areaEnter.OnEnterArea(gameObject); // 'gameObject' refers to the trigger area's GameObject
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Get all IAreaEnter components on the other object
        IAreaEnter[] areaEnterComponents = other.GetComponents<IAreaEnter>();

        foreach (IAreaEnter areaEnter in areaEnterComponents)
        {
            // Call the OnExitArea method for each IAreaEnter component and pass the exiting object
            areaEnter.OnExitArea(gameObject); // 'gameObject' refers to the trigger area's GameObject
        }
    }

    private void OnTriggerStay(Collider other)
    {
        IAreaEnter[] areaEnterComponents = other.GetComponents<IAreaEnter>();

        foreach (IAreaEnter areaEnter in areaEnterComponents)
        {
            areaEnter.OnTriggerStayArea(gameObject);
        }
    }
}
