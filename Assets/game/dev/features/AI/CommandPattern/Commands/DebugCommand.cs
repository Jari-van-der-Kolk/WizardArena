using UnityEngine;

public class DebugLogCommand : Command
{
    private readonly string logMessage;

    public DebugLogCommand(string logMessage)
    {
        this.logMessage = logMessage;
    }

    public override bool MoveNext()
    {
        // Log the message to the console
        Debug.Log(logMessage);

        // Mark the command as executed
        isExecuted = true;

        // This command doesn't have a continuous execution, so return false
        return false;
    }

    

    public override object Current => null;
}
