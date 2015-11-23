using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class search_sequence : general_node {

    List<general_node> children;
    // constructor for setup (not relying on Unity's Start or Awake)
    public search_sequence(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<general_node>();
        children.Add(new is_searching(world_status, this_actor));
        children.Add(new search_successor(world_status, this_actor));        
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
        //MESSAGE.print(curState.ToString(), 0, 0, 12, 768);
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
