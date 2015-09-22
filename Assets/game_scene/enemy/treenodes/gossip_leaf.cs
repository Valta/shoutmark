using UnityEngine;
using System.Collections;

public class gossip_leaf : GeneralNode {

    float gossiptimer;
    
    // constructor for setup (not relying on Unity's Start or Awake)
    public gossip_leaf(treeroot_script world_status, enemy_script this_actor)
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
        EndAction();
        base.Close();
    }
    public override void StartAction()
    {
        // do smth at state beginning
        // TODO: stop movement and slow radar
        curState = State.RUNNING;
        gossiptimer = _TIMER.time();
        
    }
    public override void EndAction()
    {
        // do smth at state end
        // call from parent node if priority node returns success?
        status.last_gossip = _TIMER.time();
        curState = State.SUCCESS;
    }
    public override State Tick()
    {

        if (CheckConditions())
        {
            Debug.Log("gossip");
            if (curState != State.RUNNING)
            {                
                {
                    StartAction();
                }
            }
            else if (_TIMER.time() - gossiptimer > 2) ///// TODO: master timer!
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
        if (status.friend_close && status.will_talk)
            return true;
        else return false;
    }
}
