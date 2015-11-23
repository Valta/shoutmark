using UnityEngine;
using System.Collections;

public class is_in_range : general_node {

    // constructor for setup (not relying on Unity's Start or Awake)
    public is_in_range(tree_script world_status, enemy_script this_actor)
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
        if (status.player_in_range)
        {
            MESSAGE.print("attack", -100, -70, 12, 2000);
            curState = State.SUCCESS;
        }
        else curState = State.FAILURE;

        return curState;
    }
}
