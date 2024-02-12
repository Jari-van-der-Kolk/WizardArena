using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    public QueueCommands queueCommands { get; private set; }
    private Dictionary<Command, Coroutine> runningCommands = new Dictionary<Command, Coroutine>();

    private void Start()
    {
        queueCommands = new QueueCommands();
    }
    void Update()
    {
        queueCommands.ExecuteCommands();
        //print(gameObject + "running commands: " + runningCommands.Count);
        //print("queued commands: " + queueCommands.commandQueue.Count);
    }

    #region RunningCommands 
    public void AddCommand(Command command)
    {
        command.Enlisted();
        Coroutine coroutine = StartCoroutine(ExecuteCommand(command));
        runningCommands.Add(command, coroutine);
    }

    private IEnumerator ExecuteCommand(Command command)
    {
        while (command.MoveNext())
        {
            yield return null; // Yielding null to wait for the next frame
        }
        runningCommands.Remove(command);
    }


    public void CancelCommand(Command command)
    {
        StopRunningCommand(command);
    }
    public void ClearAllCommands()
    {
        queueCommands.ClearQueueCommands();
        ClearRunningCommands();
    }

    public void ClearRunningCommands()
    {
        foreach (var kvp in runningCommands)
        {
            StopCoroutine(kvp.Value);
        }
        runningCommands.Clear();
    }
   

    private void StopRunningCommand(Command command)
    {
        if (runningCommands.TryGetValue(command, out Coroutine coroutine))
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            runningCommands.Remove(command);
        }
    }

   
    
    #endregion
     
   


}

public class QueueCommands
{
    internal Queue<Command> commandQueue = new Queue<Command>();
    private Command currentCommand;

    public void ExecuteCommands()
    {
        if(commandQueue.Count > 0)
        { 
            Command currentCommand = commandQueue.Peek();
            if (currentCommand.MoveNext())
            {
                return;
            }

            currentCommand.Reset();
            commandQueue.Dequeue();
        }
    }

    public void AddCommand(Command command)
    {
        command.Enlisted();
        commandQueue.Enqueue(command);
    }
  
    private void ExecuteNextCommand()
    {
        if (commandQueue.Count > 0)
        {
            currentCommand = commandQueue.Peek();
        }
        else
        {
            currentCommand = null;
        }
    }

    public void CancelCurrentCommand()
    {
        CancelCommand(currentCommand);
    }

    public void CancelCommand(Command command)
    {
        if (command != null && commandQueue.Contains(command))
        {
            command.Reset();
            commandQueue = new Queue<Command>(commandQueue.Where(c => c != command));
        }

        ExecuteNextCommand();
    }

    public void ClearQueueCommands()
    {
        if (commandQueue.Count > 0)
        {
            // Resetting commands
            foreach (Command command in commandQueue)
            {
                command.Reset();
            }

            // Clearing the queue
            commandQueue.Clear();
        }
    }

    public void Swap(Command lhs, Command rhs)
    {
        if (lhs == null || rhs == null)
        {
            Debug.LogError("Cannot swap null commands.");
            return;
        }

        List<Command> commandList = commandQueue.ToList();

        for (int i = 0; i < commandList.Count; i++)
        {
            if (commandList[i] == lhs)
            {
                commandList[i].Reset();
                commandList[i] = rhs;
            }
            else if (commandList[i] == rhs)
            {
                commandList[i].Reset();
                commandList[i] = lhs;
            }
        }

        commandQueue = new Queue<Command>(commandList);
    }

}


