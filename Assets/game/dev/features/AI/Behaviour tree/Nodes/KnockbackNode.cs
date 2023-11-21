using UnityEngine;

namespace JBehaviourTree
{
    public class KnockbackNode : LeafNode
    {
        private Rigidbody2D rb;
        private Vector2 knockbackStrength;

        public KnockbackNode(Rigidbody2D rb, Vector2 knockbackStrength)
        {
            this.rb = rb;
            this.knockbackStrength = knockbackStrength;
        }
        
        protected override void OnStart()
        {
            EnemyFunctions.KnockBack(rb, knockbackStrength);
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}