using UnityEngine;
using UnityEngine.UI;

public enum HealthModifierType
{
    None,
    Melee,
    Fire,
    Earth,
    Wind,
} 

public abstract class StatusEffectBase : ScriptableObject, IHealthModifier
{
    //config
    public int tickDuration { private get; set; }
    
    public Image effectLogo;
    public int statusHealthModifierAmount;
    
    
    //mutable
    public bool onHold {  get; private set; }
    int _currentTick;


    public void OnEnable()
    {
        StatusEffectManager.Subscribe(this);
    }

   
    public void ReleaseHold()
    {
        onHold = false;
    }

  
}




