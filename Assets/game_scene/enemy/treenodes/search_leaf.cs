﻿using UnityEngine;
using System.Collections;

public class search_leaf : general_node {

    // constructor for setup (not relying on Unity's Start or Awake)
    public search_leaf(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        curState = State.FAILURE;
        instance = this;
    }
    // presumably we need these. Use when found out where.
    public override void Open()
    {
        base.Open();
    }
    public override void Close()
    {
        //status.open_nodes.Remove(this);
        base.Close();
    }
    public override void StartAction()
    {
        Debug.Log("search start to dest " + status.player_last_seen + " from pos " + status._position);
        
        actor.SetDirection(status.player_last_seen.x, status.player_last_seen.y); 
        curState = State.RUNNING;
       
    }
    public override void EndAction()
    {
        // do smth at state end
        Debug.Log("search end");
        curState = State.SUCCESS;
        
        actor.SetLooking(true);
    }
    public override State Tick()
    {
        if (CheckConditions())
        {
            MESSAGE.print("search", -100, -70, 12, 2000);
            if (curState != State.RUNNING)
                StartAction();
            else if (isnotnear()) ///// TODO: Better condition!!!!
            {
                actor.SetDirection(status.player_last_seen.x, status.player_last_seen.y);
            }
            else EndAction();
        }
        else
        {
            Debug.Log("search failed");
            curState = State.FAILURE;
        }
        return curState;
    }
    public bool CheckConditions()
    {
        // player is out of sight AND last known position is not reached
        if (!status.player_sighted && status.searching)
            return true;
        return false;
    }
    private bool isnotnear()
    {
        bool near = (Mathf.Abs(status._position.x - status.player_last_seen.x) < 0.2f &&
                     Mathf.Abs(status._position.y - status.player_last_seen.y) < 0.2f);
        if (near) Debug.Log("search destination reached" + status.player_last_seen + " from pos " + status._position);
        return !near;
    }
}
