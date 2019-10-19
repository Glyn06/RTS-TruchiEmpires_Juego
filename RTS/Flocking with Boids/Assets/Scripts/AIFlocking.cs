using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlocking : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Vector3 Alineacion(AIBoid myBoid)
    {
        Vector3 promedioDirecciones = Vector3.zero;
        Vector3 direccionResultante = Vector3.zero;
        if (myBoid.GetBoidCollider().Length > 0)
        {
            for (int i = 0; i < myBoid.GetBoidCollider().Length; i++)
            {
                if (myBoid != myBoid.GetBoidCollider()[i])
                {
                    promedioDirecciones = promedioDirecciones + myBoid.GetBoidCollider()[i].transform.forward;
                }
            }
        }
        return promedioDirecciones.normalized;
    }
    public Vector3 Cohesion(AIBoid myBoid)
    {
        Vector3 promedioPosiciones = Vector3.zero;
        if (myBoid.GetBoidCollider().Length > 0)
        {
            for (int i = 0; i < myBoid.GetBoidCollider().Length; i++)
            {
                if (myBoid != myBoid.GetBoidCollider()[i])
                {
                    promedioPosiciones = promedioPosiciones + myBoid.GetBoidCollider()[i].transform.position;

                }
            }
            promedioPosiciones = promedioPosiciones / myBoid.GetBoidCollider().Length;
            promedioPosiciones = promedioPosiciones.normalized;
        }
        return promedioPosiciones;
    }
    public Vector3 Separacion()
    {
        return Vector3.zero;
    }
}
