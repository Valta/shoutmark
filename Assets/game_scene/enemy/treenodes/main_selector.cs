using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class main_selector : GeneralNode {


    List<GeneralNode> children;
    // constructor for setup (not relying on Unity's Start or Awake)
    public main_selector(treeroot_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<GeneralNode>();
        children.Add(new fight_selector(world_status, this_actor));
        children.Add(new search_leaf(world_status, this_actor));
        children.Add(new idle_selector(world_status, this_actor));
        
    }
    // presumably we need these. Use when found out where.
    //public override void Open()
    //{
    //    base.Open();
    //}
    //public override void Close()
    //{
    //    base.Close();
    //}
    public override void StartAction()
    {
        // do smth at state beginning

    }
    public override void EndAction()
    {
        // do smth at state end
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        // TODO: loop!

        for (int i = 0; i < children.Count; ++i)
        {
            
            children[i].Open();
            curState = children[i].Tick();
            if (curState != State.FAILURE)
                return curState;           
        }
        //    curState = RunChild(0);
        //if (curState == State.FAILURE)
        //    curState = RunChild(1);
        //// only another node can interfere.

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
