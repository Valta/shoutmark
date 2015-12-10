using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum State
{
    SUCCESS,
    FAILURE,
    RUNNING
    //ERROR // so far not used.
}

public struct sequence_memory_data
{
    public int running_child;
    public State current_state;
}

public abstract class general_node
{
    protected State curState;
    protected tree_script status;
    protected general_node instance;

    // not defined for leaf nodes
    protected List<general_node> children;

    public virtual void Open(enemy_script actor)
    {
        //Debug.Log("adding to list: " + instance);

        // Nodes are on last open list only if they are still running!
        if (actor.open_nodes.Contains(instance))
            curState = State.RUNNING;
    }
    public virtual void Close()
    {
        //Debug.Log("removing from list: " + instance);
        status.open_nodes.Remove(instance);

    }
    public abstract void StartAction(); // call if not on open-list?
    public abstract void EndAction(); // Do stuff needed to exit 
    public abstract State Tick(enemy_script actor);

    // 
    public State exec(enemy_script actor)
    {
        Open(actor);
        
        if (Tick(actor) != State.RUNNING) // Tick modifies curState anyway
                Close();
        
        return curState;
    }

    public void add_child_node(general_node child)
    {
        if (children == null)
        {
            Debug.Log("Error! Trying to add child to leaf node");
        }
        else
        children.Add(child);
    }
}
