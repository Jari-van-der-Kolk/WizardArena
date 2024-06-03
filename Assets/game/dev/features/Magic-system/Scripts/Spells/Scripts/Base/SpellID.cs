using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellID
{
    public string EffectName = "placeholder";
    public List<KeyCode> playerKeyCombination = new List<KeyCode>();

    
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


}

