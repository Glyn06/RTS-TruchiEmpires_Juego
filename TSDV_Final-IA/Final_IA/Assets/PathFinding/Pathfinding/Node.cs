using UnityEngine;
public class Node : MonoBehaviour
{
    public NodeValue nodeValue;
    public bool taken = false;
    Node predecesor = null;  
    public NodeState nodeState;
    NodeAdy[] ady;
    private float cost;
    private float totalCost;
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
    public float GetCost()
    {
        return cost;
    }
    public void SetCost(float _cost)
    {
        cost = _cost;
    }
    public void SetTotalCost(float _totalCost)
    {
        totalCost = _totalCost;
    }
    public float GetTotalCost()
    {
        return totalCost;
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