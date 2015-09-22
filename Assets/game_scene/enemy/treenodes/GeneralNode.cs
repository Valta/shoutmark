using UnityEngine;
using System.Collections;

public enum State
{
    SUCCESS,
    FAILURE,
    RUNNING,
    ERROR
}

public abstract class GeneralNode
{
    public bool open;
    public State curState;
    protected enemy_script actor;
    protected treeroot_script status;

    public virtual void Open()
    {
        open = true;
    }
    public virtual void Close()
    {
        open = false;
    }
    public abstract void StartAction();
    public abstract void EndAction();
    public abstract State Tick();

}
