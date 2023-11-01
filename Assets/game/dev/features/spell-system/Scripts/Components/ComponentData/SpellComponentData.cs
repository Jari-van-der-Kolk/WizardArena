using System;
using UnityEngine;

namespace Job.SpellSystem
{
    [Serializable]
    public class SpellComponentData
    {
        [SerializeField, HideInInspector] private string _name;

        public SpellComponentData()
        {
            SetComponentName();
        }

        public void SetComponentName()
        {
            _name = GetType().Name;
        }
    }
}