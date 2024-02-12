using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.Sensor;
using Saxon.BT.AI.Controller;
using Saxon.NodePositioning;
using UnityEngine.UIElements;
using Saxon.BT;

namespace Saxon.BT.AI
{
    public class NecroServant : Agent
    {
        public NecroServant(AgentController agent) : base(agent) { }

        public override AgentTypes agentType { get { return AgentTypes.NecroServant; } protected set { } }

        public override BehaviourTree CreateTree()
        {



            FallbackNode fallback = new FallbackNode(new List<Node>
            {
                
            });

            RootNode rootNode = new RootNode(fallback);

            return new BehaviourTree(rootNode);
        }
    }
}
           /* var followTargetCommand = new MoveToDestinationCommand(navMesh, detection.target, reachedLocationDistance);
            ExecuteCommandNode followTarget = new ExecuteCommandNode(this, followTargetCommand);

            var rotateTowardsTargetCommand = new RotateTowardsCommand(transform, detection);
            ExecuteCommandNode rotateTowardsTarget = new ExecuteCommandNode(this, rotateTowardsTargetCommand);
             
            ConditionNode hasTargetInSightCondition = new ConditionNode(() => hasTargetInSight);
            ConditionNode hasNoOcclusionWithTargetCondition = new ConditionNode(() => hasTargetOcclusion);

            ConditionNode hasCloseRangeDistance = new ConditionNode("inDistance",() => Vector3.Distance(position, target.position) < detection.data.closeRangeAttackDistance);
            ConditionNode hasMidRangeDistance = new ConditionNode(() => Vector3.Distance(position, target.position) < detection.data.midRangeAttackDistance);
            ConditionNode hasLongRangeDistance = new ConditionNode(() => Vector3.Distance(position, target.position) < detection.data.farRangeAttackDistance);

            SequenceNode lookAtTarget = new SequenceNode(new List<Node>
            {
                hasTargetInSightCondition, 
            });



            SetDestinationNode followPlayer = new SetDestinationNode(this, 5f, detection.target);



            ReturnStateNode returnStateNode = new ReturnStateNode(Node.NodeState.Success);
            FallbackNode fallback = new FallbackNode(new List<Node>
            {
                HasOcclusion(), FoundTarget()
            });
            */