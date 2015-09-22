using UnityEngine;
using System.Collections;

public class idle_leaf : GeneralNode {

    public State curState = State.SUCCESS; // init as success bc all is possible
    treeroot_script status;

    // constructor for setup (not relying on Unity's Start or Awake)
    public idle_leaf(treeroot_script world_status)
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
        //info.ChangeText("idling...");
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
