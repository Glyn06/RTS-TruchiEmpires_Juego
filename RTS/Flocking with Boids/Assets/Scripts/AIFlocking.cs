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
        for (int i = 0; i < myBoid.GetBoidCollider().Length; i++)
        {
            promedioDirecciones = promedioDirecciones + myBoid.GetBoidCollider()[i].transform.forward;
        }
        return promedioDirecciones.normalized;
    }
    public Vector3 Cohesion()
    {
        return Vector3.zero;
    }
    public Vector3 Separacion()
    {
        return Vector3.zero;
    }
}
