using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class search_successor : GeneralNode
{
    List<GeneralNode> children;
    int running_child = 0;

    // constructor for setup (not relying on Unity's Start or Awake)
    public search_successor(treeroot_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<GeneralNode>();
        children.Add(new search_leaf(world_status, this_actor));
        children.Add(new lookout_leaf(world_status, this_actor));
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
        Debug.Log("search started");
        curState = State.RUNNING;
    }
    public override void EndAction()
    {
        Debug.Log("search ended");
        status.searching = false;
        running_child = 0;
        curState = State.FAILURE;
    }
    public override State Tick()
    {
        if (CheckConditions())
        {
            if (running_child >= children.Count)
                EndAction();
            else
            {
                //Debug.Log(children[running_child]);
                curState = children[running_child].exec();
                Debug.Log(children[running_child] + ": " + curState);
                if (curState == State.SUCCESS)
                {
                    running_child++;
                    
                    //Debug.Log("running child moved " + children[running_child]);
                }
            }  
        }
        else curState = State.FAILURE;
        return curState;
    }
    public bool CheckConditions()
    {
        if (!status.player_sighted && status.searching)
            return true;
        return false;
    }

}