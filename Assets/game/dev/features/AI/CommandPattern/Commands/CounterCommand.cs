using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCommand : Command
{

    private float maxTime = 5f; // Maximum time allowed for the command to complete
    private string name; // Maximum time allowed for the command to complete
    public CounterCommand(string name, float time)
    {
        this.name = name;
        maxTime = time;
    }

    private float timer = 0f; // Timer to track elapsed time

    public override object Current => null;

    public override bool MoveNext()
    {
        // Update the timer while the command is running
        if (timer < maxTime)
        {
            timer += Time.deltaTime;
            Debug.Log(name + " timer");
            return true; // Return true if the command is still running
        }
        else
        {
            Debug.Log(name + " complete");
            isExecuted = true;
            timer = 0f;
            return false; 
        }
    }

    public override void Reset()
    {
        // Reset the timer when resetting the command
        timer = 0f;
    }
}
