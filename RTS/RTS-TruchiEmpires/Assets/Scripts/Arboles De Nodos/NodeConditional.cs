using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConditional : Nodo
{
    //if ok returns fail, else returns ok
    private void Start()
    {
        tipoNodo = "Conditional";
    }
    public int Conditional()
    {
        switch (state)
        {
            case NodeState.ok:
                return (int)NodeState.ok;
            case NodeState.running:
                return (int)NodeState.fail;
            case NodeState.fail:
                return (int)NodeState.fail;
        }
        return (int)NodeState.fail;           //fail
    }
}
