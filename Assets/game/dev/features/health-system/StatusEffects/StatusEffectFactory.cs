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

    public IStatusEffect Create(ElementType element, LayerMask targetMask,int healthModifier)
    {
        switch(element)
        {
            case ElementType.Fire:
                return new FireEffect(targetMask, healthModifier);
            
            default :
                Debug.LogError("cannot return status effect by the name of: " + name + " it does not exist inside of the factory");
                return null;


        }

    }

}
