using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class main_selector : GeneralNode {

    public State curState = State.FAILURE; // init as success bc all is possible

    treeroot_script status;
    List<GeneralNode> children;
    // constructor for setup (not relying on Unity's Start or Awake)
    public main_selector(treeroot_script world_status)
    {
        status = world_status;
        children = new List<GeneralNode>();
        children.Add(new fight_selector(world_status));
        children.Add(new idle_selector(world_status));
        
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

    }
    public void EndAction()
    {
        // do smth at state end
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        curState = RunChild(0);
        if (curState == State.FAILURE)
            curState = RunChild(1);
        // only another node can interfere.

        return curState;
    }
    public bool CheckConditions()
    {
        return true;        
    }
    State RunChild(int i)
    {
        return children[i].Tick();
    }
}
