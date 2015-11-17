using UnityEngine;
using System.Collections;

public class is_cooled_down : general_node {

    float timer;
    const float COOLDOWN_TIME = 3.0f;
    // constructor for setup (not relying on Unity's Start or Awake)
    public is_cooled_down(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        curState = State.SUCCESS;
        timer = 0;
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
        timer -= _TIMER.deltatime();
        if (timer <= 0)
        {
            timer = COOLDOWN_TIME;
            curState = State.SUCCESS;
        }
        else curState = State.FAILURE;

        return curState;
    }
}
