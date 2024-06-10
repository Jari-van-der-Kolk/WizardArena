using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainField : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


/*using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PainField : MonoBehaviour
{
    [SerializeField] private TargetLayerData _targetLayerData;
    //[SerializeField] private InterfaceReference<IHealthModifier, StatusEffectBase> _appliedEffect;
    [SerializeField] private bool _deleteOnContact = true;
    [SerializeField] private int duration = 1;

    *//*private Transform _owner;
 
  
    private void OnTriggerEnter(Collider other)
    {

        bool friendlyPainfieldCheck = other.GetComponent<PainField>()._targetLayerData == _targetLayerData; 
        if (_owner == other.transform || friendlyPainfieldCheck)
            return;

        other.GetComponent<HealthComponent>()?.ApplyStatusEffect(_appliedEffect.Value, duration, _targetLayerData.targetedLayers);
       
        if (_deleteOnContact)
            Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_owner == other.transform)
            return;

        HealthComponent health = other.GetComponent<HealthComponent>();
        if(health != null)
        {
            health.GetStatusEffect(_appliedEffect.Value).ReleaseHold();
        }
    }

    public PainField SetTargetLayer(TargetLayerData targetLayer)
    {
        this._targetLayerData = targetLayer;
        return this;
    }

    public PainField SetStatusEffect(IHealthModifier statusEffect)
    {
        _appliedEffect.Value = statusEffect;
        return this;
    }

    public PainField SetDeleteOnContect(bool deleteOnContact)
    {
        this._deleteOnContact = deleteOnContact;
        return this;
    }
    
    public PainField SetOwner(Transform owner)
    {
        this._owner = owner;    
        return this;
    }*//*





}*/