﻿using UnityEngine;
using System.Collections;

public class chase_leaf : GeneralNode {

    // constructor for setup (not relying on Unity's Start or Awake)
    public chase_leaf(treeroot_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        curState = State.FAILURE;
        instance = this;
    }
    // presumably we need these. Use when found out where.
    public override void Open()
    {
        if (!status.open_nodes.Contains(instance))
            status.open_nodes.Add(instance);
        base.Open();
    }
    public override void Close()
    {
        //status.open_nodes.Remove(this);
        base.Close();
    }
    public override void StartAction()
    {
        // do smth at state beginning
        // set move direction
        Debug.Log("chase");
        status.CloseOthers(this);
        curState = State.RUNNING;
        
    }
    public override void EndAction()
    {
        // do smth at state end
        Debug.Log("chase end");
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        if (CheckConditions())
        {
            if (curState != State.RUNNING)
                StartAction();
            //curState = State.SUCCESS;
            
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
