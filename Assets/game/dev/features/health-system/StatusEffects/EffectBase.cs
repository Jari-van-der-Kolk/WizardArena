using TMPro.EditorUtilities;
using UnityEngine;

public abstract class EffectBase : IStatusEffect
{
    public LayerMask targetMask;
    public int statusHealthModifierAmount;

    int currentTick;
    int tickDuration;
    public bool cancelEffect;

    public EffectBase(LayerMask targetMask, int statusHealthModifierAmount)
    {
        this.targetMask = targetMask;
        this.statusHealthModifierAmount = statusHealthModifierAmount;
    }
   
    public virtual void OnApply(HealthComponent health, int tickDuration)
    {
        currentTick = 0;
        this.tickDuration = tickDuration;           
    }

    public virtual bool OnUpdate(HealthComponent health)
    {
        currentTick++;
        if(currentTick >= tickDuration)
        {
            return false;
        }
        return true;
    }

    public virtual void OnRemove(HealthComponent health)
    {
        health.StatusEffects.Remove(this);  
    }

    public void OnReset()
    {
        currentTick = -1;
    }
}




