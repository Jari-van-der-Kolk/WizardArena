using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class PainField : MonoBehaviour
{
    //[SerializeField] private InterfaceReference<IHealthModifier, StatusEffectBase> _appliedEffect;
    


    //mutable
    [Tag][SerializeField] private string targetTag;
    [Tag][SerializeField] private string friendlyTag;
    [SerializeField] private bool _deleteOnContact = true;
    [SerializeField] private int duration = 1;


    public PainField SetTag(string tag)
    {
        targetTag = tag;
        return this;
    } 

    public PainField SetDeleteOnContact(bool deleteOnContact = true)
    {
        _deleteOnContact = deleteOnContact;
        return this;
    }

    public PainField SetDuration(GameObject gameObject, int duration)
    {
        Destroy(gameObject, duration);
        return this;
    }

    public PainField SetFriendlyTag(string tag)
    {
        friendlyTag = tag;
        return this;

    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {

        }
      
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {

        }
    }







}