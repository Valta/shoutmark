using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class always_success : general_node {

    public always_success(tree_script world_status)
    {
        status = world_status;        
        curState = State.SUCCESS;
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
        children[0].exec(actor);
        return State.SUCCESS;
    }
}
