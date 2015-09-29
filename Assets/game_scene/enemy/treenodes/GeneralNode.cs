using UnityEngine;
using System.Collections;

public enum State
{
    SUCCESS,
    FAILURE,
    RUNNING
    //ERROR // so far not used.
}

public abstract class GeneralNode
{
    public bool open;
    protected State curState;
    protected enemy_script actor;
    protected treeroot_script status;
    protected GeneralNode instance;

    public virtual void Open()
    {
        open = true;
        
    }
    public virtual void Close()
    {
        status.open_nodes.Remove(instance);
        open = false;
        // remove from open list
        // call endaction
    }
    public abstract void StartAction(); // call if not on open-list?
    public abstract void EndAction(); // Do stuff needed to exit 
    public abstract State Tick();

    // 
    public State exec()
    {
        Open();
        
        if (Tick() != State.RUNNING) // Tick modifies curState anyway
                Close();
        
        return curState;
    }

}
