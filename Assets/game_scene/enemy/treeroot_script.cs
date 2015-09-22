using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treeroot_script : MonoBehaviour {

    public GeneralNode rootnode;
    radar_script radar; // TODO: diff radar for enemies?
    enemy_script actor;
    private const float ATTACK_RANGE = 2.0f;
    private const float GOSSIP_RANGE = 1.0f; // Has to be more than enemy radius * 2!

    public Vector3 _position;
    public Vector3 player_last_seen;
    public List<Vector3> friends_last_seen;
    public Vector3 closest_friend;

    public bool player_sighted;
    public bool player_in_range;
    public bool friend_close;
    public bool can_attack = true;
    public bool will_talk;
    public bool searching;

    // values defined in leaf nodes
    public float last_attack;
    public float last_gossip;

    void Start()
    {        
        radar = this.gameObject.GetComponent<radar_script>();
        friends_last_seen = new List<Vector3>();
        actor = this.gameObject.GetComponent<enemy_script>();
        rootnode = new main_selector(this, actor);
    }

    void Update()
    {        
        // Update world state

        _position = this.transform.position;
        // clear friends info
        friends_last_seen.Clear();
        closest_friend.x = 100.0f;
        closest_friend.y = 100.0f;
        // get friends list from radar (TODO: friends list in radar)
        // get player sighted from radar


        if (_TIMER.time() - last_attack > 1.0f)
            can_attack = true;
		if (_TIMER.time() - last_gossip > 2.0f)
        {
            will_talk = true;
           foreach (Vector3 v in friends_last_seen)
           {
               float distance = Vector3.Distance(_position, v);

               if (distance < GOSSIP_RANGE)
                   friend_close = true;
               if (distance < Vector3.Distance(_position, closest_friend))
                   closest_friend = v;               
           }
        }
        player_sighted = radar.seeing_player();        
        player_last_seen = radar.last_player_position();
        player_in_range = Vector3.Distance(_position, player_last_seen) < ATTACK_RANGE;

        // 
        // Update behavior
        rootnode.Tick();
    }

}
