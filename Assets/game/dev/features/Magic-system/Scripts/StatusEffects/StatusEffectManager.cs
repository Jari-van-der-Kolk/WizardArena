using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffectManager : MonoBehaviour
{

    public static List<IStatusEffect> statusEffects = new List<IStatusEffect>(); 

    public static void Subscribe(IStatusEffect statusEffect)
    {
        if(statusEffect == null)
        {
            statusEffects = new List<IStatusEffect>();
        }

        statusEffects.Add(statusEffect);
    }

    public static IStatusEffect GetStatusEffectByString(string effectName)
    {
        effectName.ToLower();
        foreach (var statusEffect in statusEffects)
        {
            if(statusEffect.Name == effectName)
            {
                return statusEffect;
            }
        }

        Debug.LogWarning($"No StatusEffect found with ID {effectName}");

        return null;
    }

    

}
