using UnityEngine;

public class FireEffect : EffectBase
{
    public FireEffect(LayerMask targetMask, int statusHealthModifierAmount) : base(targetMask, statusHealthModifierAmount)
    {
    }

    public override void OnApply(HealthComponent health, int tickAmount)
    {
        base.OnApply(health, tickAmount);
        Debug.Log(health.transform.name + " starts recieveing fire damage");

    }

    public override bool OnUpdate(HealthComponent health)
    {
        if (!base.OnUpdate(health))
        {
            return false;
        }

        health.SubractHealth(targetMask, statusHealthModifierAmount);

        Debug.Log(health.transform.name + " recieves fire damage");

        return true;
    }

    public override void OnRemove(HealthComponent health)
    {
        base.OnRemove(health);
        Debug.Log(health.transform.name + " stopped recieveing fire damage");

    }
}




