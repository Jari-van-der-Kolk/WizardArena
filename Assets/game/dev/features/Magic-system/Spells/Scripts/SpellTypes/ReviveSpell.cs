using RoboRyanTron.Unite2017.Variables;
using Saxon.BT.AI.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utilities;
using Saxon.BT.AI.Types;

[CreateAssetMenu(fileName = "NewReviveSpell", menuName = "Magic System/Spells/ReviveSpell")]

public class ReviveSpell : SpellBase
{
    [SerializeField] private FloatVariable _radius;
    [SerializeField] private int reviveAmount;

    public override void CastSpell(MonoBehaviour caller, string tag)
    {
        var agents = caller.transform.GetComponentsInArea<AgentBehaviour>(_radius.Value);
        
        for (int i = 0; i < agents.Count; i++)
        {
            if(i > reviveAmount)
            {
                break;
            }

            var agent = agents[i];
            if (!agent.data.alive)
            {

                var owner = caller.GetComponent<IOwner>();
                if(owner == null)
                {
                    Debug.LogError($"{caller.name} needs to be assigned a FollowersHolder component!");
                }

                agent.data.Revive();
                agent.data.SetOrigin(caller.transform);
                agent.data.followersHolder.SetEmployer(owner);
                agent.data.ChangeAgentType(AgentType.NecroServant);
                agent.UpdateData();
                caller.GetComponent<FollowersHolder>().AddFollower(agent);
            }
        }
    }
}
