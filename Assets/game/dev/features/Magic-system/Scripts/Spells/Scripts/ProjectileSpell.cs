using AYellowpaper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "NewProjectileSpell", menuName = "Magic System/Spells/ProjectileSpell")]

public class ProjectileSpell : SpellBase
{

    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _duration;
    [SerializeField] InterfaceReference<IStatusEffect, StatusEffectBase> _statusEffect;

   
    public override void CastSpell(Transform origin, LayerMask hitableLayers)
    {
        var shield = Instantiate(_prefab, origin.position.With(y: 1f), Quaternion.identity);
        shield.layer = hitableLayers;
        shield.GetComponent<PainField>().SetStatusEffect(_statusEffect.Value);
        Destroy(shield, _duration);
    }
}
