using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Job.SpellSystem.Data;
using Job.SpellSystem.Spells;
namespace Job.SpellSystem
{
    [CreateAssetMenu(menuName = "Spell", fileName = "Spell")]
    public class SpellSO : ScriptableObject
    {
        public SpellCombo spellCombo;

        [Space(10)]
        [SerializeReference] public List<ComponentData> SpellComponent;


        public Type[] GetAllDependencies()
        {
            return SpellComponent.Select(x => x.componentDependency).ToArray();
        }

        public T GetData<T>()
        {
            return SpellComponent.OfType<T>().FirstOrDefault();
        }
        
        public void AddSpellComponent(ComponentData component)
        {
            SpellComponent.Add(component);
        }
    }
}

