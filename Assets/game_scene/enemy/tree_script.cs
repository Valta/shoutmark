using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tree_script : MonoBehaviour {

    public general_node rootnode;

    // fight branch nodes
    private general_node check_visible;
    private general_node lock_on;
    private general_node return_success;
    private general_node in_range_check;
    private general_node cooldown_check;
    private general_node attack;
    private general_node attack_sequence;
    private general_node fight_sequence;

    // search branch nodes
    private general_node searching_check;
    private general_node go_to_spot;
    private general_node look_around;
    private general_node search_routine;
    private general_node search_sequence;

    // idle branch nodes
    private general_node idle;

    // open list
    public List<general_node> open_nodes;

    void Start()
    {
        InitiateNodes();
        open_nodes = new List<general_node>();
    }

    void Update()
    {        

        
        //// clear friends info
        //friends_last_seen.Clear();
        //closest_friend.x = 100.0f;
        //closest_friend.y = 100.0f;
        //// get friends list from radar (TODO: friends list in radar)
        //// get player sighted from radar


        //if (_TIMER.time() - last_gossip > 2.0f)
        //{
        //    will_talk = true;
        //   foreach (Vector3 v in friends_last_seen)
        //   {
        //       float distance = Vector3.Distance(_position, v);

        //       if (distance < GOSSIP_RANGE)
        //           friend_close = true;
        //       if (distance < Vector3.Distance(_position, closest_friend))
        //           closest_friend = v;               
        //   }
        //}


        
    }
    // Alternate update / traversal
    // TODO: rewrite enemy_script to keep track of necessary data (or separate class? why?)
    // TODO: use actor as function parameter!
    // previous open list saved to actor, current open in update
    // NB! All non-const data saved in actor instance, NOT in nodes!
    public void UpdateAI(enemy_script actor)
    {
        open_nodes.Clear();
        rootnode.Tick(actor);
        //printlist();
        //printlastlist(actor);
        ClosePrevious(actor);
        actor.SetOpenList(open_nodes);
    }
    void printlist()
    {
        Debug.Log("open list:");
        foreach (general_node g in open_nodes)
            Debug.Log(g);
    }
    void printlastlist(enemy_script actor)
    {
        Debug.Log("last open list:");
        foreach (general_node g in actor.open_nodes)
            Debug.Log(g);
    }
    // Close nodes previously left open, if priority branch interrupts
    // Called after every tick    
    private void ClosePrevious(enemy_script _actor)
    {
        for (int i = 0; i < _actor.open_nodes.Count; ++i)
        {
            if (i < open_nodes.Count)
            {
                if (open_nodes[i] != _actor.open_nodes[i])
                {
                    Debug.Log("ending node " + _actor.open_nodes[i]);
                    _actor.open_nodes[i].EndAction();
                }
            }
            else _actor.open_nodes[i].EndAction();
        }        
    }
    // Construct a tree.
    private void InitiateNodes()
    {
        // fight branch
        in_range_check = new is_in_range(this);
        cooldown_check = new is_cooled_down(this);
        attack = new attack_leaf(this);
        attack_sequence = new sequence(this);
        attack_sequence.add_child_node(in_range_check);
        attack_sequence.add_child_node(cooldown_check);
        attack_sequence.add_child_node(attack);
        return_success = new always_success(this);
        return_success.add_child_node(attack_sequence);
        check_visible = new is_visible(this);
        lock_on = new lock_on_target_leaf(this);
        fight_sequence = new sequence(this);
        fight_sequence.add_child_node(check_visible);
        fight_sequence.add_child_node(lock_on);
        fight_sequence.add_child_node(return_success);

        // search branch
        go_to_spot = new search_leaf(this);
        look_around = new lookout_leaf(this);
        search_routine = new sequence_memory(this);
        search_routine.add_child_node(go_to_spot);
        search_routine.add_child_node(look_around);
        searching_check = new is_searching(this);
        search_sequence = new sequence(this);
        search_sequence.add_child_node(searching_check);
        search_sequence.add_child_node(search_routine);

        // idle
        idle = new idle_leaf(this);

        // root
        rootnode = new selector(this);
        rootnode.add_child_node(fight_sequence);
        rootnode.add_child_node(search_sequence);
        rootnode.add_child_node(idle);
    }
}
