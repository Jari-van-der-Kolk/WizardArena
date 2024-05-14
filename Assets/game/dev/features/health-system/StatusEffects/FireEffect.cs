using UnityEngine;

public class FireEffect : EffectBase
{
    public FireEffect(LayerMask targetMask, int statusHealthModifierAmount) : base(targetMask, statusHealthModifierAmount)
    {
    }

    public override void OnApply(HealthComponent health, int tickAmount, bool putOnHold = false)
    {
        base.OnApply(health, tickAmount);
        Debug.Log(health.transform.name + " starts recieveing fire damage");
        Effect(health);

    }

    public override bool OnUpdate(HealthComponent health)
    {
        if (!base.OnUpdate(health))
        {
            return false;
        }

        Effect(health);

        Debug.Log(health.transform.name + " recieves fire damage");

        return true;
    }

    public override void OnRemove(HealthComponent health)
    {
        base.OnRemove(health);
        Debug.Log(health.transform.name + " stopped recieveing fire damage");

    }

    private void Effect(HealthComponent health)
    {
        health.SubractHealth(targetMask, statusHealthModifierAmount);
    }
}




