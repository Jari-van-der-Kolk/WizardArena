using System.Collections.Generic;
using UnityEngine;

namespace Job.SpellSystem.Spells
{
    public class SpellHolder : MonoBehaviour
    {
        public SpellSOPrototype spellData;

        public List<SpellPrototype> spellComponents;
        
        public void SetSpellSO(SpellSOPrototype spell)
        {
            spellData = spell;
        }
        public void SetSpellComponents(List<SpellPrototype> components)
        {
            spellComponents = components;
        }
        public void LaunchSpell()
        {
            foreach (var spell in spellComponents)
            {
                spell.SetSpellHolders(this);
                spell.CastSpell();
            }
        }
        
    }
}