using System.Collections;
using UnityEngine;

public abstract class Command : IEnumerator
{
    public bool isExecuted = true;

    public virtual void Enlisted()
    {
        isExecuted = false;
        
    }
    public abstract bool MoveNext();

    public virtual void Reset() { }


    public abstract object Current { get; }
}

