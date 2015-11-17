using UnityEngine;
using System.Collections;

public class lookout_leaf : general_node
{
    float end_angle;
    float current_angle_in_degrees;
    // constructor for setup (not relying on Unity's Start or Awake)
    public lookout_leaf(tree_script world_status, enemy_script this_actor)
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
        // TODO: set default movement and radar
        //Debug.Log("lookout start");

        // stop movement while turning around
        actor.SetLooking(true);        
        curState = State.RUNNING;
        current_angle_in_degrees = actor.GetDirectionAngle();
        end_angle = current_angle_in_degrees + 360.0f;
    }
    public override void EndAction()
    {
        //Debug.Log("lookout end");

        // release movement
        actor.SetLooking(false);
        status.searching = false;
        curState = State.SUCCESS;
    }
    public override State Tick()
    {
        MESSAGE.print("lookout", -100, -70, 12, 2000);
        
        current_angle_in_degrees += _TIMER.deltatime() * 90.0f;
        if (curState != State.RUNNING)
            StartAction();
        else if (current_angle_in_degrees >= end_angle)
        {
            EndAction();
        }
        else actor.SetDirection(current_angle_in_degrees);
        
        return curState;
    }
}
