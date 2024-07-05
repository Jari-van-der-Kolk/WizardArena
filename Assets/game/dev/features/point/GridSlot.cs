using NaughtyAttributes;
using PlasticGui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GridSlot : MonoBehaviour
{
    [Tag][SerializeField] private string gridslotTag;
    public static GridSlot selectedInstance;

    [Header("Ball movement")]
    [SerializeField] private float duration;
    [SerializeField] private float height;

    [Space] 
    [SerializeField] private bool canMoveHorizontal;
    [SerializeField] private bool canMoveVertical;
    [SerializeField] private bool canMoveOverFriendlys;

    public Pawn occupiedPawn = null;

    public static List<GridSlot> gridSlots = new List<GridSlot>();

    private void OnMouseDown()
    {
        if (Pawn.selectedInstance.type != Pawn.turn)
            return;

        var previouseInstance = selectedInstance;
        selectedInstance = this;

        Pawn instance = Pawn.selectedInstance;
        if(selectedInstance.transform.CheckIncrementalAngle(instance.transform, out var hits))
        {
            for (int i = 0; i < hits.Length; i++)
            {
                print(hits[i].transform.name);
            }

            var opponents = CheckLaneForOpponents(hits);
            if (opponents.Count >= 2) return;

            if(opponents.Count > 0)
            { 
                opponents[0].gameObject.SetActive(false);
            }
            
            if(previouseInstance != null) previouseInstance.occupiedPawn = null;
          
            occupiedPawn = instance;

            Move();

        }

        


    }

    private List<Pawn> CheckLaneForOpponents(RaycastHit[] hits)
    {
        List<Pawn> pawnsAcrossLane = new List<Pawn>();
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.TryGetComponent<Pawn>(out var pawn))
            {
                if (pawn.transform == Pawn.selectedInstance.transform)
                    continue;
                    
                pawnsAcrossLane.Add(pawn);
            }
        }

        return pawnsAcrossLane;
    }

    private void Move()
    {
        Pawn.Move(Pawn.selectedInstance, transform.position, height, duration);
    }

 

    
}

/*    private List<GridSlot> CheckncrementalLanes()
    {
        List<GridSlot > gridslots = new List<GridSlot>();
        Ray ray;
        Vector3 dir;

        int numSegments = 8;
        float angleIncrement = 360f / numSegments;

        for (int i = 0; i < numSegments; i++)
        {
            // Calculate the current angle in degrees
            float currentAngle = i * angleIncrement;

            // Convert angle to radians for trigonometric functions
            float angleRad = currentAngle * Mathf.Deg2Rad;

            // Calculate the direction vector from the angle
            dir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f);

            // Optionally, round the direction vector and scale it
            dir = new Vector3(Mathf.Round(dir.x), Mathf.Round(dir.y), Mathf.Round(dir.z));
            dir *= angleIncrement;
            
            ray = new Ray(selectedInstance.transform.position, dir);

            

            // Draw a debug ray in the direction
            Debug.DrawRay(transform.position, transform.right * 5f, Color.red);
        
        }




        return gridslots;
    }*/

/*  // Convert the angle to radians
  float angleInRadians = Mathf.Deg2Rad * (i * angleIncrement);

  // Calculate the direction using trigonometry
  float x = Mathf.Cos(angleInRadians);
  float z = Mathf.Sin(angleInRadians);

  // Create the vector and assign to the array
  var vectorDirection = new Vector3(x, 0, z).normalized * distance;
  ray = new Ray(selectedInstance.transform.position, vectorDirection);

  Physics.Raycast(ray, out var hit);
  if (hit.collider.CompareTag(gridslotTag))
  {
      var slot = hit.collider.GetComponent<GridSlot>();
      if (slot != null)
      {
          gridslots.Add(slot);
      }
      else
      {
          Debug.LogError($"raycast could find a gridslot on:{slot} ");
      }
  }*/
  /*  private void RotateToAngle()
    {
        float angleAmount = 360 / 8;
        Vector3 dir = _faceMousePos.angle / angleAmount;
        dir = Mathf.RoundToInt(dir);
        dir *= angleAmount;
        transform.rotation = Quaternion.AngleAxis(dir, Vector3.forward);
        Debug.DrawRay(transform.position, transform.right * mineDistance, Color.red);

    }*/
    /*private float GetAngle(Vector3 dir)
    {
        // Ensure the direction vector is not zero
        if (dir == Vector3.zero)
        {
            Debug.LogError("Direction vector is zero, angle cannot be determined.");
            return 0f;
        }

        // Project the direction onto the horizontal plane
        Vector3 flatDir = new Vector3(dir.x, 0, dir.z).normalized;

        // Calculate the angle in degrees between the forward vector and the direction vector
        float angle = Vector3.SignedAngle(Vector3.forward, flatDir, Vector3.up);

        // Return the angle
        return angle;
    }*/