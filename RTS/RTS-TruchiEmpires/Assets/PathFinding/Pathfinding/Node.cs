using UnityEngine;

public class Node : MonoBehaviour
{
    public NodeValue nodeValue;
    public bool taken = false;
    Node predecesor = null; 
    public NodeState nodeState;
    NodeAdy[] ady;
    private int G_cost; //costo de la distancia del comienzo del nodo.
    private int H_cost; //costo de la distancia del final del nodo;
    private int F_cost; // G_cost + H_cost;
    bool isObstacle = false;

    void Awake()
    {
        ady = new NodeAdy[(int)AdyDirection.Count];

        for (int i = 0; i < ady.Length; i++)
        {
            ady[i].node = null;
            ady[i].type = NodeAdyType.Straight;
        }
    }
    
    public void AddAdyNode(Node node, NodeAdyType type, AdyDirection direction)
    {
        ady[(int)direction].node = node;
        ady[(int)direction].type = type;
        
    }

    public NodeAdy[] GetNodeAdyacents()
    {
        return ady;
    }

    public bool IsObstacle
    {
        get { return isObstacle; }
        set
        {
            isObstacle = value;
        }
    }

    public Node Predecesor
    {
        get { return predecesor;  }
        set { predecesor = value; }
    }
    public void SetG_Cost(int g_cost)
    {
        G_cost = g_cost;
    }
    public void SetH_Cost(int h_cost)
    {
        H_cost = h_cost;
    }
    public int GetG_Cost()
    {
        return G_cost;
    }
    public int GetH_Cost()
    {
        return H_cost;
    }
    public int GetF_Cost()
    {
        F_cost = 0;
        F_cost = H_cost + G_cost;
        return F_cost;
    }
    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Aldeano":
                IsObstacle = true;
                break;
            case "Centro Urbano":
                IsObstacle = true;
                break;
            case "Mineral":
                IsObstacle = true;
                break;
                
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Aldeano":
                IsObstacle = false;
                break;
            case "Centro Urbano":
                IsObstacle = false;
                break;
            case "Mineral":
                IsObstacle = false;
                break;
        }
    }
}