using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{

    [SerializeField] private float duration;
    [SerializeField] private float height;
    private void OnMouseDown()
    {
        Pawn.Move(Pawn.selectedInstance, transform.position, height, duration);
    }
}
