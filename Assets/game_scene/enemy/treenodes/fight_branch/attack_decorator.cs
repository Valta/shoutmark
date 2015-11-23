using UnityEngine;
using System.Collections;

public class attack_decorator : general_node {

    general_node child;

    public attack_decorator(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        curState = State.SUCCESS;
        child = new attack_sequence(world_status, this_actor);
        instance = this;
    }
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
    public override void StartAction() { }

    public override void EndAction() { }

    public override State Tick()
    {
        child.exec();
        return State.SUCCESS;
    }
}
