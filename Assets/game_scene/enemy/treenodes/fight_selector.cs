using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class fight_selector : GeneralNode {

    public State curState = State.FAILURE; // init as success bc all is possible
    treeroot_script status;
    List<GeneralNode> children;

    // constructor for setup 
    public fight_selector(treeroot_script world_status)
    {
        status = world_status;
        children = new List<GeneralNode>();
        children.Add(new attack_leaf(world_status));
        children.Add(new chase_leaf(world_status));
        
    }
    // presumably we need these. Use when found out where.
    public void Open()
    {
       
    }
    public void Close()
    {
        
    }
    public void StartAction()
    {
        // do smth at state beginning
        // TODO: start idling
    }
    public void EndAction()
    {
        // do smth at state end
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        if (CheckConditions())
        {
            curState = RunChild(0);
            if (curState == State.FAILURE)
                curState = RunChild(1);
        }
        else curState = State.FAILURE;

        return curState;
    }
    public bool CheckConditions()
    {
        if (status.player_sighted)
            return true;
        return false;
    }
    State RunChild(int i)
    {
        return children[i].Tick();
    }
}
