using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicked : MonoBehaviour
{
    public float range;
    //public string objectSelect;
    private bool cancelarAccion;
    private Vector3 positionA;
    private Vector3 positionB;
    private GameObject seleccion1;
    private GameObject seleccion2;
    private int tope_clicks = 2;
    private bool Cancelar;
    private int clicks;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            seleccion1 = CheckHitMouse();
        }
        if (seleccion1 != null && Input.GetKeyDown(KeyCode.Mouse1))
        {
            seleccion2 = CheckHitMouse();
        }

        if (seleccion1 != null && seleccion2 != null)
        {
            if (seleccion1.tag == "Aldeano" && seleccion2.tag == "Mineral")
            {
                seleccion1.GetComponent<Aldeano>().SetObjetivoTrabajo(seleccion2);
                seleccion1.GetComponent<Aldeano>().trabajo = "Minar";
            }
        }
    }
    public GameObject CheckHitMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform != null)
            {
                return hit.transform.gameObject;
            }
            else
            {
                return null;
            }
        }
        GameObject empty = null;
        return empty;

    }
}
