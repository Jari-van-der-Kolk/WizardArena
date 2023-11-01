using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Job.SpellSystem
{
    [CreateAssetMenu(menuName = "Spell", fileName = "Spell")]
    public class Spell : ScriptableObject
    {
        public SpellCombo spellCombo;

        [Space(10)]
        [SerializeReference] public List<SpellComponentData> SpellComponent;

        public void AddSpellComponent(SpellComponentData component)
        {
            SpellComponent.Add(component);
        }
    }
}

