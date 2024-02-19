using System.Collections.Generic;
using UnityEngine;

namespace Job.SpellSystem.Spells
{
    public class SpellHolder : MonoBehaviour
    {
        public SpellSO spellData;

        public List<Spell> spellComponents;
        
        public void SetSpellSO(SpellSO spell)
        {
            spellData = spell;
        }
        public void SetSpellComponents(List<Spell> components)
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