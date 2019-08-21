using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Aldeano : MonoBehaviour
{
    public float speed;
    public float capacity;
    private float cantOro;
    private float cantPiedra;
    private float cantAlimento;
    private float cantMadera;
    private FSM fsmMinero;
    private bool ExplotandoRecurso;
    private List<GameObject> Depositos;
    //Esta variable contendra la posicion donde se encuentra el trabajo a realizar(la posicion de la casa a construir o la mina la cual minar, etc).
    private GameObject objetivoTrabajo;
    private Transform posicionActual;
    private Transform depositoMasCercano;
    private GameManager gm;
    [HideInInspector]
    public string trabajo;
    //HAGO UN ENUM DE Estados
    public enum EstadosMinero {
        Idle,
        IrAMinar,
        Minando,
        LLevarOro,
        DepositarOro,
        CancelarAccion,
        Count
    }
    //HAGO UN ENUM DE Eventos
    public enum EventosMinero {
        ClickInMine,
        CollisionMine,
        FullCapasity,
        CollisionHouse,
        Stop,
        Count
    }
    private void Awake()
    {
        Depositos = new List<GameObject>();
        if (GameManager.instanceGameManager != null)
        {
            gm = GameManager.instanceGameManager;
        }
        // Aca defino las relaciones de estado y le hago el new al objeto FSM
        fsmMinero = new FSM((int)EstadosMinero.Count, (int)EventosMinero.Count, (int)EstadosMinero.Idle);

        fsmMinero.SetRelations((int)EstadosMinero.Idle, (int)EstadosMinero.IrAMinar, (int)EventosMinero.ClickInMine);
        fsmMinero.SetRelations((int)EstadosMinero.IrAMinar, (int)EstadosMinero.Minando, (int)EventosMinero.CollisionMine);
        fsmMinero.SetRelations((int)EstadosMinero.IrAMinar, (int)EstadosMinero.CancelarAccion, (int)EventosMinero.Stop);
        fsmMinero.SetRelations((int)EstadosMinero.Minando, (int)EstadosMinero.LLevarOro, (int)EventosMinero.FullCapasity);
        fsmMinero.SetRelations((int)EstadosMinero.Minando, (int)EstadosMinero.CancelarAccion, (int)EventosMinero.Stop);
        fsmMinero.SetRelations((int)EstadosMinero.LLevarOro, (int)EstadosMinero.DepositarOro, (int)EventosMinero.CollisionHouse);
        fsmMinero.SetRelations((int)EstadosMinero.LLevarOro, (int)EstadosMinero.CancelarAccion, (int)EventosMinero.Stop);
        fsmMinero.SetRelations((int)EstadosMinero.DepositarOro, (int)EstadosMinero.Idle, (int)EventosMinero.Stop);
        fsmMinero.SetRelations((int)EstadosMinero.DepositarOro, (int)EstadosMinero.IrAMinar, (int)EventosMinero.ClickInMine);
        fsmMinero.SetRelations((int)EstadosMinero.CancelarAccion, (int)EstadosMinero.Idle, (int)EventosMinero.Stop);
    }

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("ESTADO MINERO: " + fsmMinero.GetCurrentState());
        //HAGO EL SWITCH DE LA MAQUINA DE ESTADOS
        switch (fsmMinero.GetCurrentState()) {
            case (int)EstadosMinero.Idle:            
                IdleMinero();
                break;
            case (int)EstadosMinero.IrAMinar:
                IrAMinar();
                break;
            case (int)EstadosMinero.Minando:

                Minar();
                break;
            case (int)EstadosMinero.LLevarOro:
                LLevarOro();
                break;
            case (int)EstadosMinero.DepositarOro:
                DepositarOro();
                break;
            case (int)EstadosMinero.CancelarAccion:
                CancelarAccion();
                break;
        }
    }
    public void SetObjetivoTrabajo(GameObject _objetivoTrabajo)
    {
        objetivoTrabajo = _objetivoTrabajo;
    }
    public void SetPosisionActual(Transform _posicionActual)
    {
        posicionActual = _posicionActual;
    }
    //HAGO LAS FUNCIONES QUE VA A LLAMAR EL SWITCH DE LA MAQUINA DE ESTADOS UBICADA EN EL Update()
    public void IdleMinero() {
        //Animacion del minero con un cacho de oro en caso de tenerlo
        //sino tiene oro en sima se ejecuta la animacion "Idle" del aldeano en si
        if (trabajo == "Minar")
        {
            fsmMinero.SendEvent((int)EventosMinero.ClickInMine);
        }
        else {
            //CORRER ANIMACION IDLE
        }
    }
    public void IrAMinar() {
        if (objetivoTrabajo.gameObject.activeSelf)
        {
            transform.LookAt(new Vector3(objetivoTrabajo.transform.position.x,transform.position.y, objetivoTrabajo.transform.position.z));
            transform.position = transform.position + transform.forward * speed;
        }
        else {
            fsmMinero.SendEvent((int)EstadosMinero.Idle);
        }
    }
    public void Minar() {
        //SE EJECUTA LA ANIMACION DE MINAR
        if (objetivoTrabajo.gameObject.activeSelf)
        {
            cantOro = cantOro + Time.deltaTime;
            Debug.Log("cantOro: " + (int)cantOro);
        }
        if (cantOro >= capacity) {
            Debug.Log("FULL CAPASITY");
            fsmMinero.SendEvent((int)EventosMinero.FullCapasity);
        }
        if (!objetivoTrabajo.gameObject.activeSelf)
        {
            fsmMinero.SendEvent((int)EventosMinero.FullCapasity);
        }
        
    }
    public void LLevarOro() {
        //DEBERIA FIJARSE CUAL ES EL ALMACEN DE ORO O CENTRO URBANO MAS CERCANO
        BuscarAlmacenMasCercano();
        transform.LookAt(new Vector3(depositoMasCercano.position.x,transform.position.y,depositoMasCercano.position.z));
        transform.position = transform.position + transform.forward * speed;

    }
    public void DepositarOro() {
        gm.recursoOro = gm.recursoOro + cantOro;
        cantOro = 0;
        fsmMinero.SendEvent((int)EventosMinero.ClickInMine);
    }
    public void CancelarAccion() {
        //TODAVIA NO HACE NADA
    }
    public void BuscarAlmacenMasCercano()
    {
        depositoMasCercano = SceneManager.GetActiveScene().GetRootGameObjects()[0].transform;
        for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
        {
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Centro Urbano" || SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Deposito Minero")
            {
                Depositos.Add(SceneManager.GetActiveScene().GetRootGameObjects()[i]);

            }
        }
        for (int i = 0; i < Depositos.Count; i++)
        {
            if (Depositos[i].transform.position.magnitude < depositoMasCercano.transform.position.magnitude)
            {
                depositoMasCercano= Depositos[i].transform;
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("COLISIONE");
        //Debug.Log("TRIGGEREO CON " + other.gameObject.tag);
        if (trabajo == "Minar" && other.gameObject.tag == "Mineral" && cantOro < capacity)
        {
            ExplotandoRecurso = true;
            fsmMinero.SendEvent((int)EventosMinero.CollisionMine);
            other.gameObject.GetComponent<Recurso>().CantidadDeRecurso = other.gameObject.GetComponent<Recurso>().CantidadDeRecurso - Time.deltaTime;
        }
        //Debug.Log(trabajo);

        
        if (cantOro >= capacity)
        {
            fsmMinero.SendEvent((int)EventosMinero.FullCapasity);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (trabajo == "Minar" && other.gameObject.tag == "Centro Urbano" && cantOro > 0)
        {
            //Debug.Log("ENTRE");
            fsmMinero.SendEvent((int)EventosMinero.CollisionHouse);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (trabajo == "Minar" && other.gameObject.tag == "Mineral")
        {
            Debug.Log("ENTRE");
            ExplotandoRecurso = false;
        }
    }
}
