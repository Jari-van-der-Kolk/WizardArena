using TMPro.EditorUtilities;
using UnityEngine;



public abstract class EffectBase : IStatusEffect
{
    public LayerMask targetMask;
    public int statusHealthModifierAmount;
    public bool onHold;

    int _currentTick;
    int _tickDuration;

    public EffectBase(LayerMask targetMask, int statusHealthModifierAmount)
    {
        this.targetMask = targetMask;
        this.statusHealthModifierAmount = statusHealthModifierAmount;
    }
   
    public virtual void OnApply(HealthComponent health, int tickDuration, bool putOnHold = false)
    {
        _currentTick = 0;
        this._tickDuration = tickDuration;           
        onHold = putOnHold;

    }

    public virtual bool OnUpdate(HealthComponent health)
    {
        if(onHold == false)
            _currentTick++;

        if(_currentTick >= _tickDuration)
            return false;

        return true;
    }

    public virtual void OnRemove(HealthComponent health)
    {
        health.StatusEffects.Remove(this);  
    }

    public void ReleaseHold()
    {
        onHold = false;
    }

  
}




