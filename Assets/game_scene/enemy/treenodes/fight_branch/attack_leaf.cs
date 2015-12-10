using UnityEngine;
using System.Collections;

public class attack_leaf : general_node {

    // constructor for setup (not relying on Unity's Start or Awake)
    public attack_leaf(tree_script world_status)
    {
        status = world_status;        
        curState = State.SUCCESS;
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

    }
    public override void EndAction()
    {

    }
    public override State Tick(enemy_script actor)
    {
        // TODO: Actual attack logic.
        MESSAGE.print("attack", -100, -70, 12, 2000);
        
        MESSAGE.report("penkele", 7);
        Hit(actor);
        return curState;
    }
    void Hit(enemy_script actor)
    {
        // TODO: laser attack! Also health should not be here (no can know if hit or miss).

        actor.hits++;
        if (actor.is_dead())
        {
            MESSAGE.print("GAME OVER...", -160, -90, 1, 65);
            _TIMER.set_pause(true);
        }



        //status.laser.shoot_beam(new Vector3(status.player_last_seen.x, 0, status.player_last_seen.y), new Vector3(status._position.x, 0, status._position.y));
        //status.last_attack = _TIMER.time();
    }
}
