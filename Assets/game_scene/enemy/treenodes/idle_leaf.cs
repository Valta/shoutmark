using UnityEngine;
using System.Collections;

public class idle_leaf : GeneralNode {

    // constructor for setup (not relying on Unity's Start or Awake)
    public idle_leaf(treeroot_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        curState = State.SUCCESS;
    }
    // presumably we need these. Use when found out where.
    public override void Open()
    {
        open = true;
    }
    public override void Close()
    {
        EndAction();
        base.Close();
    }
    public override void StartAction()
    {
        // do smth at state beginning
        // TODO: set default movement and radar
        Debug.Log("idlestart");
        curState = State.RUNNING;
        actor.SetRoaming(true);
    }
    public override void EndAction()
    {
        // do smth at state end
        curState = State.SUCCESS;
    }
    public override State Tick()
    {        
        if (CheckConditions())
        {
            //Debug.Log("idle");
            if (curState != State.RUNNING)
                StartAction();                
        }
        return curState;
    }
    public bool CheckConditions()
    {
        // redundant in default mode.
        return true;
    }
}
