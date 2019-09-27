using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeFather : Nodo
{
    // https://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior_trees_for_AI_How_they_work.php
    List<Nodo> nodes;
    private void Start()
    {
        tipoNodo = "NodeFather";
    }
    public void CheckNodes()
    {
        foreach (Nodo n in nodes)
        {
            switch (n.tipoNodo)
            {
                case "Action":
                    //llamar metodo action, ver como referenciarlo aca? (algunos ej usan delegate)
                    break;
                case "Conditional":

                    break;
                case "Decorator":

                    break;
                case "Sequencer":

                    break;
            }            
        }
    }
}
