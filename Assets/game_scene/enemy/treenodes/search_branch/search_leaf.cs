using UnityEngine;
using System.Collections;

public class search_leaf : general_node {

    // constructor for setup
    enemy_script cur_actor;
    public search_leaf(tree_script world_status)
    {
        status = world_status;        
        curState = State.FAILURE;
        instance = this;
    }
    public override void Open(enemy_script actor)
    {
        if (!status.open_nodes.Contains(instance)) 
            status.open_nodes.Add(instance);
        base.Open(actor);
    }
    public override void Close()
    {        
        base.Close();
    }
    public override void StartAction() 
    {
        cur_actor.ResetSearchTimer();
        //Debug.Log("timer reset");
    }
    public override void EndAction()
    {
        curState = State.SUCCESS;
        //MESSAGE.print("", -100, -60, 12, 3000);        
    }
    public override State Tick(enemy_script actor)
    {
        cur_actor = actor;
        //MESSAGE.print("search", -100, -70, 12, 2000);
        if (curState != State.RUNNING)
            StartAction();
        if (actor.isnotnear()) ///// TODO: Better condition!!!!                               
        {
            curState = State.RUNNING;            
            actor.SetDirectionToPlayer(); 
        }
        else EndAction();

        return curState;
    }

}
