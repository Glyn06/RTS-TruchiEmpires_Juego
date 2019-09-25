using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeAction : Nodo
{
    //running, ok, fail
    private void Start()
    {
        tipoNodo = "Action";
        state = NodeState.running;
    }
    public void Action()
    {
        if(state == NodeState.running)
        {
                   

            
        }
       
    }
}
