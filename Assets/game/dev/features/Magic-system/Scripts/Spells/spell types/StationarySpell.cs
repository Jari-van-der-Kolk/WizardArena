using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "NewStationarySpell", menuName = "Spell System/StationarySpell")]

public class StationarySpell : SpellBase
{

    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _duration;

    public override void Init()
    {
        base.Init();
        
    }

    public override void CastSpell(Transform origin)
    {
        var shield = Instantiate(_prefab, origin.position.With(y: 1f), Quaternion.identity);
        Destroy(shield, _duration);
    }
}
