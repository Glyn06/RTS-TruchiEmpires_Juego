using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlocking : MonoBehaviour
{
    public AIBoid boid;
    public void Update()
    {
        boid.FinalPosition = Flocking(boid);
        
    }
    public Vector3 Flocking(AIBoid myBoid)
    {
        Vector3 direccionFinal = Vector3.zero;
        direccionFinal = Alineacion(myBoid) + Cohesion(myBoid) + Separacion(myBoid);
        direccionFinal = direccionFinal / 3;
        return direccionFinal;
    }
    public Vector3 Alineacion(AIBoid myBoid)
    {
        Vector3 promedioDirecciones = Vector3.zero;
        Vector3 direccionResultante = Vector3.zero;
        if (myBoid.GetBoidCollider() != null)
        {
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
        }
        return promedioDirecciones.normalized;
    }
    public Vector3 Cohesion(AIBoid myBoid)
    {
        Vector3 promedioPosiciones = Vector3.zero;
        if (myBoid.GetBoidCollider() != null)
        {
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
        }
        return promedioPosiciones;
    }
    public Vector3 Separacion(AIBoid myBoid)
    {
        Vector3 VectorResultante = myBoid.transform.position;
        if (myBoid.GetBoidCollider() != null)
        {
            if (myBoid.GetBoidCollider().Length > 0)
            {
                for (int i = 0; i < myBoid.GetBoidCollider().Length; i++)
                {
                    if (myBoid != myBoid.GetBoidCollider()[i])
                    {
                        VectorResultante = VectorResultante + (myBoid.GetBoidCollider()[i].transform.position - myBoid.transform.position).normalized;
                    }
                }
                VectorResultante = VectorResultante * -1;
                VectorResultante = VectorResultante.normalized;
            }
        }
        return VectorResultante;
    }

}
