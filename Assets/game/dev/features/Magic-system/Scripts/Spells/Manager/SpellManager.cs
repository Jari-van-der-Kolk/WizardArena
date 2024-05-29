using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private static List<SpellBase> spells;
    private static Dictionary<SpellID, SpellBase> spellMap = new Dictionary<SpellID, SpellBase>();

    private void Awake()
    {
        spellMap = new Dictionary<SpellID, SpellBase>();
        foreach (SpellBase spell in spells)
        {
            spellMap.Add(spell.spellID, spell);
        }

        print(spellMap.Count);
    }

    public static void Subscribe(SpellBase spell)
    {
        if (spells == null)
        {
            spells = new List<SpellBase>();
        }

        spells.Add(spell);
    }

    public static void CastSpellByString(string stringID, Transform origin)
    {
        stringID.ToLower();
        GetSpellByString(stringID)?.CastSpell(origin);
    }

    private static SpellBase GetSpellByString(string id)
    {
        foreach (var key in spellMap.Keys)
        {
            if (key.stringID == id)
            {
                return spellMap[key];
            }
        }

        Debug.LogWarning($"No spell found with ID {id}");
        return null;    
    }

    private static SpellBase GetStringByKeyCombonation(List<KeyCode> id) 
    {
        foreach(var key in spellMap.Keys)
        {
            if(key.playerKeyCombination == id)
            {
                return spellMap[key];
            }
        }

        Debug.LogWarning($"No spell found with ID {id}");
        return null;

    }

    private static string GetKeyCombinationString(List<KeyCode> keyCombination)
    {
        return string.Join("+", keyCombination);
    }

}

