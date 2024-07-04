using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawner : MonoBehaviour
{
    [SerializeField] private PawnType spawnType = PawnType.TeamOne;
    [Tag][SerializeField] private string gridslotTag;
    [SerializeField] private GameObject appearance;

    [SerializeField] private List<GridSlot> gridSlots = new List<GridSlot>();


    private void Awake()
    {
        BoardGenerator.finishedGeneratingBoard += () => Spawn();
    }

    private void Spawn()
    {
        var col = GetComponent<BoxCollider>();
        if (col == null)
        {
            Debug.LogError("BoxCollider is missing!");
            return;
        }

        // Calculate the bounds for the BoxCast
        Vector3 boxCenter = col.center;
        Vector3 boxHalfExtents = col.size * 0.5f;
        Quaternion boxOrientation = transform.rotation;
        Vector3 boxDirection = transform.forward; // Can be any direction you need
        float maxDistance = 0f; // Zero to only check for overlapping

        // Perform the BoxCast to detect objects
        RaycastHit[] hits = Physics.BoxCastAll(transform.TransformPoint(boxCenter), boxHalfExtents, boxDirection, boxOrientation, maxDistance);

        // Process the detected objects
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag(gridslotTag))
            {
                var pawn = Instantiate(appearance, hit.transform.position, Quaternion.identity);
                pawn.GetComponent<Pawn>().SetType(spawnType).SetType(spawnType);
                switch (spawnType)
                {
                    case PawnType.TeamOne:
                        Pawn.teamOneAmount++;
                        break;
                    case PawnType.TeamTwo:
                        Pawn.teamTwoAmount++;
                        break;
                }

                Debug.Log("Found object with tag: " + hit.transform.position);
            }
        }
    }
}
  



/*var pawns = Pawn.GetTeamList(spawnType);
    for (int i = 0; i < pawns.Count; i++)
    {
        
    }*/
    