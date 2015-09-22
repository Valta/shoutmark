using UnityEngine;
using System.Collections;

public class attack_leaf : GeneralNode {

    public State curState = State.FAILURE; // init as success bc all is possible
    treeroot_script status;
    
    float animationtimer;
    float cooldowntimer;
    bool hasAttacked = false;
    // constructor for setup (not relying on Unity's Start or Awake)
    public attack_leaf(treeroot_script world_status)
    {
        status = world_status;
        
    }
    // presumably we need these. Use when found out where.
    public void Open()
    {
        
    }
    public void Close()
    {
        
    }
    public void StartAction()
    {
        // do smth at state beginning
        curState = State.RUNNING;
        animationtimer = 1;
        cooldowntimer = 2;
        hasAttacked = false;
        //info.ChangeText("Attacking...");
    }
    public void EndAction()
    {

    }
    public override State Tick()
    {
        
        if (CheckConditions())
        {
            if (curState != State.RUNNING)
            {
                StartAction();
            }
            else
            {
                // TODO: change to use master timer.
                if (animationtimer == 0 && hasAttacked == false)
                    Hit();
                else if (cooldowntimer == 0)
                    curState = State.SUCCESS;
            }
        }
        else curState = State.FAILURE;
        
        return curState;
    }
    public bool CheckConditions()
    {
        if (status.player_in_range)
            return true;
        return false;
    }
    void Hit()
    {
        //if (Chance(70))
        //    info.ChangeText("Hit!");
        //else
        //    info.ChangeText("Miss!");
        hasAttacked = true;
    }

}
