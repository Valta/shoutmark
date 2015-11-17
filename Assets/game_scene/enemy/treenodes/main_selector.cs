﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class main_selector : general_node {


    List<general_node> children;
    // constructor for setup (not relying on Unity's Start or Awake)
    public main_selector(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<general_node>();
        children.Add(new fight_sequence(world_status, this_actor));
        children.Add(new search_sequence(world_status, this_actor));
        children.Add(new idle_selector(world_status, this_actor));
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
        base.Close();
    }
    public override void StartAction()
    {
        // do smth at state beginning

    }
    public override void EndAction()
    {
        // do smth at state end
        //curState = State.SUCCESS;
    }
    public override State Tick()
    {
        
        for (int i = 0; i < children.Count; ++i)
        {            
            curState = children[i].exec();
            
            if (curState != State.FAILURE)
            {
                //status.CloseOthers(children[i]); // Close nodes on open list
                return curState;
            }
        }

        //// only another node can interfere.

        return curState;
    }

}
