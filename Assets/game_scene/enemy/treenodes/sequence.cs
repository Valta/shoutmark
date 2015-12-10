using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sequence : general_node
{


    // constructor for setup
    public sequence(tree_script world_status)
    {
        status = world_status;
        children = new List<general_node>();
        instance = this;
    }
    public override void Open(enemy_script actor)
    {
        if (!status.open_nodes.Contains(instance))
            status.open_nodes.Add(instance);
        base.Open(actor);
    }
    public override void Close()
    {
        base.Close();
    }
    public override void StartAction() { }
    public override void EndAction() { }
    public override State Tick(enemy_script actor)
    {

        for (int i = 0; i < children.Count; ++i)
        {
            curState = children[i].exec(actor);

            if (curState == State.FAILURE)
            {
                return curState;
            }
        }

        return curState;
    }
}
