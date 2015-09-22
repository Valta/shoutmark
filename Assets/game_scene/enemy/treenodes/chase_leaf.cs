using UnityEngine;
using System.Collections;

public class chase_leaf : GeneralNode {

    public State curState = State.FAILURE; // init as success bc all is possible
    treeroot_script status;

    // constructor for setup (not relying on Unity's Start or Awake)
    public chase_leaf(treeroot_script world_status)
    {
        status = world_status;
        
    }
    // presumably we need these. Use when found out where.
    public void Open()
    {
        
    }
    public void Close()
    {
        
    }
    public void StartAction()
    {
        // do smth at state beginning
        
        //info.ChangeText("Chasing!...");
    }
    public void EndAction()
    {
        // do smth at state end
        curState = State.SUCCESS;
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
        if (status.player_sighted && !status.player_in_range)
            return true;
        return false;
    }
}
