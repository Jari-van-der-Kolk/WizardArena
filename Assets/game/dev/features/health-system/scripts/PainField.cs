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
    public bool deleteOnContact = true;

    [Inject]
    private StatusEffectFactory _effectFactory;
    private IStatusEffect _statusEffect;

    private void OnTriggerEnter(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();

        if (health != null)
        {
            _statusEffect = _effectFactory.Create(effectName, target, 2);
            health.ApplyStatusEffect(_statusEffect, 5);
            if (deleteOnContact)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        if(health != null)
        {
            health.GetStatusEffect(_statusEffect).ReleaseHold();
        }
    }

  
}
