using UnityEngine;
using UnityEngine.UI;

public enum HealthModifierType
{
    None,
    Melee,
    Fire,
    Earth,
    Wind,
} 

public abstract class StatusEffectBase : ScriptableObject, IHealthModifier
{
    //config
    public int tickDuration { private get; set; }
    
    public Image effectLogo;
    public int statusHealthModifierAmount;
    
    
    //mutable
    public bool onHold {  get; private set; }
    int _currentTick;



    public HealthModifierID ID => throw new System.NotImplementedException();




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




