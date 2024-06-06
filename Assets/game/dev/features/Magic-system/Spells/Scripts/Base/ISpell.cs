using UnityEngine;

public interface ISpell
{
    SpellID SpellID { get; }
    public void CastSpell(Transform origin, TargetLayerData hitableLayers);

}


