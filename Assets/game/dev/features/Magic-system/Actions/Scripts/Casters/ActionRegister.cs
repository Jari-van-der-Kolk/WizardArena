using AYellowpaper;
using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ActionType
{
    None,
    Heal,
    Revive,
    LightAttack,
    HeavyAttack,
    SpellRegisterComboOne,
    SpelLRegisterTwo,
    

}


[System.Serializable]
public class ActionReference
{
    public ActionType actionType;
    public InterfaceReference<IActionBehaviour> action;
}

[CreateAssetMenu(fileName = "NewActionRegister", menuName = "Magic System/Actions/ActionRegister")]

public class ActionRegister : ScriptableObject
{
    [SerializeField] private ActionReference[] availableActions;

    public IActionBehaviour GetActionBehaviourByString(string name)
    {
        for (int i = 0; i < availableActions.Length; i++)
        {
            if(availableActions[i].action.ToString() == name)
            {
                return availableActions[i].action.Value;
            }
        }

        return null;
    }

    public IActionBehaviour GetRandomActionFromRegister(ActionType type)
    {
        for (int i = 0; i < availableActions.Length; i++)
        {
            if (availableActions[i].actionType == type)
            {
                return availableActions[i].action.Value;    
            }
        }

        Debug.LogWarning($"Could not find action of type: {type}");
        return null;    
    }

    private IActionBehaviour GetRandomActionTypeValue(ActionType type)
    {
        List<IActionBehaviour> qualifiedActions = new List<IActionBehaviour> ();
        for (int i = 0; i < availableActions.Length; i++)
        {
            if (availableActions[i].actionType == type)
            {
                qualifiedActions.Add(availableActions[i].action.Value);
            }
        }

        int randomValue = Random.Range(0, qualifiedActions.Count);
        return qualifiedActions[randomValue];
    }


    private IActionBehaviour GetRandomAction()
    {
        if (availableActions == null || availableActions.Length == 0)
        {
            return null; // Return null if no spells are available
        }
        int randomIndex = Random.Range(0, availableActions.Length);
        return availableActions[randomIndex].action.Value;
    }
}



