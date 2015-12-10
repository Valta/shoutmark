using UnityEngine;
using System.Collections;

public class is_cooled_down : general_node {

    
    // constructor for setup (not relying on Unity's Start or Awake)
    public is_cooled_down(tree_script world_status)
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
        actor.attack_timer -= _TIMER.deltatime();
        if (actor.attack_timer <= 0)
        {
            actor.ResetCooldown();
            curState = State.SUCCESS;
        }
        else curState = State.FAILURE;

        return curState;
    }
}
