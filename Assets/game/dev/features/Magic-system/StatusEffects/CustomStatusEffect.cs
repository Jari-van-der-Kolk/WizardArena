using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Magic System/Effects/CustomEffect")]

public class CustomStatusEffect : StatusEffectBase
{
  /*  public override void OnApply(HealthComponent health, int tickAmount, bool putOnHold = false)
    {
        base.OnApply(health, tickAmount);
        Debug.Log(health.transform.name + $" starts recieveing {name} damage");
        Effect(health);

    }

    public override bool OnUpdate(HealthComponent health)
    {
        if (!base.OnUpdate(health))
        {
            return false;
        }

        Effect(health);

        Debug.Log(health.transform.name + $" recieves {name} damage");

        return true;
    }

    public override void OnRemove(HealthComponent health)
    {
        base.OnRemove(health);
        Debug.Log(health.transform.name + $" stopped recieveing {name} damage");

    }

    private void Effect(HealthComponent health)
    {
        health.SubractHealth(statusHealthModifierAmount);
    }*/
}




