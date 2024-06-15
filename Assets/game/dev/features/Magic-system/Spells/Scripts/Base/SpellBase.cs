using AYellowpaper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;


public abstract class SpellBase : ScriptableObject, IActionBehaviour
{
    [SerializeField] private SpellID spellID;
    public SpellID SpellID { get { return spellID; } }


    public void OnEnable()
    {
        Init();
    }
    
    public virtual void Init()
    {
        if (spellID != null)
        {
            spellID.SetEffectName(name);
        }
        SpellManager.Subscribe(this);
    }
    public abstract void CastSpell(Transform origin, TargetLayerData hitableLayers);

}


