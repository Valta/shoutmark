using UnityEngine;
using System.Collections;

public class lock_on_target_leaf : general_node {

    // constructor for setup (not relying on Unity's Start or Awake)
    public lock_on_target_leaf(tree_script world_status)
    {
        status = world_status;        
        curState = State.SUCCESS;
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
    public override void StartAction() { }
    public override void EndAction() { }
    public override State Tick(enemy_script actor)
    {
        actor.SetDirectionToPlayer();
        actor.searching = true;
        //actor.SetLooking(false);
        // curState is always SUCCESS
        // TODO: define when failed
        return curState;
    }
}
