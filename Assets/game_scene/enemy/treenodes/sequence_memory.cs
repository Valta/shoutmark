using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sequence_memory : general_node
{
    private int data_id;
    enemy_script actor;
    // initialising methods
    public sequence_memory(tree_script world_status)
    {
        status = world_status;
        children = new List<general_node>();        
        instance = this;
    }

    // TODO: Use enums to keep track of right sequences (if there's more than one)
    public void set_data_id(int id)
    {
        data_id = id;
    }
    // tree traversal
    public override void Open(enemy_script cur_actor)
    {
        if (!status.open_nodes.Contains(instance))
            status.open_nodes.Add(instance);
        base.Open(cur_actor);
    }
    public override void Close()
    {
        base.Close();
    }
    public override void StartAction()
    {
        actor.set_running_child(0, data_id);
        curState = State.RUNNING;
    }
    public override void EndAction()
    {
        actor.set_running_child(0, data_id);
    }
    public override State Tick(enemy_script cur_actor)
    {
        actor = cur_actor;
        sequence_memory_data temp_data = actor.get_data(data_id);
        int curChild = temp_data.running_child;

        if (curState != State.RUNNING)
            StartAction();

        if (curChild >= children.Count)
        {
            curState = State.SUCCESS;
            EndAction();
        }
        else
        {
            State childState = children[curChild].exec(actor);

            if (childState == State.SUCCESS)
            {
                curChild++;
                actor.set_running_child(curChild, data_id);
            }
            if (childState == State.FAILURE)
            {
                curState = State.FAILURE;
                EndAction();
            }
        }
        return curState;
    }
}