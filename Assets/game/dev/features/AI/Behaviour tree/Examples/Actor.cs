using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.BT;


namespace Saxon.Examples
{

    public class Actor : MonoBehaviour
    {
        internal Rigidbody2D rb;
        internal NavMeshAgent agent;
        internal Transform playerLocation;
        public float detectionDistance = 8.5f;
        public float searchRandomLocationRadius = 5;
        public float reachedDestination = 0.75f;
    
        private NavMeshPath navMeshPath;
    
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();      
            agent = GetComponent<NavMeshAgent>();
            playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
    
            navMeshPath = new NavMeshPath();
        }
    
        public Node.NodeState GoToLocation()
        {
            if (Vector2.Distance(transform.position, agent.destination) < reachedDestination)
            {
                return Node.NodeState.Success;
            }
            return Node.NodeState.Running;
        }
    
        public Node.NodeState GoToPlayer()
        {
            agent.SetDestination(playerLocation.position);
            return Node.NodeState.Success;
        }
    
        public bool HasObjectInSight(Transform location)
        {
            var raycast = Physics2D.Raycast(transform.position, (location.position - transform.position).normalized * detectionDistance);
            var distance = Vector2.Distance(transform.position, location.position);
            Debug.DrawRay(transform.position, (location.position - transform.position).normalized * detectionDistance, Color.blue);
    
            if (distance < detectionDistance && raycast.collider != null && raycast.collider.CompareTag("Player"))
            {
                return true;
            }
    
            return false;
        }
    
        public Node.NodeState HasPlayerInSight()
        {
            if (!HasObjectInSight(playerLocation))
                return Node.NodeState.Success;
    
            return Node.NodeState.Failure;
        }
    
        public Node.NodeState TargetPlayer()
        {
            agent.destination = playerLocation.position;
            return Node.NodeState.Success;
        }
    
        public Node.NodeState HasReachedLocation()
        {
            if(Vector2.Distance(transform.position, agent.destination) < reachedDestination)
            {
                return Node.NodeState.Success;
            }
    
            return Node.NodeState.Running;
        }
    
        public Node.NodeState SearchForRandomLocation()
        {
            Vector3 randomDirection = transform.position + UnityEngine.Random.insideUnitSphere * searchRandomLocationRadius;
            if (NavMesh.SamplePosition(randomDirection,out var hit, searchRandomLocationRadius, 1))  {
                agent.destination = hit.position;
            }
    
            if (!CheckPath())
            {
                SearchForRandomLocation();
                return Node.NodeState.Running;
            }
    
            return Node.NodeState.Success;
        }
    
        internal bool CheckPath()
        {
            agent.CalculatePath(agent.destination, navMeshPath);
            if (navMeshPath.status != NavMeshPathStatus.PathComplete)
                return false;
            else
                return true;
        }
    
    }

}
