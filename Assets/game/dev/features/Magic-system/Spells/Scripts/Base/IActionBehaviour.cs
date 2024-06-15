using UnityEngine;

public interface IActionBehaviour
{
    SpellID SpellID { get; }
    public void CastSpell(Transform origin, TargetLayerData hitableLayers);

}


