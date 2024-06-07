using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffectManager : MonoBehaviour
{


#region data
    public static List<IHealthModifier> healthModifiersTypes = new List<IHealthModifier>();


    #endregion

    #region config
    public int max_HealthModifiers = 1000;

#endregion

#region mutable


    #endregion
    public static void Subscribe(IHealthModifier statusEffect)
    {
        if(statusEffect == null)
        {
            healthModifiersTypes = new List<IHealthModifier>();
        }

        healthModifiersTypes.Add(statusEffect);
    }

    public static IHealthModifier GetHealthModifierByEnum(HealthModifierType type)
    {
        for (int i = 0; i < healthModifiersTypes.Count; i++)
        {
            if (healthModifiersTypes[i].ID.Type == type)
            {
                return healthModifiersTypes[i];
            }
        }

        return null;

    }

    public static IHealthModifier GetHealthModifierByString(string effectName)
    {
        effectName.ToLower();
        for (int i = 0; i < healthModifiersTypes.Count; i++)
        {
            if (healthModifiersTypes[i].ID.modifierName == effectName)
            {
                return healthModifiersTypes[i];
            }
        }

        Debug.LogWarning($"No StatusEffect found with ID {effectName}");

        return null;
    }

    

}
