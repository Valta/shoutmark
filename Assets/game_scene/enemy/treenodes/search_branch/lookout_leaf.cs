using UnityEngine;
using System.Collections;

// TODO: Fix!
// Has to use startaction to define dest angle!
// Has to ask enemy instance is it running!
public class lookout_leaf : general_node
{
    enemy_script cur_actor;
    // constructor for setup
    public lookout_leaf(tree_script world_status)
    {
        status = world_status;
        curState = State.FAILURE;
        instance = this;
    }
    // presumably we need these. Use when found out where.
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
        // stop movement while turning around
        cur_actor.SetLooking(true);        
        curState = State.RUNNING;
        // get current angle and set target angle at full circle
        cur_actor.current_angle_in_degrees = cur_actor.GetDirectionAngle();
        cur_actor.end_angle = cur_actor.current_angle_in_degrees + 360.0f;
        
    }
    public override void EndAction()
    {

        // release movement
        cur_actor.SetLooking(false);
        cur_actor.searching = false;
        curState = State.SUCCESS;
    }
    public override State Tick(enemy_script actor)
    {
        cur_actor = actor;
        //MESSAGE.print("lookout", -100, -70, 12, 2000);

        actor.current_angle_in_degrees += _TIMER.deltatime() * 90.0f;
        if (curState != State.RUNNING)
            StartAction();
        else if (actor.current_angle_in_degrees >= actor.end_angle)
        {
            EndAction();
        }
        else actor.SetDirection(actor.current_angle_in_degrees);
        
        return curState;
    }
}
