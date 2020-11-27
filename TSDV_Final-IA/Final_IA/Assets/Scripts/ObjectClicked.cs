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
            if (seleccion1 != null && seleccion1.tag == "Aldeano" && seleccion2 != null && seleccion2.tag == "Piso")
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, range))
                {
                    seleccion2.transform.position = hit.point;
                }
            }
        }

        if (seleccion1 != null && seleccion2 != null)
        {
            if (seleccion1.tag == "Aldeano" && seleccion2.tag == "Mineral")
            {
                seleccion1.GetComponent<Aldeano>().SetObjetivoTrabajo(seleccion2);
                seleccion1.GetComponent<Aldeano>().trabajo = "Minar";
                seleccion1 = null;
                seleccion2 = null;
            }
            else if (seleccion1.tag == "Aldeano" && (seleccion2.tag == "Centro Urbano" || seleccion2.tag == "Deposito Minero"))
            {
                Aldeano aldeano = seleccion1.GetComponent<Aldeano>();
                if (aldeano != null && aldeano.GetOro() > 0)
                {
                    aldeano.GetComponent<Aldeano>().SetObjetivoTrabajo(seleccion2);
                    aldeano.GetComponent<Aldeano>().trabajo = "Llevar Oro";
                    
                }
                else
                {
                    aldeano.GetComponent<Aldeano>().SetObjetivoTrabajo(seleccion2);
                    aldeano.GetComponent<Aldeano>().trabajo = "Mover Aldeano";
                }
                seleccion1 = null;
                seleccion2 = null;
            }
            else if (seleccion1.tag == "Aldeano" && seleccion2.tag == "Piso")
            {
                seleccion1.GetComponent<Aldeano>().SetObjetivoTrabajo(seleccion2);
                seleccion1.GetComponent<Aldeano>().trabajo = "Mover Aldeano";
                seleccion1 = null;
                seleccion2 = null;
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
