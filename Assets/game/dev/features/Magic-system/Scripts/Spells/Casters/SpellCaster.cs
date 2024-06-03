using AYellowpaper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : SpellComboManager
{
    [SerializeField] private TargetLayerData targetLayerData;
    [SerializeField] private List<InterfaceReference<ISpell>> availableSpells = new List<InterfaceReference<ISpell>>();

    private void Start()
    {
        CastRandomSpell(); 
    }

    private void CastRandomSpell()
    {
        // Example usage of casting a random spell at the start
        ISpell randomSpell = GetRandomSpell();
        if (randomSpell != null)
        {
            SpellManager.CastSpellByReference(randomSpell, transform, targetLayerData.targetedLayers);
        }
    }

    private ISpell GetRandomSpell()
    {
        if (availableSpells == null || availableSpells.Count == 0)
        {
            return null; // Return null if no spells are available
        }
        int randomIndex = Random.Range(0, availableSpells.Count);
        return availableSpells[randomIndex].Value;
    }

}
