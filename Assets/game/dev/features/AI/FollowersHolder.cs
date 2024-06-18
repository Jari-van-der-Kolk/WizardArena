using Saxon.BT.AI.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowersHolder : MonoBehaviour
{

    public IOwner employer;
    public List<IFollower> followers;

    private void Start()
    {
        employer = GetComponent<IOwner>();
        followers = new List<IFollower>();
    }

    public void SetEmployer(IOwner employer)
    {
        this.employer = employer;
    }

    public void AddFollower(IFollower follower)
    {
        
        followers.Add(follower);
    }

    public bool HaveFollowersDetectedTarget()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            if (followers[i].GetDetection())
            {
                return true;
            }
        }

        return false;
    }



}

public interface IFollower
{
    public bool GetDetection();

}

public interface IOwner
{

}



    /* public bool CheckServantsDetection(List<AgentBehaviourData> agents)
     {
         if (agents.Count > 0)
         {
             for (int i = 0; i < agents.Count; i++)
             {
                 if (agents[i].objectDetection.hasTargetInSight)
                 {
                     var target = agents[i].objectDetection.target;
                     detection.SetTarget(target);

                     for (int c = 0; c < agents.Count; c++)
                     {
                         agents[c].objectDetection.ToggleTargetRecentlyLost(true);
                         agents[c].objectDetection.ResetRecentlyLostTimer();
                         agents[c].objectDetection.SetTarget(target);
                     }

                     return true;
                 }

             }
         }
         return false;
     }*/
