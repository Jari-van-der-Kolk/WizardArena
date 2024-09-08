using UnityEngine;

public interface IActionBehaviour
{
    SpellID SpellID { get; }
    public void CastSpell(MonoBehaviour caller, string tag);

}


