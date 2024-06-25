using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BoardGenerator : MonoBehaviour
{

    [SerializeField] private int gridIndexes = 5;
    [SerializeField] private float boardSpacing = 1f;
    [SerializeField] private GameObject prefab;

    [Range(0, 1)]
    [SerializeField] private float debugGridSize = .5f;



    private void Start()
    {
        for (int x = 0; x < gridIndexes; x++)
        {
            for (int z = 0; z < gridIndexes; z++)
            {
                float x0 = x * boardSpacing;
                float z0 = z * boardSpacing;
                var pos = new Vector3(x0, 0f, z0);
                Instantiate(prefab, transform.position + pos, Quaternion.identity);
            }
        }
    }


    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridIndexes; x++)
        {
            for(int z = 0; z < gridIndexes; z++)
            {
                float x0 = x * boardSpacing;
                float z0 = z * boardSpacing; 
                Gizmos.DrawSphere(transform.position + new Vector3(x0, 0, z0) , debugGridSize);
            }
        }
    }
}
