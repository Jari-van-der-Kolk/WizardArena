using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private static List<ISpell> spells;

    public static void Subscribe(SpellBase spell)
    {
        if (spells == null)
        {
            spells = new List<ISpell>();
        }

        spells.Add(spell);
    }

    public static void CastSpellByString(string SpellName, Transform origin, TargetLayerData hitableTargets)
    {
        SpellName.ToLower();
        GetSpellByString(SpellName)?.CastSpell(origin, hitableTargets);
    }

    public static void CastSpellByReference(ISpell spell, Transform origin, TargetLayerData hitableTargets)
    {
        spell.CastSpell(origin, hitableTargets);
    }

    public static void CastSpellByKeyID(List<KeyCode> keyCombination, Transform origin, TargetLayerData hitableTargets)
    {
        GetSpellByKeyCombonation(keyCombination)?.CastSpell(origin, hitableTargets); 
    }

    public static ISpell GetSpellByString(string spellName)
    {
        spellName.ToLower();
        foreach (var key in spells)
        {
            if (key.SpellID.EffectName == spellName)
            {
                return key;
            }
        }
#if UNITY_EDITOR
        Debug.LogWarning($"No spell found with ID {spellName}");
#endif
        return null;    
    }

    private static ISpell GetSpellByKeyCombonation(List<KeyCode> keyCombination) 
    {
        for (int i = 0; i < spells.Count; i++)
        {
            if (spells[i].SpellID.CompareKeysIDs(keyCombination.ToArray()))
            {
                return spells[i];
            }
        }
#if UNITY_EDITOR
        Debug.LogWarning($"No spell found with ID {keyCombination}");
#endif
        return null;

    }

    

}

