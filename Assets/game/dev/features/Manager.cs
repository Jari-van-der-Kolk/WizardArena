using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour, IDependencyProvider
{
    [Provide]
    public Manager ProvideManager()
    {
        return this;
    }

    #region config
    public int maxAmountOfAgents = 500;
    public int maxAmountOfSpells = 1000;

    #endregion

    #region mutable

    #endregion

    private void Awake()
    {
        
    }
}


