using UnityEngine;
using System.Collections;

public class lock_on_target_leaf : general_node {

    // constructor for setup (not relying on Unity's Start or Awake)
    public lock_on_target_leaf(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
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

    }
    public override void EndAction()
    {

    }
    public override State Tick()
    {
        actor.SetDirection(status.player_last_seen.x, status.player_last_seen.y);
        status.searching = true;
        //actor.SetLooking(false);
        // curState is always SUCCESS
        // TODO: define when failed
        return curState;
    }
}
