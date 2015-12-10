using UnityEngine;
using System.Collections;

public class is_visible : general_node {

    // constructor for setup (not relying on Unity's Start or Awake)
    public is_visible(tree_script world_status)
    {
        status = world_status;        
        curState = State.FAILURE;
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
        if (actor.GetPlayerSighted())
        {
            
            curState = State.SUCCESS;
            actor.searching = true;
        }
        else curState = State.FAILURE;

        return curState;
    }
}
