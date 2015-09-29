using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class fight_selector : GeneralNode {

    List<GeneralNode> children;

    // constructor for setup 
    public fight_selector(treeroot_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<GeneralNode>();
        children.Add(new attack_leaf(world_status, this_actor));
        children.Add(new chase_leaf(world_status, this_actor));
        curState = State.FAILURE;
    }
    // presumably we need these. Use when found out where.
    //public override void Open()
    //{
    //    open = true;
    //}
    //public override void Close()
    //{
    //    open = false;
    //}
    public override void StartAction()
    {
        // do smth at state beginning
        // TODO: lock on target
    }
    public override void EndAction()
    {
        // do smth at state end
        // TODO: free radar & searching = true

        //status.searching = true;
        //curState = State.SUCCESS;
    }
    public override State Tick()
    {
        if (CheckConditions())
        {
            actor.SetDirection(status.player_last_seen.x, status.player_last_seen.y);
            for (int i = 0; i < children.Count; ++i)
            {
                curState = children[i].exec();
                
                if (curState != State.FAILURE)
                {                    
                    return curState;
                }
            }
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
}
