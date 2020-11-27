using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentroUrbano : GameElement
{
    // Start is called before the first frame update
    private void Update()
    {
        gm = GameManager.instanceGameManager;
        if (gm != null)
        {
            //Debug.Log(nodoGenerado);
            if (gm.generadorNodos.NodesGenerates && !nodoGenerado)
            {
                Debug.Log("NODO GENERADO EN CENTRO URBANO");
                nodoGenerado = true;
                CheckNodeCercano();
            }
        }
    }



}
