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
    public abstract State Tick();
}
