using RoboRyanTron.Unite2017.Variables;
using Saxon.BT.AI.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

[CreateAssetMenu(fileName = "NewReviveSpell", menuName = "Magic System/Spells/ReviveSpell")]

public class ReviveSpell : SpellBase
{
    [SerializeField] private FloatVariable _radius;

    public override void CastSpell(Transform origin, TargetLayerData hitableLayers)
    {
        var agents = origin.GetComponentsInArea<AgentController>(_radius.Value);
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].SetAgentActivity(true);
        }
    }
}
