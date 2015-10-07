using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class search_successor : general_node
{
    List<general_node> children;
    int running_child = 0;

    // constructor for setup (not relying on Unity's Start or Awake)
    public search_successor(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        children = new List<general_node>();
        children.Add(new search_leaf(world_status, this_actor));
        children.Add(new lookout_leaf(world_status, this_actor));
        curState = State.SUCCESS;
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
        Debug.Log("search started");
        status.CloseOthers(this);
        running_child = 0;
        curState = State.RUNNING;
    }
    public override void EndAction()
    {        
        children[running_child].EndAction(); 
        running_child = 0;
        status.searching = false;
        curState = State.FAILURE;
    }
    public override State Tick()
    {
        if (CheckConditions())
        {
            if (curState != State.RUNNING)
                StartAction();

            if (running_child >= children.Count)
            {
                running_child--;
                Debug.Log(instance + " ended sequence ");
                EndAction();
            }
            else
            {               
                State childState = children[running_child].exec();

                if (childState == State.SUCCESS)
                {
                    running_child++;
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