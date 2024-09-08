using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private static List<IActionBehaviour> spells;

    public static void Subscribe(SpellBase spell)
    {
        if (spells == null)
        {
            spells = new List<IActionBehaviour>();
        }

        spells.Add(spell);
    }

    public static void CastSpellByString(MonoBehaviour origin, string SpellName, string tag)
    {
        SpellName.ToLower();
        GetSpellByString(SpellName)?.CastSpell(origin, tag);
    }

    public static void CastSpellByReference(MonoBehaviour origin, IActionBehaviour action, string tag)
    {
        action.CastSpell(origin, tag);
    }

    public static void CastSpellByKeyID(MonoBehaviour origin, List<KeyCode> keyCombination, string tag)
    {
        GetSpellByKeyCombonation(keyCombination)?.CastSpell(origin, tag); 
    }

    public static IActionBehaviour GetSpellByString(string spellName)
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

    private static IActionBehaviour GetSpellByKeyCombonation(List<KeyCode> keyCombination) 
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

