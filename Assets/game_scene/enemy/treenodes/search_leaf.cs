using UnityEngine;
using System.Collections;

public class search_leaf : GeneralNode {

    // constructor for setup (not relying on Unity's Start or Awake)
    public search_leaf(treeroot_script world_status, enemy_script this_actor)
    {
        status = world_status;
        actor = this_actor;
        curState = State.FAILURE;
    }
    // presumably we need these. Use when found out where.
    public override void Open()
    {
        open = true;
    }
    public override void Close()
    {
        open = false;
        curState = State.SUCCESS;
    }
    public override void StartAction()
    {
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
            Debug.Log("search");
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
