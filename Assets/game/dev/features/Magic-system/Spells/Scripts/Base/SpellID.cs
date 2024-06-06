using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellID
{
    public string EffectName = "placeholder";
    public KeyCode[] playerKeyCombination;

    
    public void SetEffectName(string stringID) => this.EffectName = stringID.ToLower();
  

    public override bool Equals(object obj)
    {
        if (obj is SpellID other)
        {
            return EffectName == other.EffectName;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return EffectName.GetHashCode();
    }

    public bool CompareKeysIDs(KeyCode[] lhs)
    {
        // Check if the lengths of the arrays are different
        if (lhs.Length != playerKeyCombination.Length)
        {
            return false;
        }

        // Compare each element
        for (int i = 0; i < lhs.Length; i++)
        {
            if (lhs[i] != playerKeyCombination[i])
            {
                return false;
            }
        }

        // All elements are equal
        return true;
    }


}

