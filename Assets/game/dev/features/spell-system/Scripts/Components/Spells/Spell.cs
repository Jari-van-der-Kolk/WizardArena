using UnityEngine;

namespace Job.SpellSystem.Spells

{
    public abstract class Spell : MonoBehaviour
    {
        protected SpellHolder Holder;
        
        public abstract void CastSpell();

        public void SetSpellHolders(SpellHolder holder)
        {
            Holder = holder;
        }
    }
}