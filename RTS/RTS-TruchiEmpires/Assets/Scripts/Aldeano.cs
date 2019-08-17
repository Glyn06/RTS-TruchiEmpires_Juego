using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aldeano : MonoBehaviour
{

    private FSM fsmMinero;
    //Esta variable contendra la posicion donde se encuentra el trabajo a realizar(la posicion de la casa a construir o la mina la cual minar, etc).
    private Transform objetivoTrabajo;
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
        fsmMinero.SetRelations((int)EstadosMinero.CancelarAccion, (int)EstadosMinero.Idle, (int)EventosMinero.Stop);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
    //HAGO LAS FUNCIONES QUE VA A LLAMAR EL SWITCH DE LA MAQUINA DE ESTADOS UBICADA EN EL Update()
    public void IdleMinero() {
        //Animacion del minero con un cacho de oro en caso de tenerlo
        //sino tiene oro en sima se ejecuta la animacion "Idle" del aldeano en si
    }
    public void IrAMinar() {

    }
    public void Minar() {

    }
    public void LLevarOro() {

    }
    public void DepositarOro() {

    }
    public void CancelarAccion() {

    }
}
