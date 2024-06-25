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
    [SerializeField] private PawnType type;

    [SerializeField] private bool canMoveHorizontal;
    [SerializeField] private bool canMoveVertical;
    [SerializeField] private bool canMoveOverFriendlys;

    public static Pawn selectedInstance {  get; private set; }


    public static PawnType turn;    
    static int _teamOneAmount;
    static int _teamTwoAmount;
    static int _total;

    public static float startTimer;

    private void Start()
    {
        switch(type)
        {
            case PawnType.TeamOne:
                _teamOneAmount++;
                break;
            case PawnType.TeamTwo: 
                _teamTwoAmount++; 
          
                break;
        }


        _total++; 
    }

    private void OnTriggerEnter(Collider other)      
    {
        if(other.CompareTag(interactionTag))
        {
            other.gameObject.SetActive(false);
            _total--;
        }

    }

    private void OnMouseDown()
    {
        var previousInstance = selectedInstance;
        selectedInstance = this;        
        /*if(selectedInstance.type == turn)
        {
            selectedInstance = previousInstance;
        }*/

        Debug.Log(selectedInstance.transform.name);
    }

    public void CheckWinCondition()
    {
        if (_teamOneAmount == 0)
        {
            Debug.Log("Team Two wins!");
        }
        else if (_teamTwoAmount == 0)
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
        // Define the spline path
        Vector3[] splinePositions = {
        origin.transform.position, // Start at the pawn's current position
        new Vector3((origin.transform.position.x + dest.x) * 0.25f,origin.transform.position.y + height, (origin.transform.position.z + dest.z) * 0.25f), // Control point, adjust the height or position as needed
        new Vector3((origin.transform.position.x + dest.x) * 0.75f, origin.transform.position.y + height, (origin.transform.position.z + dest.z) * 0.75f), // Control point, adjust the height or position as needed
        dest // End at the destination
    };

        // Use LeanTween to move the pawn along the spline
        LeanTween.moveSpline(origin.gameObject, splinePositions, duration).setLoopOnce();

        SwitchTurn();
    }




    //selectedInstance.transform.position = pos;
    //Vector3 y0 = ((pos + dest) * .5f) + new Vector3(0f,height, 0f);
    /* {pos, y0 , dest}*/

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
