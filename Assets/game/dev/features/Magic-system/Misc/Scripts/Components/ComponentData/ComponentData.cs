using System;
using UnityEngine;
using Job.SpellSystem.Spells;
namespace Job.SpellSystem.Data
{
    [Serializable]
    public abstract class ComponentData
    {
        [SerializeField, HideInInspector] private string _name;

        public Type componentDependency { get; protected set; }

        public ComponentData()
        {
            SetComponentName();
            SetComponentDependency();
        }

        public void SetComponentName()
        {
            _name = GetType().Name;
        }

        protected abstract void SetComponentDependency();
    }
}