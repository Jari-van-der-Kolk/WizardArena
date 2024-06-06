using UnityEngine;

public class StatusEffectTemplate : StatusEffectBase
{
   
    public override void OnApply(HealthComponent health, int tickAmount, bool putOnHold = false)
    {
        base.OnApply(health, tickAmount);
        Effect(health);

    }

    public override bool OnUpdate(HealthComponent health)
    {
        if (!base.OnUpdate(health))
        {
            return false;
        }

        Effect(health);

        return true;
    }

    public override void OnRemove(HealthComponent health)
    {
        base.OnRemove(health);

    }

    private void Effect(HealthComponent health)
    {
        //health.SubractHealth(statusEffectTargets, statusHealthModifierAmount);
        
        //write anything here that needs to be new to the feature 
    }
}




