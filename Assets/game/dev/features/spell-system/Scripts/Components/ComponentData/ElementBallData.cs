using System;
using Job.SpellSystem.Spells;
using UnityEngine;

namespace Job.SpellSystem.Data
{
    [Serializable]
    public class ElementBallData : ComponentData
    {
        public float radius;
        public float launchSpeed;
        public bool useGravity;
        public float damageOnContact;
        public GameObject particle;
        
        protected override void SetComponentDependency()
        {
            componentDependency = typeof(ElementBall);
        }
    }
}
