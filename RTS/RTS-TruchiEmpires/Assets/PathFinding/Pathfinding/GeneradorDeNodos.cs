using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeNodos : MonoBehaviour
{

    public GameObject Terrain;
    private int ancho = 1;
    private int largo = 1;
    public GameObject cursor;
    public GameObject nodeObject;
    //public static float range;
    //private static Nodo nodoCrear;
    private static Vector3 vector;
    private static List<List<Node>> listaNodos;
    private static Vector3 OriginalPosition;
    
    private void Awake()
    {
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
                        Node node = Instantiate(nodeObject, new Vector3(actualPos.x, Terrain.transform.position.y+1, actualPos.z), Quaternion.identity).GetComponent<Node>();

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
    public static Node GetClosestNode(Vector3 pos)
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
                    if (diff.magnitude < 1f)
                    {
                        return listaNodos[i][j];
                    }
                }
            }
        }
        return null;
    }
}

