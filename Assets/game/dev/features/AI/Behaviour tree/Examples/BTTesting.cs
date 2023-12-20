using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saxon.BT;
using Saxon.Examples; 

public class BTTesting : Actor
{
    private RootNode root;
    private FallbackStarNode fallbackNode;
    private DebugLogNode debugLogNode1;
    private DebugLogNode debugLogNode2;
    private DebugLogNode debugLogNode3;
    private FunctionNode functionNode;

    bool foo;

    void Start()
    {
        debugLogNode1 = new DebugLogNode(" 1"); 
        debugLogNode2 = new DebugLogNode(" 2");
        debugLogNode3 = new DebugLogNode(" ffffffffffff");
        functionNode = new FunctionNode(ReturnFailure);
        WaitNode panda = new WaitNode(.5f);
        RepeatChildNode repeatNode = new RepeatChildNode(debugLogNode1, 5f);
        InverterNode inverterNode = new InverterNode(repeatNode);


        fallbackNode = new FallbackStarNode (new List<Node>
        {
             inverterNode, debugLogNode2, debugLogNode3
        });

        //root = new RootNode(fallbackNode, new UnityEngine.AI.NavMeshAgent());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foo = !foo;
            functionNode = new FunctionNode(ReturnFailure);
        }

        root.TimeStepUpdate(.5f);
    }

    private Node.State ReturnFailure()
    {
        Debug.Log("f");
        if(foo)
        {
            return Node.State.Success;
        }
        return Node.State.Failure;
    }
}
