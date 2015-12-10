using UnityEngine;
using System.Collections;

public class gossip_leaf : general_node {
       
    // constructor for setup
    public gossip_leaf(tree_script world_status, enemy_script actor)
    {
        status = world_status;
        curState = State.FAILURE;
        instance = this;
    }
    // presumably we need these. Use when found out where.
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
    public override void StartAction()
    {

    }
    public override void EndAction()
    {

    }
    public override State Tick(enemy_script actor)
    {
 
        return curState;
    }

}
