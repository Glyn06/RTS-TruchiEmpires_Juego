using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Nodo : MonoBehaviour
{
    public enum NodeState
    {
        none,
        running,
        ok,
        fail,
        count
    }
    public NodeState state;
    public string tipoNodo;
}
