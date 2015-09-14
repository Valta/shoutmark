using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{

    private static bool thisObjectHasSpawned = false;

    void Awake()
    {
        if (thisObjectHasSpawned == false)
        {
            thisObjectHasSpawned = true;
            
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            // This deletes new objects of same type when loading
            // new scenes but keeps the first one
            DestroyImmediate(gameObject);
        }
    }

}
