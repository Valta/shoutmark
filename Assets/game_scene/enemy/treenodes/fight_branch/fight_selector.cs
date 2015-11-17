using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class fight_selector : general_node {

    List<general_node> children;

    // constructor for setup 
    public fight_selector(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<general_node>();
        children.Add(new attack_sequence(world_status, this_actor));
        children.Add(new chase_leaf(world_status, this_actor));
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
        base.Close();
    }
    public override void StartAction()
    {

    }
    public override void EndAction()
    {

    }
    public override State Tick()
    {            
        for (int i = 0; i < children.Count; ++i)
        {
            curState = children[i].exec();
                
            if (curState != State.FAILURE)
            {                    
                return curState;
            }
        }
        return curState;
    }

}
