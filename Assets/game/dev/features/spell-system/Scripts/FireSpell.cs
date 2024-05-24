using UnityEngine;
// Concrete Spells
public class FireSpell : ISpell
{
    public void Cast(Transform origin)
    {
        Debug.Log("Casting Fire Spell!");
        // Add fire spell specific logic here
    }
}


