using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;

namespace Saxon.BT.AI
{
    public class NecroServant : Agent
    {
        public override RootNode rootNode { get; protected set; }

        public NecroServant(AgentControllerData agentData) : base(agentData) { rootNode = CreateTree(); }

        public override AgentType agentType { get { return AgentType.NecroServant; } protected set { } }

        public override RootNode CreateTree()
        {
            detection.Data.closeRangeAttackDistance = 5f;
            float findNewLocationRadius = 6f;
            float pickLocationRadius = 2f;
            OriginPatrolNode patrol = new OriginPatrolNode(this, findNewLocationRadius, pickLocationRadius);

            SelectorNode chaseCheck = new SelectorNode(new Node[]
            {
                recentlyLostTarget, targetInSight
            });

            SequenceNode chaseTarget = new SequenceNode(new Node[]
            {
                chaseCheck, ChaseTarget(detection.Data.closeRangeAttackDistance)
            });

            FallbackNode selector = new FallbackNode(new Node[]
            {
                chaseTarget, patrol
            });

            return new RootNode(selector);
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