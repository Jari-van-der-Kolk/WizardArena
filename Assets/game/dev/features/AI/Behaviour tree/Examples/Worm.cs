using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saxon.BT;
using Saxon.Examples;

public class Worm : Actor
{
    [SerializeField] LayerMask mask;
    [SerializeField] private float radius;
    [SerializeField] private float losePlayerTime = 4f;

    private BehaviourTree tree;
    private SequenceNode lostPlayer;
    private FallbackStarNode root;
    private ReactiveSequenceNode followPlayer;
    private SequenceNode wander;
    private ReactiveSequenceNode trackPlayer;
    private ReactiveSequenceNode roam;
    private FunctionNode goToPlayer;
    private FunctionNode targetPlayer;
    private FunctionNode searchForLocation;
    private FunctionNode goToLocation;
    private FunctionNode inSight;
    private RandomDelayNode waitOnPosition;
    private InverterNode invertInSight;
    private InverterNode reachedLocation;
    private TimedRepeatNode followPlayerTime;


    private void Start()
    {
        CreateBT();

        transform.rotation = Quaternion.Euler(0, 0, 0);

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void CreateBT()
    {
        searchForLocation = new FunctionNode(SearchForRandomLocation);
        goToLocation = new FunctionNode(GoToLocation);
        waitOnPosition = new RandomDelayNode(.5f, 4f);
        goToPlayer = new FunctionNode(GoToPlayer);
        targetPlayer = new FunctionNode(TargetPlayer);
        inSight = new FunctionNode(HasPlayerInSight);
        invertInSight = new InverterNode(inSight);


        wander = new SequenceNode(new List<Node>
        {
            searchForLocation, goToLocation, waitOnPosition
        });

        roam = new ReactiveSequenceNode(new List<Node>
        {
            inSight, wander
        });

        followPlayer = new ReactiveSequenceNode(new List<Node>
        {
            invertInSight, goToPlayer  
        });

        reachedLocation = new InverterNode(goToLocation);
        followPlayerTime = new TimedRepeatNode(targetPlayer, losePlayerTime);

        lostPlayer = new SequenceNode(new List<Node>
        {
            followPlayerTime, goToLocation   
        });

        trackPlayer = new ReactiveSequenceNode(new List<Node>
        {
            inSight, reachedLocation, lostPlayer 
        });

        root = new FallbackStarNode(new List<Node>   
        {
            roam, followPlayer, trackPlayer 
        });

        //tree = new RootNode(root, agent);
    }

    void Update()
    {
        tree.TimeStepUpdate(.5f);
    }

    private void OnDrawGizmos()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Gizmos.DrawSphere(agent.destination, .75f);
    }
}


