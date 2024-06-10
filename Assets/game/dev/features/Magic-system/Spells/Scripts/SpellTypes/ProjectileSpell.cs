using AYellowpaper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "NewProjectileSpell", menuName = "Magic System/Spells/ProjectileSpell")]

public class ProjectileSpell : SpellBase
{

    //[SerializeField] InterfaceReference<IHealthModifier, StatusEffectBase> _statusEffect;
    [SerializeField] private PainField _prefab;
    [SerializeField] private float _duration = 20f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _deleteSpellOnContact = true;

    public override void CastSpell(Transform origin, TargetLayerData hitableLayers)
    {
        var shield = Instantiate(_prefab, origin.position.Add(y: .25f).Add(z: 1.25f), Quaternion.identity);

        shield.GetComponent<PainField>();
        
        Vector3 targetPosition = origin.position + origin.forward * _speed * _duration;
        shield.transform.LeanMove(targetPosition, _duration).setLoopType(LeanTweenType.linear);

        Destroy(shield, _duration);
    }
}
