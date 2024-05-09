using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PainField : MonoBehaviour
{
    public string effectName;
    public LayerMask target;
    public bool removeOnHit;

    [Inject]
    private StatusEffectFactory _effectFactory;
    private List<IStatusEffect> _appliedEffects = new List<IStatusEffect>();

    private void OnTriggerEnter(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();

        if (health != null)
        {
            var statusEffect = _effectFactory.Get(effectName, target, 2);
            _appliedEffects.Add(statusEffect);
            health.ApplyStatusEffect(statusEffect, 5);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (var e in _appliedEffects)
        {
            e.OnReset();
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        
    }


}
