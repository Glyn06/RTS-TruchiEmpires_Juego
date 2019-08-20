using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recurso : MonoBehaviour
{
    // Start is called before the first frame update
    public float CantidadDeRecurso;

    // Update is called once per frame
    void Update()
    {
        checkAcabado();
    }
    public void checkAcabado()
    {
        if (CantidadDeRecurso <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
