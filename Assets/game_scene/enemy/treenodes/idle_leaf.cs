using UnityEngine;
using System.Collections;

public class idle_leaf : GeneralNode {

    // constructor for setup (not relying on Unity's Start or Awake)
    public idle_leaf(treeroot_script world_status, enemy_script this_actor)
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
        actor.SetRoaming(true);
    }
    public override void EndAction()
    {
        
        //actor.SetRoaming(false);
        // not to be left running, never fails (idle is default behavior)
        Debug.Log("idle end called");
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        
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
