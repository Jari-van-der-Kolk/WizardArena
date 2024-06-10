using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffectManager : MonoBehaviour
{
    public static List<IHealthModifier> healthModifiersTypes = new List<IHealthModifier>();


    [Inject]
    //private manager
    public static void Subscribe(IHealthModifier statusEffect)
    {
        if(statusEffect == null)
        {
            healthModifiersTypes = new List<IHealthModifier>();
        }

        healthModifiersTypes.Add(statusEffect);
    }

  

    

}
