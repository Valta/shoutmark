using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fight_sequence : general_node {

    List<general_node> children;
    // constructor for setup (not relying on Unity's Start or Awake)
    public fight_sequence(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<general_node>();
        children.Add(new is_visible(world_status, this_actor));
        children.Add(new lock_on_target_leaf(world_status, this_actor));
        children.Add(new attack_decorator(world_status, this_actor));
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
            
            if (curState == State.FAILURE)
            {                
                return curState;
            }
        }
        
        return curState;
    }
}
