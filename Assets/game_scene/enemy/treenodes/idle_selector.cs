using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class idle_selector : GeneralNode
{
    List<GeneralNode> children;

    // constructor for setup (not relying on Unity's Start or Awake)
    public idle_selector(treeroot_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<GeneralNode>();
        children.Add(new gossip_leaf(world_status, this_actor));
        children.Add(new idle_leaf(world_status, this_actor));
        curState = State.SUCCESS;
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
            children[i].Open();
            curState = children[i].Tick();
            if (curState != State.FAILURE)
            {
                foreach (GeneralNode g in children)
                {
                    if (children[i] != g)
                        g.Close();
                }
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
    State RunChild(int i)
    {
        return children[i].Tick();
    }
    
}