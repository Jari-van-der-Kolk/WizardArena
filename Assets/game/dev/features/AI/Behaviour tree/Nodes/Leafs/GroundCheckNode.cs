using UnityEngine;

namespace Saxon.BT
{
    public class GroundCheckNode : LeafNode
    {
        private Collider2D groundCheckCollider;
        private LayerMask mask;

        public GroundCheckNode(Collider2D groundCheckCollider, LayerMask mask)
        {
            this.groundCheckCollider = groundCheckCollider;
            this.mask = mask;
        }
        
        
        
        protected override void OnStart() { }

        internal override void OnStop() { }

        protected override NodeState OnUpdate()
        {
            if (groundCheckCollider.IsTouchingLayers(mask))
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }
    }
}