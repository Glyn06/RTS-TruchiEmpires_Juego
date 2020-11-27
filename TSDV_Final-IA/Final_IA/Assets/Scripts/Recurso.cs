using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recurso : GameElement
{
    // Start is called before the first frame update
    public float CantidadDeRecurso;

    // Update is called once per frame
    private void Start()
    {
        CheckNodeCercano();
    }
    void Update()
    {
        checkAcabado();
        gm = GameManager.instanceGameManager;
        if (gm != null)
        {
            if (gm.generadorNodos.NodesGenerates && !nodoGenerado)
            {
                nodoGenerado = true;
                CheckNodeCercano();
            }
        }
    }
    public void checkAcabado()
    {
        if (CantidadDeRecurso <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
