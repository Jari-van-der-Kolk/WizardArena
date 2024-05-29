using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public enum SpellType
{
    Projectile,
    Shield,
};

public abstract class SpellBase : ScriptableObject
{
    public SpellID spellID;
    public ElementType elementType;
    public bool damageOverTime;


    public void OnEnable()
    {
        Init();
    }

    public abstract void CastSpell(Transform origin);
    
    public virtual void Init()
    {
        if (spellID != null)
        {
            spellID.SetStringID(name);
        }
        SpellManager.Subscribe(this);
    }

}


