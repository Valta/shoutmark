using UnityEngine;
using System.Collections;

// NB! This actually does nothing.
public class chase_leaf : general_node {

    // constructor for setup (not relying on Unity's Start or Awake)
    public chase_leaf(tree_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        curState = State.FAILURE;
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
        // do smth at state beginning
        // set move direction
        Debug.Log("chase start");
        //status.CloseOthers(this);
        actor.SetLooking(false);
        curState = State.RUNNING;
        
    }
    public override void EndAction()
    {

        Debug.Log("chase end");

        curState = State.SUCCESS;
    }
    public override State Tick()
    {

        MESSAGE.print("chase", -100, -70, 12, 2000);
        if (curState != State.RUNNING)
            StartAction();

        return curState;
    }

}
