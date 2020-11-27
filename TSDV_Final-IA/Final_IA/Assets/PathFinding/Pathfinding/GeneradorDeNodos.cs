using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeNodos : MonoBehaviour
{
    [Header("Configuracion Del Costo")]
    public float costNodePasto;
    public float costNodePiso;
    [Header("Variables Generales")]
    public GameObject Terrain;
    public GameObject cursor;
    public GameObject nodeObject;
    public float magnitud;
    public bool restaurarNodo;
    private int ancho = 1;
    private int largo = 1;
    private static Vector3 vector;
    private static List<List<Node>> listaNodos;
    private static Vector3 OriginalPosition;
    [HideInInspector]
    public bool NodesGenerates;
    int cantNodosCreados = 0;
    [SerializeField] private GameObject parentNodes;
    private void Awake()
    {
        restaurarNodo = false;
        ancho = (int)Terrain.transform.localScale.x;
        ancho = ancho + 1;
        largo = (int)Terrain.transform.localScale.z;
        largo = largo + 1;
        OriginalPosition = cursor.transform.position;
        listaNodos = new List<List<Node>>();

        for (int i = 0; i < ancho; i++)
        {
            listaNodos.Add(new List<Node>());
        }
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                listaNodos[i].Add(null);
            }
        }
    }
    void Start()
    {
        GenerarMapaDeNodos();
        SeteadorDeAdyasentes();
        NodesGenerates = true;
    }
    public void RestaorarNodo(Node stateOriginalNode, Node node)
    {
        node = stateOriginalNode;
    }
    private void GenerarMapaDeNodos()
    {
        Vector3 actualPos = OriginalPosition;

        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                
                RaycastHit hit;
                if (Physics.Raycast(actualPos, Vector3.down, out hit, actualPos.y))
                {
                    if (hit.collider.tag != "Obstaculo")
                    {
                        //Debug.Log("ENTRE");
                        Node node = Instantiate(nodeObject, new Vector3(actualPos.x, Terrain.transform.position.y+1, actualPos.z), Quaternion.identity, parentNodes.transform).GetComponent<Node>();
                        switch (hit.collider.tag)
                        {
                            case "Pasto":
                                node.SetCost(costNodePasto);
                                node.SetTotalCost(1);
                                break;
                            case "Piso":
                                node.SetCost(costNodePiso);
                                node.SetTotalCost(1);
                                break;
                        }
                        cantNodosCreados++;
                        if (hit.collider.tag == "Mineral" || hit.collider.tag == "Centro Urbano")
                        {
                            node.IsObstacle = true;
                        }
                        listaNodos[i][j] = node;
                    }
                }
                actualPos.x += 1.0f;
            }
            actualPos.x = OriginalPosition.x;
            actualPos.z += 1.0f;
            //transform.position = actualPos;
        }
        //Debug.Log("NODOS CREADOS: "+cantNodosCreados);
    }
    private void SeteadorDeAdyasentes() {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                if (listaNodos[i][j] != null)
                {
                    //Lados
                    if ((i + 1 < ancho && i + 1 < largo) && listaNodos[i + 1][j] != null)
                    {
                        listaNodos[i][j].AddAdyNode(listaNodos[i + 1][j], NodeAdyType.Straight, AdyDirection.Up);
                    }
                    if ((j + 1 < ancho && j + 1 < largo) && listaNodos[i][j + 1] != null)
                    {
                        listaNodos[i][j].AddAdyNode(listaNodos[i][j + 1], NodeAdyType.Straight, AdyDirection.Right);
                    }
                    if (i - 1 >= 0 && listaNodos[i - 1][j] != null)
                    {
                        listaNodos[i][j].AddAdyNode(listaNodos[i - 1][j], NodeAdyType.Straight, AdyDirection.Down);
                    }
                    if (j - 1 >= 0 && listaNodos[i][j - 1] != null)
                    {
                        listaNodos[i][j].AddAdyNode(listaNodos[i][j - 1], NodeAdyType.Straight, AdyDirection.Left);
                    }

                    //Diagonales
                    if ((i + 1 < ancho && i + 1 < largo) && (j + 1 < ancho && j + 1 < largo) && listaNodos[i + 1][j + 1] != null)
                    {
                        listaNodos[i][j].AddAdyNode(listaNodos[i + 1][j + 1], NodeAdyType.Diagonal, AdyDirection.UpRight);
                    }
                    if ((i + 1 < ancho && i + 1 < largo) && j - 1 >= 0 && listaNodos[i + 1][j - 1] != null)
                    {
                        listaNodos[i][j].AddAdyNode(listaNodos[i + 1][j - 1], NodeAdyType.Diagonal, AdyDirection.UpLeft);
                    }
                    if (i - 1 >= 0 && (j + 1 < ancho && j + 1 < largo) && listaNodos[i - 1][j + 1] != null)
                    {
                        listaNodos[i][j].AddAdyNode(listaNodos[i - 1][j + 1], NodeAdyType.Diagonal, AdyDirection.DownRight);
                    }
                    if (i - 1 >= 0 && j - 1 >= 0 && listaNodos[i - 1][j - 1] != null)
                    {
                        listaNodos[i][j].AddAdyNode(listaNodos[i - 1][j - 1], NodeAdyType.Diagonal, AdyDirection.DownLeft);
                    }
                }
            }
        }
    }
    public static List<List<Node>> GetListNodes()
    {
        return listaNodos;
    }
    public Node GetClosestNode(Vector3 pos)
    {
        //int x = (int)(pos.x - OriginalPosition.x); // -5.5 - -9.5 = -15
        //int y = (int)(pos.z - OriginalPosition.z);

        //return listaNodos[x-1][y-1];
        Vector3 diff = Vector3.zero;
        for (int i = 0; i < listaNodos.Count; i++)
        {
            for (int j = 0; j < listaNodos[i].Count; j++)
            {
                if (listaNodos[i][j] != null)
                {
                    Vector3 point = listaNodos[i][j].transform.position;
                    diff = point - pos;
                    if (listaNodos[i][j].transform.position.x == pos.x && listaNodos[i][j].transform.position.z == pos.z)
                    {
                        return listaNodos[i][j];
                    }
                    else if (diff.magnitude <= magnitud)
                    {
                        return listaNodos[i][j];
                    }
                }
            }
        }
        return null;
    }

    //private void OnDrawGizmos()
    //{
    //    if (listaNodos == null)
    //        return;

    //    for (int i = 1; i < listaNodos.Count ; i++)
    //    {
    //        for (int j = 1; j < listaNodos[i].Count ; j++)
    //        {
    //            if (listaNodos[i][j])
    //            {
    //                foreach (NodeAdy n in listaNodos[i][j].GetNodeAdyacents())
    //                {
    //                    if (n.node)
    //                    {
    //                        Gizmos.DrawLine(n.node.transform.position, listaNodos[i][j].transform.position);
    //                        if (listaNodos[i][j].nodeState == NodeState.Open)
    //                        {
    //                            Gizmos.color = Color.yellow;
    //                        }
    //                        if (listaNodos[i][j].nodeState == NodeState.Close)
    //                        {
    //                            Gizmos.color = Color.red;
    //                        }
    //                        Gizmos.DrawCube(listaNodos[i][j].transform.position, listaNodos[i][j].transform.localScale * 2.5f);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}

