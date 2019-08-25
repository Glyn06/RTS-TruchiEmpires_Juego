using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    
    public enum TypePathFinder
    {
        BreadhFirst,
        DepthFirst,
        Count,
    }
    public TypePathFinder TipoPath;
    public int ancho = 1;
    public int largo = 1;
    public GameObject cursor;
    //public float range;
    private Nodo nodoCrear;
    private Vector3 vector;
    private List<List<Nodo>> listaNodos;
    private Vector3 OriginalPosition;
    RaycastHit hit;
    void Start()
    {
        ancho = ancho + 1;
        largo = largo + 1;
        OriginalPosition = transform.position;
        listaNodos = new List<List<Nodo>>();
        
        for (int i = 0; i < ancho; i++)
        {
            listaNodos.Add(new List<Nodo>());
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

    // Update is called once per frame
    private void GenerarMapaDeNodos()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                
                if (Physics.Raycast(cursor.transform.position, cursor.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
                {
                    switch (hit.collider.tag)
                    {
                        case "Piso":
                            if (hit.collider.transform.position != null)
                            {
                                nodoCrear = new Nodo(hit.point + new Vector3(0, 1, 0), Nodo.EstadoNodo.Abierto, false);
                                listaNodos[i][j] = nodoCrear;
                            }
                            break;
                        case "Mineral":
                            if (hit.collider.transform.position != null)
                            {
                                nodoCrear = new Nodo(hit.point + new Vector3(0, 1, 0), Nodo.EstadoNodo.Cerrado, true);
                                listaNodos[i][j] = nodoCrear;
 
                            }
                            break;
                            //AGREGAR MAS CASE SI HAY MAS TAGS DE OBJETOS TEMPORALES EN EL MAPA COMO POR EJEMPLO UNA CONSTRUCCION.
                    }
                    
                    //Debug.Log()
                }
                transform.position = OriginalPosition + new Vector3(i, 0, j);
            }
            //Debug.Log(listaNodos.Count);
        }
    }
    private void SeteadorDeAdyasentes()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                if (listaNodos[i][j] != null)
                {
                    //Lados
                    if ((i + 1 < ancho && i + 1 < largo) && listaNodos[i + 1][j] != null)
                    {
                        listaNodos[i][j].AddAdyNodo(listaNodos[i + 1][j]);
                    }
                    if ((j + 1 < ancho && j + 1 < largo) && listaNodos[i][j + 1] != null)
                    {
                        listaNodos[i][j].AddAdyNodo(listaNodos[i][j + 1]);
                    }
                    if (i - 1 >= 0 && listaNodos[i - 1][j] != null)
                    {
                        listaNodos[i][j].AddAdyNodo(listaNodos[i - 1][j]);
                    }
                    if (j - 1 >= 0 && listaNodos[i][j - 1] != null)
                    {
                        listaNodos[i][j].AddAdyNodo(listaNodos[i][j - 1]);
                    }

                    //Diagonales
                    if ((i + 1 < ancho && i + 1 < largo) && (j + 1 < ancho && j + 1 < largo) && listaNodos[i + 1][j + 1] != null)
                    {
                        listaNodos[i][j].AddAdyNodo(listaNodos[i + 1][j + 1]);
                    }
                    if ((i + 1 < ancho && i + 1 < largo) && j - 1 >= 0 && listaNodos[i + 1][j - 1] != null)
                    {
                        listaNodos[i][j].AddAdyNodo(listaNodos[i + 1][j - 1]);
                    }
                    if (i - 1 >= 0 && (j + 1 < ancho && j + 1 < largo) && listaNodos[i - 1][j + 1] != null)
                    {
                        listaNodos[i][j].AddAdyNodo(listaNodos[i - 1][j + 1]);
                    }
                    if (i - 1 >= 0 && j - 1 >= 0 && listaNodos[i - 1][j - 1] != null)
                    {
                        listaNodos[i][j].AddAdyNodo(listaNodos[i - 1][j - 1]);
                    }
                }
            }
        }
    }
    public List<Nodo> GetPath(Nodo origen,Nodo destino)
    {
        return null;
    }
    public void OpenNode(Nodo nodo)
    {

    }
    
    /*private void OnDrawGizmos()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                if (listaNodos[i][j] != null)
                {
                    Gizmos.DrawCube(listaNodos[i][j].GetPosition(), new Vector3(0.1f, 0.1f, 0.1f));
                }
            }
        }
    }*/
}
