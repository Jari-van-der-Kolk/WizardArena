using AYellowpaper;
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

    private void CastRandomAction()
    {
        // Example usage of casting a random spell at the start
        IActionBehaviour randomSpell = GetRandomAction();
        if (randomSpell != null)
        {
            //SpellManager.CastSpellByReference(randomSpell, transform, _castableTargetLayerData);
        }
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



