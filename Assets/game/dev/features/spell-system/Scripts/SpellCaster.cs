using DependencyInjection;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [Inject]
    private SpellManager _spellManager;

    private void Update()
    {
       
    }

    public void CastSpell(SpellStratagy spellData)
    {
        
    }
}


