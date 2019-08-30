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
        Debug.Log("posicion:" + posicion);
        _estado = estado;
        this.posicion = new Vector3(posicion.x, posicion.y, posicion.z);
        Adjacents = new List<Nodo>();
    }
    public Nodo(){}

    public Vector3 GetPosition()
    {
        return posicion;
    }
    public void AddAdyNodo(Nodo nodo)
    {
        if (nodo != null)
        {
            Adjacents.Add(nodo);
        }
    }
    public Nodo GetAdyacente()
    {
        for (int i = 0; i < Adjacents.Count; i++)
        {
            if (!Adjacents[i].GetObstacle() && Adjacents[i].GetEstado() == EstadoNodo.Abierto)
            {
                return Adjacents[i];
            }
        }
        return null;
    }
    public List<Nodo> GetAdyacentes()
    {
        return Adjacents;
    }
    public void SetObstacle(bool _obstacle)
    {
        obstacle = _obstacle;
    }
    public void SetEstado(EstadoNodo estadoNodo)
    {
        _estado = estadoNodo;
    }
    public bool GetObstacle()
    {
        return obstacle;
    }
    public EstadoNodo GetEstado()
    {
        return _estado;
    }
}
