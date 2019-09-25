using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConditional : Nodo
{
    //if ok returns fail, else returns ok
    public int Conditional()
    {
        if (state == NodeState.fail)
        {
            return 2;       //ok
        }
        return 3;           //fail
    }
}
