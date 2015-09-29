using UnityEngine;
using System.Collections;

public class attack_leaf : GeneralNode {

    float animationtimer;
    float cooldowntimer;
    bool hasAttacked = false;
    // constructor for setup (not relying on Unity's Start or Awake)
    public attack_leaf(treeroot_script world_status, enemy_script this_actor)
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
        Debug.Log("attack");
        MESSAGE.report("pois eestä", 7);
        status.CloseOthers(this);
        curState = State.RUNNING;
        animationtimer = 2;
        cooldowntimer = 4;
        hasAttacked = false;
        
    }
    public override void EndAction()
    {
        if (!status.player_sighted)
            status.searching = true;
        Debug.Log("attack ended");
        curState = State.FAILURE;
    }
    public override State Tick()
    {
        
        if (CheckConditions())
        {
            //Debug.Log("fight");
            if (curState != State.RUNNING)
            {
                StartAction();
            }
            else
            {
                animationtimer -= Time.deltaTime;
                cooldowntimer -= Time.deltaTime;
                // TODO: change to use master timer.
                if (animationtimer <= 0 && hasAttacked == false)
                    Hit();
                else if (cooldowntimer <= 0)
                    curState = State.SUCCESS;
            }
        }
        else curState = State.FAILURE;
        
        return curState;
    }
    public bool CheckConditions()
    {
        if (status.player_in_range)
            return true;
        return false;
    }
    void Hit()
    {
        //if (Chance(70))
        //    info.ChangeText("Hit!");
        //else
        //    info.ChangeText("Miss!");
        Debug.Log("hit");
        //MESSAGE.report("penkele", 7);
        hasAttacked = true;
    }

}
