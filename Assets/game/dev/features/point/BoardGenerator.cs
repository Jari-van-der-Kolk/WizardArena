using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BoardGenerator : MonoBehaviour
{

    [SerializeField] private int gridIndexes = 5;
    [SerializeField] private float boardUnit = 1f;
    [SerializeField] private GridSlot prefab;

    [Range(0, 1)]
    [SerializeField] private float debugGridSize = .5f;

    public static Action finishedGeneratingBoard;


    private void Awake()
    {
        for (int x = 0; x < gridIndexes; x++)
        {
            for (int z = 0; z < gridIndexes; z++)
            {
                float x0 = x * boardUnit;
                float z0 = z * boardUnit;
                var pos = new Vector3(x0, 0f, z0);
                var slot = Instantiate(prefab, transform.position + pos, Quaternion.identity);
                GridSlot.gridSlots.Add(slot);
            }
        }


    }

    private void Start()
    {
        finishedGeneratingBoard?.Invoke();
    }


    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridIndexes; x++)
        {
            for(int z = 0; z < gridIndexes; z++)
            {
                float x0 = x * boardUnit;
                float z0 = z * boardUnit; 
                Gizmos.DrawSphere(transform.position + new Vector3(x0, 0, z0) , debugGridSize);
            }
        }
    }
}
