using UnityEngine;
using System.Collections;

public class idle_leaf : general_node {

    // constructor for setup (not relying on Unity's Start or Awake)
    public idle_leaf(tree_script world_status, enemy_script this_actor)
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
        //status.open_nodes.Remove(this);
        base.Close();
    }
    public override void StartAction()
    {
        // do smth at state beginning
        // TODO: set default movement and radar
        Debug.Log("idlestart");
        status.CloseOthers(this);
        curState = State.RUNNING;
        actor.SetLooking(false);
    }
    public override void EndAction()
    {
        
        
        // not to be left running, never fails (idle is default behavior)
        Debug.Log("idle end called");
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        MESSAGE.print("idle", -100, -70, 12, 2000);
        if (curState != State.RUNNING)
            StartAction();        
        return curState;
    }
    public bool CheckConditions()
    {
        // redundant in default mode.
        return true;
    }
}
