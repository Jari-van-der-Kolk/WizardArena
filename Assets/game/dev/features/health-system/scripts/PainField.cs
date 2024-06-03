using AYellowpaper;
using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PainField : MonoBehaviour
{
    [SerializeField] private TargetLayerData targetLayerData;
    [SerializeField] private InterfaceReference<IStatusEffect, StatusEffectBase> _appliedEffect;
    public bool deleteOnContact = true;
    public int duration = 1;
 
    public void SetPainField(TargetLayerData targetLayerData, IStatusEffect statusEffect)
    {
        this.targetLayerData = targetLayerData;
        _appliedEffect.Value = statusEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();

        if (health != null)
        {
            health.ApplyStatusEffect(_appliedEffect.Value, duration, targetLayerData.targetedLayers);
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
            health.GetStatusEffect(_appliedEffect.Value).ReleaseHold();
        }
    }

    public PainField SetTargetLayer(TargetLayerData targetLayer)
    {
        this.targetLayerData = targetLayer;
        return this;
    }

    public PainField SetStatusEffect(IStatusEffect statusEffect)
    {
        _appliedEffect.Value = statusEffect;
        return this;
    }

    public PainField SetDeleteOnContect(bool deleteOnContact)
    {
        this.deleteOnContact = deleteOnContact;
        return this;
    }


  
}