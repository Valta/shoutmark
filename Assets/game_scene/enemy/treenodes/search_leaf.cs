using UnityEngine;
using System.Collections;

public class search_leaf : GeneralNode {

    // constructor for setup (not relying on Unity's Start or Awake)
    public search_leaf(treeroot_script world_status, enemy_script this_actor)
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
        //status.open_nodes.Remove(this);
        base.Close();
    }
    public override void StartAction()
    {
        Debug.Log("search");
        status.CloseOthers(this);
        // do smth at state beginning
        curState = State.RUNNING;
        // set destination to last player position
    }
    public override void EndAction()
    {
        // do smth at state end
        curState = State.SUCCESS;
        status.searching = false;
    }
    public override State Tick()
    {
        if (CheckConditions())
        {
            curState = State.SUCCESS;
            StartAction();
        }
        else curState = State.FAILURE;
        return curState;
    }
    public bool CheckConditions()
    {
        // player is out of sight AND last known position is not reached
        if (!status.player_sighted && status.searching)
            return true;
        return false;
    }
}
