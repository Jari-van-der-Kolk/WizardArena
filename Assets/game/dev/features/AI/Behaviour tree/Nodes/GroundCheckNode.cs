using UnityEngine;

namespace JBehaviourTree
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

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (groundCheckCollider.IsTouchingLayers(mask))
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}