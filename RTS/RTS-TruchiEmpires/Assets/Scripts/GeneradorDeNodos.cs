using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeNodos : MonoBehaviour
{
    // Start is called before the first frame update

    public float ancho;
    public float largo;
    public GameObject cursor;
    //public float range;
    private Nodo nodoCrear;
    private Vector3 vector;
    private List<Nodo> listaNodos;
    private Vector3 OriginalPosition;
    void Start()
    {
        OriginalPosition = transform.position;
        listaNodos = new List<Nodo>();
        GenerarMapaDeNodos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerarMapaDeNodos()
    {
        for (int i = 0; i <= ancho; i++)
        {
            for (int j = 0; j <= largo; j++)
            {
                RaycastHit hit;
                if (Physics.Raycast(cursor.transform.position, cursor.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
                {
                    if (hit.collider.tag == "Piso")
                    {
                        if (hit.collider.transform.position != null)
                        {
                            nodoCrear = new Nodo(hit.point+ new Vector3(0,1,0), Nodo.EstadoNodo.Nada,false); 
                            listaNodos.Add(nodoCrear);
                        }
                        
                    }
                    /*else
                    {
                        if (hit.collider.transform.position != null)
                        {
                            nodoCrear = new Nodo(hit.point, Nodo.EstadoNodo.Cerrado, true); /*+ new Vector3(0,1,0));
                            listaNodos.Add(nodoCrear);
                        }
                    }*/
                }
                transform.position = OriginalPosition + new Vector3(i, 0, j);
                //Debug.Log()
            }
        }
        Debug.Log(listaNodos.Count);
    }

    private void OnDrawGizmos()
    {
        foreach(Nodo n in listaNodos)
        {
            Gizmos.DrawCube(n.GetPosition(), new Vector3(0.1f, 0.1f, 0.1f));
        }
    }
}
