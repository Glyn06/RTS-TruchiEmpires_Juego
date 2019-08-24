using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo 
{
    // Start is called before the first frame update
    public enum EstadoNodo
    {
        Abierto,
        Cerrado,
        Nada,
        Count,
    }
    List<Nodo> Adjacents;
    bool obstacle;
    Vector3 NodoRegistrar;
    EstadoNodo _estado;
    Vector3 posicion;
    public Nodo(Vector3 posicion, EstadoNodo estado, bool obstacle)
    {
        Debug.Log("posicion:"+posicion);
        _estado = estado;
        this.posicion = new Vector3(posicion.x,posicion.y,posicion.z);
    }

    public Vector3 GetPosition()
    {
        return posicion;
    }
}
