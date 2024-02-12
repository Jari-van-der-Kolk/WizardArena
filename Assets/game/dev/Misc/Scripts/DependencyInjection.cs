using System;
using System.Collections;
using UnityEngine;

namespace DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {

    }
    
    public class DependencyInjection : MonoBehaviour
    {
       
    }
}