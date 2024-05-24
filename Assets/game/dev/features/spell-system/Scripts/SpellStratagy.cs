using DependencyInjection;
using System.Collections.Generic;
using UnityEngine;
// Spell Data ScriptableObject
public abstract class SpellStratagy : ScriptableObject
{
    [SerializeField] [Inject] private SpellManager _spellManager;
    public ElementType element;
    public List<KeyCode> keyCombination;
    public abstract void CastSpell(Transform origin);
}

