using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugCommand : MonoBehaviour
{

    int index;
    private CommandInvoker commandInvoker;

    private void Awake()
    {
        commandInvoker = GetComponent<CommandInvoker>();
    }
  
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            index++;
            CounterCommand debugLogCommand1 = new CounterCommand(index.ToString(),5);
            commandInvoker.queueCommands.AddCommand(debugLogCommand1);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            index++;
            CounterCommand debugLogCommand1 = new CounterCommand(index.ToString(), 5);
            commandInvoker.AddCommand(debugLogCommand1);

        }

    }
}
