using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public GameManager gm;
    private Node myNode;
    [HideInInspector]
    public bool nodoGenerado;

    public void CheckNodeCercano()
    {
        if (GameManager.instanceGameManager != null)
        {
            Debug.Log("Instancie GM");
            gm = GameManager.instanceGameManager;
        }
        if (gm != null && gm.generadorNodos.GetClosestNode(transform.position) != null)
        {
            Debug.Log("CREE MyNode");
            myNode = gm.generadorNodos.GetClosestNode(transform.position);
        }
    }
    // Update is called once per frame
    public Node GetMyNode()
    {
        return myNode;
    }
}
