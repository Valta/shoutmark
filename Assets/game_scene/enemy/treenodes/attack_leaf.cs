using UnityEngine;
using System.Collections;

public class attack_leaf : GeneralNode {

    float animationtimer;
    float cooldowntimer;
    bool hasAttacked = false;
    // constructor for setup (not relying on Unity's Start or Awake)
    public attack_leaf(treeroot_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        curState = State.FAILURE;
    }
    // presumably we need these. Use when found out where.
    public override void Open()
    {
        open = true;
    }
    public override void Close()
    {
        open = false;
    }
    public override void StartAction()
    {
        // do smth at state beginning
        curState = State.RUNNING;
        animationtimer = 1;
        cooldowntimer = 2;
        hasAttacked = false;
        
    }
    public override void EndAction()
    {

    }
    public override State Tick()
    {
        
        if (CheckConditions())
        {
            Debug.Log("fight");
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
