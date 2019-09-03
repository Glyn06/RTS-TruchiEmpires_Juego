using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    static List<Node> path = new List<Node>();
    static List<Node> openNodes = new List<Node>();
    static List<Node> closeNodes = new List<Node>();

    public List<Node> GetPath(Node start, Node finish, PathfinderType pfT)
    {
        OpenNode(start, null);
        Node node;
        while (openNodes.Count > 0)
        {
            node = GetOpenNode(pfT);
            if (node == finish)
            {
                path = CreatePath(finish);
                return path;
            }
            CloseNode(node);
            OpenAdyNodes(node);
        }
        //path = CreatePath(finish);
        return path;
    }

    static void CloseNode(Node node)
    {
        if (node != null)
        {
            node.nodeState = NodeState.Close;
            openNodes.Remove(node);
            closeNodes.Add(node);
        }
    }

    static void OpenNode(Node node, Node opener)
    {
        if (node != null)
        {
            openNodes.Add(node);
            node.nodeState = NodeState.Open;
            node.Predecesor = opener;
        }
        
    }

    static void OpenAdyNodes(Node node)
    {
        NodeAdy[] adyNodes = node.GetNodeAdyacents();

        for (int i = 0; i < adyNodes.Length; i++)
        {
            if (adyNodes[i].node != null)
            {
                if (adyNodes[i].node && !adyNodes[i].node.IsObstacle && adyNodes[i].node.nodeState == NodeState.Ok)
                {
                    OpenNode(adyNodes[i].node, node);
                    //Debug.Log("ESTOY RECORRIENDO ADYACENTES Y AGREGANDO ABIERTOS");
                }
            }
        }
    }

    static Node GetOpenNode(PathfinderType pfT)
    {
        Node node = null;

        switch(pfT)
        {
            case PathfinderType.BreadthFirst:
                node = openNodes[0];
            break;

            case PathfinderType.DepthFirst:
                node = openNodes[openNodes.Count - 1];
            break;

            case PathfinderType.Star:
                
            break;
        }

        return node;
    }
    static List<Node> CreatePath(Node nodo)
    {
        List<Node> auxPath = new List<Node>();
        do
        {
            if (nodo != null)
            {
                auxPath.Insert(0, nodo);
                nodo = nodo.Predecesor;
            }
        } while (nodo != null);

        return auxPath;
    }
    

    public void CleanNodes()
    {
        while(openNodes.Count > 0)
        {
            openNodes[0].nodeState = NodeState.Ok;
            openNodes[0].Predecesor = null;
            openNodes.RemoveAt(0);
        }

        while(closeNodes.Count > 0)
        {
            closeNodes[0].nodeState = NodeState.Ok;
            closeNodes[0].Predecesor = null;
            closeNodes.RemoveAt(0);
        }
    }
}
