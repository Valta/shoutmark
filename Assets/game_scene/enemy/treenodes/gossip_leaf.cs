using UnityEngine;
using System.Collections;

public class gossip_leaf : GeneralNode {

    public State curState = State.FAILURE; // init as failure so only default gets through
    treeroot_script status;

    float gossiptimer = 0;
    float idletimer = 0;
    // constructor for setup (not relying on Unity's Start or Awake)
    public gossip_leaf(treeroot_script world_status)
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
        // start moving in random direction here / roaming coroutine?
        curState = State.RUNNING;
        gossiptimer = 2;
        //info.ChangeText("blah blah blah ...");

    }
    public void EndAction()
    {
        // do smth at state end
        // call from parent node if priority node returns success?
        idletimer = 2;
        curState = State.SUCCESS;
    }
    public override State Tick()
    {

        if (CheckConditions())
        {
            if (curState != State.RUNNING)
            {                
                {
                    StartAction();
                }
            }
            else if (gossiptimer == 0) ///// TODO: master timer!
            {
                EndAction();
            }
        }
        else curState = State.FAILURE;
        // only another node can interfere. 
        return curState;
    }
    public bool CheckConditions()
    {
        if (status.friend_close && idletimer == 0)
            return true;
        else return false;
    }
}
