﻿using UnityEngine;
using System.Collections;

public class chase_leaf : GeneralNode {

    // constructor for setup (not relying on Unity's Start or Awake)
    public chase_leaf(treeroot_script world_status, enemy_script this_actor)
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
        // set move direction   
        //Debug.Log("chase");
    }
    public override void EndAction()
    {
        // do smth at state end
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        if (CheckConditions())
        {            
            curState = State.SUCCESS;
            StartAction();
        }
        else curState = State.FAILURE;
        return curState;
    }
    public bool CheckConditions()
    {
        if (status.player_sighted && !status.player_in_range)
            return true;
        return false;
    }
}
