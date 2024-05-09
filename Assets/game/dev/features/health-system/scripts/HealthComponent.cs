using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class HealthComponent : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private Observer<int> health = new Observer<int>(100);
    [SerializeField] private UnityEvent deathEvent;

    private List<IStatusEffect> statusEffects = new List<IStatusEffect>();
    public List<IStatusEffect> StatusEffects { get { return statusEffects; } }


    readonly float tickDelay = 2f;
    private float tick;

    private void Start()
    {
        health.Invoke();
    }

    private void Update()
    {
        UpdateStatusEffects();
    }

    public void AddHealth(LayerMask layer, int amount)
    {

        if(layer != hitLayer)
            return;

        this.health.Value += amount;

        if (health.Value <= 0)
        {
            deathEvent.Invoke();
        }

    }
    
    public void SubractHealth(LayerMask layer, int amount) 
    {
        if (layer != hitLayer)
            return;

        this.health.Value -= amount;

        if (health.Value <= 0)
        {
            deathEvent.Invoke();
        }
    }

    public void ApplyStatusEffect(IStatusEffect appliedEffect, int duration)
    {
        if (appliedEffect == null)
            return;

        foreach (IStatusEffect existingEffects in statusEffects)
        {
            if(existingEffects.GetType() == appliedEffect.GetType())
            {
                return;
            }
        }

        appliedEffect.OnApply(this, duration);
        statusEffects.Add(appliedEffect);
    }

    public void CancelStatusEffect(IStatusEffect statusEffect)
    {
        if(statusEffects.Contains(statusEffect))
        {
            statusEffects.Remove(statusEffect);
        }
    }

   
    
    public void UpdateStatusEffects()
    {
        tick += 1f * Time.deltaTime;

        if(tick > tickDelay)
        {
            foreach (IStatusEffect s in statusEffects)
            {
                if (!s.OnUpdate(this))
                {
                    s.OnRemove(this);
                    break;
                }
            }
            
            tick = 0f;
        }
    }

}




