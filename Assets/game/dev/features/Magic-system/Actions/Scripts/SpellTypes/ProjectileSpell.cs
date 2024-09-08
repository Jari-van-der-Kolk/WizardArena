using AYellowpaper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "NewProjectileSpell", menuName = "Magic System/Spells/ProjectileSpell")]

public class ProjectileSpell : SpellBase
{

    //[SerializeField] InterfaceReference<IHealthModifier, StatusEffectBase> _statusEffect;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _duration = 20f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _deleteSpellOnContact = true;

    public override void CastSpell(MonoBehaviour caller, string tag)
    {
        var projectile = Instantiate(_prefab, caller.transform.position + caller.transform.forward.normalized * 1.25f, caller.transform.rotation);

        Vector3 targetPosition = caller.transform.position + Camera.main.transform.forward * _speed * _duration;
        projectile.transform.LeanMove(targetPosition, _duration).setLoopType(LeanTweenType.linear);

        Destroy(projectile, _duration);
    }
}
