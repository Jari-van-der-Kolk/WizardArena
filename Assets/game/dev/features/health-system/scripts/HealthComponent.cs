using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{

    [SerializeField] private TargetLayerData hitLayer;
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

    public void UpdateStatusEffects()
    {
        tick += 1f * Time.deltaTime;

        if (tick > tickDelay)
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

    public void AddHealth(int amount)
    {
        this.health.Value += amount;

        if (health.Value <= 0)
        {
            deathEvent.Invoke();
        }
    }
    
    public void SubractHealth(int amount) 
    {
        this.health.Value -= amount;

        if (health.Value <= 0)
        {
            deathEvent.Invoke();
        }
    }

    public void ModifyHeatlh(int amount)
    {
        this.health.Value += amount;

        if (health.Value <= 0)
        {
            deathEvent.Invoke();
        }
    }

    public void ApplyStatusEffect(IStatusEffect appliedEffect, int duration, LayerMask hitMask)
    {
        if (appliedEffect == null || hitLayer.targetedLayers != hitMask)
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

    public IStatusEffect GetStatusEffect(IStatusEffect effect)
    {
        foreach (IStatusEffect existingEffects in statusEffects)
        {
            print(existingEffects.GetType().ToString() + " "  + effect.GetType().ToString());
            if (existingEffects.GetType() == effect.GetType())
            {
                return effect;
            }
        }
        Debug.LogError("there is no such type available on this object: make sure you have null checked the code that provides this method");
        return null;
    }

    public void ScaleHealthBar(Image image)
    {
        Vector3 newScale = image.rectTransform.localScale;
        newScale.x = 1.0f / 100.0f * health.Value;
        image.rectTransform.localScale = newScale;
    }



   

}




