using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellID
{
    public string stringID = "placeholder";
    public List<KeyCode> playerKeyCombination = new List<KeyCode>();

    public SpellID()
    {
        Debug.Log("test");
    }

    public void SetStringID(string stringID) => this.stringID = stringID;
  

    public override bool Equals(object obj)
    {
        if (obj is SpellID other)
        {
            return stringID == other.stringID;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return stringID.GetHashCode();
    }


}

