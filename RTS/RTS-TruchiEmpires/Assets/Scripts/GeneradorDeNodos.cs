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
    //public float range;
    //private Nodo nodoCrear;
    private Vector3 vector;
    private List<List<Node>> listaNodos;
    private Vector3 OriginalPosition;
    RaycastHit hit;
    void Start()
    {
        ancho = (int)Terrain.transform.localScale.x;
        ancho = ancho + 1;
        largo = (int)Terrain.transform.localScale.z;
        largo = largo + 1;
        Debug.Log("Ancho:" + ancho);
        Debug.Log("Largo:" + largo);
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
                        Debug.Log("ENTRE");
                        Node node = Instantiate(nodeObject, new Vector3(actualPos.x, hit.transform.position.y + 1.0f, actualPos.z), Quaternion.identity).GetComponent<Node>();
                        listaNodos[i][j] = node;

                        if (hit.collider.tag == "Mineral" || hit.collider.tag == "Centro Urbano")
                            node.IsObstacle = true;
                    }
                }
                actualPos.x += 1.0f;
            }
            actualPos.x = OriginalPosition.x;
            actualPos.z += 1.0f;
            //transform.position = actualPos;
        }
    }
        // Update is called once per frame
        /*private void GenerarMapaDeNodos()
        {
            for (int i = 0; i < ancho; i++)
            {
                for (int j = 0; j < largo; j++)
                {

                    if (Physics.Raycast(cursor.transform.position, Vector3.down, out hit, Mathf.Infinity))
                    {
                        switch (hit.collider.tag)
                        {
                            case "Piso":
                                if (hit.collider.transform.position != null)
                                {
                                    Node nodo = Instantiate(nodeObject, hit.point + new Vector3(0, 1f, 0), Quaternion.identity).GetComponent<Node>();
                                    nodo.IsObstacle = false;
                                    listaNodos[i][j] = nodo;
                                }
                                break;
                            case "Mineral":
                                if (hit.collider.transform.position != null)
                                {
                                    Node nodo = Instantiate(nodeObject, hit.point, Quaternion.identity).GetComponent<Node>();
                                    nodo.IsObstacle = true;
                                    listaNodos[i][j] = nodo;

                                }
                                break;
                                //AGREGAR MAS CASE SI HAY MAS TAGS DE OBJETOS TEMPORALES EN EL MAPA COMO POR EJEMPLO UNA CONSTRUCCION.
                        }

                        //Debug.Log()
                    }
                    cursor.transform.position +=  new Vector3(1,0,0);
                    //cursor.transform.position = OriginalPosition + new Vector3(i, 0, j);
                }
                cursor.transform.position += new Vector3(0, 0, 1);
                //Debug.Log(listaNodos.Count);
            }
        }*/
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
}

