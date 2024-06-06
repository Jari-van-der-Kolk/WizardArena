using UnityEngine;
using UnityEngine.UI;

public abstract class StatusEffectBase : ScriptableObject, IStatusEffect
{
    public Image effectLogo;
    public int statusHealthModifierAmount;
    public bool onHold {  get; private set; }
    public int tickDuration { private get; set; }

    public string Name => name;

    int _currentTick;

    public void OnEnable()
    {
        Init(); 
    }

    public virtual void Init()
    {
        StatusEffectManager.Subscribe(this);
    }

    public virtual void OnApply(HealthComponent health, int tickDuration, bool putOnHold = false)
    {
        _currentTick = 0;
        this.tickDuration = tickDuration;           
        onHold = putOnHold;

    }

    public virtual bool OnUpdate(HealthComponent health)
    {
        if(onHold == false)
            _currentTick++;

        if(_currentTick >= tickDuration)
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




