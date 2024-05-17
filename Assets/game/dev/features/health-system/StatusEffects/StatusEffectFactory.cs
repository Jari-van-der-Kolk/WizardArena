using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectFactory : MonoBehaviour, IDependencyProvider
{
    [Provide]
    public StatusEffectFactory ProvideStatusEffectFactory()
    {
        return this;
    }

    public void init()
    {
        Debug.Log("init");
    }

    public IStatusEffect Create(string name, LayerMask targetMask,int healthModifier)
    {
        switch(name)
        {
            case "Fire":
                return new FireEffect(targetMask, healthModifier);
            
            default :
                Debug.LogError("cannot return status effect by the name of: " + name + " it does not exist inside of the factory");
                return null;


        }

    }

}
