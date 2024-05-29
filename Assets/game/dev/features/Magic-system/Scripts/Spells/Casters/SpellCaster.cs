using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SpellCaster : MonoBehaviour
{
    private void Start()
    {
        SpellManager.CastSpellByString("shield", transform);
    }

    
}
