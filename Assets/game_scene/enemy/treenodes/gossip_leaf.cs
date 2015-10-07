using UnityEngine;
using System.Collections;

public class gossip_leaf : general_node {

    float gossiptimer;
    
    // constructor for setup (not relying on Unity's Start or Awake)
    public gossip_leaf(tree_script world_status, enemy_script this_actor)
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
        // do smth at state beginning
        // TODO: stop movement and slow radar
        Debug.Log("gossip start");
        status.CloseOthers(this);
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
            MESSAGE.print("gossip", -100, -70, 12, 2000);
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
