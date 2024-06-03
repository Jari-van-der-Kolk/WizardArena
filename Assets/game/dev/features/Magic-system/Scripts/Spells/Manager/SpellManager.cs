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

    public static void CastSpellByString(string SpellName, Transform origin, LayerMask hitableTargets)
    {
        SpellName.ToLower();
        GetSpellByString(SpellName)?.CastSpell(origin, hitableTargets);
    }

    public static void CastSpellByReference(ISpell spell, Transform origin, LayerMask hitableTargets)
    {
        spell.CastSpell(origin, hitableTargets);
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

    private static ISpell GetStringByKeyCombonation(List<KeyCode> keyCombination) 
    {
        foreach(var key in spells)
        {
            if(key.SpellID.playerKeyCombination == keyCombination)
            {
                return key;
            }
        }
#if UNITY_EDITOR
        Debug.LogWarning($"No spell found with ID {keyCombination}");
#endif
        return null;

    }

}

