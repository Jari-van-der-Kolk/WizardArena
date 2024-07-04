using Codice.CM.Common;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public enum PawnType
{
    TeamOne,
    TeamTwo,
}

public class Pawn : MonoBehaviour
{
    [Tag] [SerializeField] private string interactionTag;
    public PawnType type;

    [CurveRange(-1, -1, 1, 1, EColor.Red)]
    public AnimationCurve curve;

    public static Pawn selectedInstance {  get; private set; }
    public static PawnType turn;    

    public static int teamOneAmount;
    public static int teamTwoAmount;
    
    static int _total;

    

    public static float startTimer;

    public Pawn SetType(PawnType pawnType)
    {
        type = pawnType;
        return this;
    }

    public Pawn SetTag(string tag)
    {
        interactionTag = tag;
        return this;
    }


    private void Start()
    {
        _total++; 
    }
   

    private void OnMouseDown()
    {
        selectedInstance = this;
        print("foo");
    }

    public void CheckWinCondition()
    {
        if (teamOneAmount == 0)
        {
            Debug.Log("Team Two wins!");
        }
        else if (teamTwoAmount == 0)
        {
            Debug.Log("Team One wins!");
        }
        else if (_total == 0)
        {
            Debug.Log("Game Over: No pawns left!");
        }
    }

    public static void Move(Pawn origin, Vector3 dest, float height, float duration)
    {
        Vector3 startPosition = origin.transform.position;
        Vector3 endPosition = dest;
        Vector3 controlPoint = new Vector3((startPosition.x + endPosition.x) * 0.5f, height, (startPosition.z + endPosition.z) * 0.5f);

       /* if(CheckLaneForPawns(origin, dest, out var detectedPawn))
        {
            switch (detectedPawn.type)
            {
                case PawnType.TeamOne:
                    teamOneAmount--;
                    break; 
                case PawnType.TeamTwo:
                    teamTwoAmount--;
                    break;
            }
*/
            //detectedPawn.gameObject.SetActive(false);

            LeanTween.value(origin.gameObject, 0, 1, duration)
                .setOnUpdate((float t) => 
                {
                    Vector3 newPosition = CalculateQuadraticBezierPoint(t, startPosition, controlPoint, endPosition);
                    origin.transform.position = newPosition;
                })
                .setOnComplete(() =>
                {
                    // Call SwitchTurn() or any other logic after movement completes
                    SwitchTurn();
                });
        //}
    }

    public static bool CheckLaneForPawns(Pawn origin, Vector3 rhs, out Pawn detectedPawn)
    {
        Physics.Linecast(origin.transform.position, rhs, out var hit);
        detectedPawn = hit.collider.GetComponent<Pawn>();

        if (detectedPawn != null && detectedPawn.type != origin.type)
        {
            return true;
        }

        return false;

    }

    public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p0; // (1-t)^2 * p0
        point += 2 * u * t * p1; // 2(1-t)t * p1
        point += tt * p2;        // t^2 * p2

        return point;
    }

    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0; // (1-t)^3 * p0
        point += 3 * uu * t * p1; // 3(1-t)^2 t * p1
        point += 3 * u * tt * p2; // 3(1-t) t^2 * p2
        point += ttt * p3;        // t^3 * p3

        return point;
    }
  
    private static void SwitchTurn()
    {
        switch (turn)
        {
            case PawnType.TeamOne:
                turn = PawnType.TeamTwo;
                break;
            case PawnType.TeamTwo:
                turn = PawnType.TeamOne;
                break;
        }
    }




}
