using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class idle_selector : general_node
{
    List<general_node> children;

    // constructor for setup (not relying on Unity's Start or Awake)
    public idle_selector(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<general_node>();
        children.Add(new gossip_leaf(world_status, this_actor));
        children.Add(new idle_leaf(world_status, this_actor));
        curState = State.SUCCESS;
    }
    // presumably we need these. Use when found out where.
    public override void Open()
    {
        base.Open();
    }
    public override void Close()
    {
        base.Close();
    }
    public override void StartAction()
    {
        // do smth at state beginning
        // start moving in random direction here / roaming coroutine?
        // curState = State.RUNNING;
        status.searching = false;

    }
    public override void EndAction()
    {
        // do smth at state end
        // call from parent node if priority node returns success?
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        for (int i = 0; i < children.Count; ++i)
        {
            curState = children[i].exec();
            //Debug.Log(children[i] + ": " + curState);
            if (curState != State.FAILURE)
            {
                //status.CloseOthers(children[i]); // Close nodes on open list
                return curState;
            }
        }
        
        return curState;
    }
    public bool CheckConditions()
    {
        // redundant in default mode.
        return true;
    }
   
}