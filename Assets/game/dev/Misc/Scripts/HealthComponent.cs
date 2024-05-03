using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public interface IStatusEffectObserver
{
    void OnApply(HealthComponent health, int tickAmount);
    void OnUpdate(HealthComponent health);
    void OnRemove(HealthComponent health);
}

public abstract class EffectBase : IStatusEffectObserver
{
    int currentTick;
    int tickDuration;
    public bool cancelEffect;

    public virtual void OnApply(HealthComponent health, int tickDuration)
    {
        currentTick = 0;
        this.tickDuration = tickDuration;           
    }

    public virtual void OnUpdate(HealthComponent health)
    {
        currentTick++;
        if(currentTick >= tickDuration)
        {
            OnRemove(health);  
        }
    }

    public virtual void OnRemove(HealthComponent health)
    {
        cancelEffect = false;  
    }
}

public class FireEffect : EffectBase
{
    public override void OnApply(HealthComponent health, int tickAmount)
    {
        base.OnApply(health, tickAmount);
    }

    public override void OnUpdate(HealthComponent health)
    {
        base.OnUpdate(health);
    }

    public override void OnRemove(HealthComponent health)
    {
        base.OnRemove(health);
    }
}



public class HealthComponent : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private Observer<int> health = new Observer<int>(100);
    [SerializeField] private UnityEvent deathEvent;

     private List<IStatusEffectObserver> statusEffects = new List<IStatusEffectObserver>();

    readonly float tickSpeed = 1f;
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

    public void ApplyStatusEffect(IStatusEffectObserver statusEffect, int duration)
    {
        if (statusEffect == null)
            return;

        statusEffect.OnApply(this, duration);
        statusEffects.Add(statusEffect);
    }

    public void UpdateStatusEffects()
    {
        tick += tickSpeed * Time.deltaTime;

        if(tick > 1f)
        {
            foreach (IStatusEffectObserver s in statusEffects)
            {
                s.OnUpdate(this);
            }
            
            tick = 0f;
        }
    }
}




