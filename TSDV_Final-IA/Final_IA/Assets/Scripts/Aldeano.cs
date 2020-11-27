using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Aldeano : MonoBehaviour
{
    public enum StatePath
    {
        Nulo,
        EnUso
    }
    //TESTING BORRAR LUEGO.
    //public int numeroMinero;

    private StatePath statePath;
    public float speed;
    public float capacity;
    public PathfinderType pathType;
    [SerializeField] private float cantOro;
    [SerializeField] private float cantPiedra;
    [SerializeField] private float cantAlimento;
    [SerializeField] private float cantMadera;
    private FSM fsm;
    private List<GameObject> Depositos;
    //Esta variable contendra la posicion donde se encuentra el trabajo a realizar(la posicion de la casa a construir o la mina la cual minar, etc).
    private GameObject objetivoTrabajo;
    private Transform posicionActual;
    private GameObject depositoMasCercano;
    private GameManager gm;
    private Node actualNode;
    private Node nodoFinal;
    private List<Node> path;
    private bool generarNodoActual = true;
    private bool nodoFinalObstaculo;
    private bool nodoInicialObstaculo;
    private int i = 0;
    //private PathGenerator path;
    [HideInInspector]
    public string trabajo;
    //HAGO UN ENUM DE Estados
    public enum EstadosAldeano
    {
        Idle,
        IrAMinar,
        Minando,
        LLevarOro,
        DepositarOro,
        CancelarAccion,
        MoverAldeano,
        Count
    }
    //HAGO UN ENUM DE Eventos
    public enum EventosAldeano
    {
        ClickInMine,
        ClickInHouse,
        ClickInMap,
        CollisionMine,
        FullCapasity,
        CollisionHouse,
        Stop,
        Count
    }
    private void Awake()
    {
        nodoFinalObstaculo = false;
        nodoInicialObstaculo = false;
        statePath = StatePath.Nulo;
        Depositos = new List<GameObject>();
        if (GameManager.instanceGameManager != null)
        {
            gm = GameManager.instanceGameManager;
        }
        // Aca defino las relaciones de estado y le hago el new al objeto FSM
        fsm = new FSM((int)EstadosAldeano.Count, (int)EventosAldeano.Count, (int)EstadosAldeano.Idle);

        //minero
        fsm.SetRelations((int)EstadosAldeano.Idle, (int)EstadosAldeano.IrAMinar, (int)EventosAldeano.ClickInMine);
        fsm.SetRelations((int)EstadosAldeano.IrAMinar, (int)EstadosAldeano.Minando, (int)EventosAldeano.CollisionMine);
        fsm.SetRelations((int)EstadosAldeano.IrAMinar, (int)EstadosAldeano.CancelarAccion, (int)EventosAldeano.Stop);
        fsm.SetRelations((int)EstadosAldeano.Minando, (int)EstadosAldeano.LLevarOro, (int)EventosAldeano.FullCapasity);
        fsm.SetRelations((int)EstadosAldeano.Minando, (int)EstadosAldeano.CancelarAccion, (int)EventosAldeano.Stop);
        fsm.SetRelations((int)EstadosAldeano.LLevarOro, (int)EstadosAldeano.DepositarOro, (int)EventosAldeano.CollisionHouse);
        fsm.SetRelations((int)EstadosAldeano.LLevarOro, (int)EstadosAldeano.CancelarAccion, (int)EventosAldeano.Stop);
        fsm.SetRelations((int)EstadosAldeano.DepositarOro, (int)EstadosAldeano.Idle, (int)EventosAldeano.Stop);
        fsm.SetRelations((int)EstadosAldeano.DepositarOro, (int)EstadosAldeano.IrAMinar, (int)EventosAldeano.ClickInMine);
        fsm.SetRelations((int)EstadosAldeano.CancelarAccion, (int)EstadosAldeano.Idle, (int)EventosAldeano.Stop);

        fsm.SetRelations((int)EstadosAldeano.Idle, (int)EstadosAldeano.LLevarOro, (int)EventosAldeano.ClickInHouse);
        fsm.SetRelations((int)EstadosAldeano.IrAMinar, (int)EstadosAldeano.LLevarOro, (int)EventosAldeano.ClickInHouse);
        fsm.SetRelations((int)EstadosAldeano.Minando, (int)EstadosAldeano.LLevarOro, (int)EventosAldeano.ClickInHouse);

        //movimiento
        fsm.SetRelations((int)EstadosAldeano.Idle, (int)EstadosAldeano.MoverAldeano, (int)EventosAldeano.ClickInMap);
        fsm.SetRelations((int)EstadosAldeano.IrAMinar, (int)EstadosAldeano.MoverAldeano, (int)EventosAldeano.ClickInMap);
        fsm.SetRelations((int)EstadosAldeano.Minando, (int)EstadosAldeano.MoverAldeano, (int)EventosAldeano.ClickInMap);
        fsm.SetRelations((int)EstadosAldeano.MoverAldeano, (int)EstadosAldeano.Idle, (int)EventosAldeano.Stop);
    }

    private void Start()
    {
        if (GameManager.instanceGameManager != null)
        {
            gm = GameManager.instanceGameManager;
        }
        nodoFinalObstaculo = false;
        nodoInicialObstaculo = false;
        statePath = StatePath.Nulo;
    }
    // Update is called once per frame
    void Update()
    {
        if (generarNodoActual)
        {
            if (gm != null)
            {
                actualNode = gm.FindClosestNode(transform.position);
                generarNodoActual = false;
            }
        }
        //HAGO EL SWITCH DE LA MAQUINA DE ESTADOS
        //Debug.Log((EstadosMinero)fsmMinero.GetCurrentState() + ": " + numeroMinero);
        switch (fsm.GetCurrentState())
        {
            case (int)EstadosAldeano.Idle:
                Idle();
                break;
            case (int)EstadosAldeano.IrAMinar:
                IrAMinar();
                break;
            case (int)EstadosAldeano.Minando:
                Minar();
                break;
            case (int)EstadosAldeano.LLevarOro:
                LLevarOro();
                break;
            case (int)EstadosAldeano.DepositarOro:
                DepositarOro();
                break;
            case (int)EstadosAldeano.CancelarAccion:
                CancelarAccion();
                break;
        }

    }

    public void CheckNodeActual(Vector3 position)
    {
        if (gm != null)
        {
            actualNode = gm.generadorNodos.GetClosestNode(position);
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
    public void Idle()
    {
        switch (trabajo)
        {
            case "Minar":
                fsm.SendEvent((int)EventosAldeano.ClickInMine);
                i = 0;
                break;
            case "Llevar Oro":
                fsm.SendEvent((int)EventosAldeano.ClickInHouse);
                break;
            case "Mover Aldeano":
                fsm.SendEvent((int)EventosAldeano.ClickInMap);
                break;
        }
    }
    public void IrAMinar()
    {
        //Debug.Log("Yendo a Minar");
        //if (objetivoTrabajo.gameObject.activeSelf)
        //{
            if (statePath == StatePath.Nulo)
            {
                CheckNodeActual(transform.position);

                nodoFinal = objetivoTrabajo.GetComponent<GameElement>().GetMyNode();
                if (nodoFinal.IsObstacle == true)
                {
                    nodoFinalObstaculo = true;
                    nodoFinal.IsObstacle = false;
                }
                //Debug.Log("ENTRE AL PATH");
                CheckPath();
                //Debug.Log(path.Count);
                statePath = StatePath.EnUso;
            }

            Vector3 diff;
            if (path.Count > 0)
            {
                if (i < path.Count)
                {
                    //Debug.Log("Sub indice:"+i);
                    Vector3 point = path[i].transform.position;
                    point.y = transform.position.y;
                    transform.LookAt(point);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    diff = point - this.transform.position;
                    if (diff.magnitude < 0.5f)
                    {
                        i++;
                    }
                }
                else
                {
                    FinishPath();
                    fsm.SendEvent((int)EventosAldeano.Stop);
                    trabajo = " ";
                }
            }

        if (trabajo == "Llevar Oro")
        {
            fsm.SendEvent((int)EventosAldeano.ClickInHouse);
        }
        //}
    }
    public void Minar()
    {
        //Debug.Log("Minando");
        //SE EJECUTA LA ANIMACION DE MINAR
        FinishPath();
        if (objetivoTrabajo.gameObject.activeSelf)
        {
            cantOro = cantOro + Time.deltaTime;
            //Debug.Log("cantOro: " + (int)cantOro);
        }
        if (cantOro >= capacity)
        {
            //Debug.Log("FULL CAPASITY");
            fsm.SendEvent((int)EventosAldeano.FullCapasity);
        }
        if (!objetivoTrabajo.gameObject.activeSelf)
        {
            fsm.SendEvent((int)EventosAldeano.FullCapasity);
        }
        if (trabajo == "Llevar Oro")
        {
            fsm.SendEvent((int)EventosAldeano.ClickInHouse);
        }
    }
    public void LLevarOro()
    {
        //Debug.Log("Llevando el oro");
        //DEBERIA FIJARSE CUAL ES EL ALMACEN DE ORO O CENTRO URBANO MAS CERCANO
        BuscarAlmacenMasCercano();
        if (depositoMasCercano != null)
        {
            
            if (statePath == StatePath.Nulo)
            {
                //gm.pathGenerator.CleanNodes();
                CheckNodeActual(transform.position);

                nodoFinal = depositoMasCercano.gameObject.GetComponent<GameElement>().GetMyNode();
                //Debug.Log(depositoMasCercano.GetComponent<GameElement>().GetMyNode().transform.position);
                if (nodoFinal.IsObstacle == true)
                {
                    nodoFinalObstaculo = true;
                    nodoFinal.IsObstacle = false;
                }
                //Debug.Log("Coordenadas depositos mas cercanos: " + depositoMasCercano.transform.position);
                //Debug.Log("Posicion Nodo final " + nodoFinal.transform.position);
                //Debug.Log("Nodo Final obstaculo: " + nodoFinal.IsObstacle);
                CheckPath();
                //Debug.Log("Count Path:" + path.Count);
                statePath = StatePath.EnUso;
            }
            
            Vector3 diff;
            if (path.Count > 0)
            {
                if (i < path.Count)
                {
                    Vector3 point = path[i].transform.position;
                    point.y = transform.position.y;
                    transform.LookAt(point);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    diff = point - this.transform.position;
                    if (diff.magnitude < 0.5f)
                    {
                        i++;
                    }
                }
            }
        }

    }
    public void DepositarOro()
    {
        Debug.Log("Depositando oro");
        FinishPath();
        gm.recursoOro = gm.recursoOro + cantOro;
        cantOro = 0;
        if (objetivoTrabajo.activeSelf)
        {
            fsm.SendEvent((int)EventosAldeano.ClickInMine);
            //Debug.Log("VOLVI AL CLICK IN MINE");
        }
        else
        {
            fsm.SendEvent((int)EventosAldeano.Stop);
            trabajo = " ";
            //Debug.Log("TOMATE UN DESCANSO PERRO");
        }

        if (trabajo == "Llevar Oro")
        {
            fsm.SendEvent((int)EventosAldeano.Stop);
            trabajo = " ";
        }
    }
    public void FinishPath()
    {
        i = 0;
        statePath = StatePath.Nulo;
        if (nodoFinalObstaculo)
        {
            nodoFinal.IsObstacle = true;
            nodoFinalObstaculo = false;
        }
        gm.pathGenerator.CleanNodes();
        path.Clear();
    }
    public Node FindNodeActual()
    {
        Node node = null;
        for (int i = 0; i < path.Count; i++)
        {
            if (i + 1 < path.Count)
            {
                if (path[i + 1] != null)
                {
                    RaycastHit Hit;
                    Vector3 MyDistanceOfObjetive = transform.position - objetivoTrabajo.transform.position;
                    Vector3 NodeDistaceOfObjetive = path[i + 1].transform.position - objetivoTrabajo.transform.position;
                    if (MyDistanceOfObjetive.magnitude > NodeDistaceOfObjetive.magnitude)
                    {

                        Vector3 Destino = path[i + 1].transform.position;
                        if (Physics.Raycast(transform.position, (Destino - transform.position).normalized, out Hit, Vector3.Distance(Destino, transform.position)))
                        {
                            if (Hit.collider.tag != "Obstaculo" && Hit.collider.tag != "Mineral")
                            {
                                return path[i + 1];
                            }
                        }

                    }
                }
                else
                {
                    return null;
                }
            }
        }
        return node;
    }
    public void CheckPath()
    {
        path = gm.pathGenerator.GetPath(actualNode, nodoFinal, pathType);
        if (pathType != PathfinderType.DepthFirst)
        {
            Node node = FindNodeActual();
            if (node == null)
            {
                path = gm.pathGenerator.GetPath(actualNode, nodoFinal, pathType);
            }
            else if (node != null)
            {
                path = path = gm.pathGenerator.GetPath(node, nodoFinal, pathType);
            }
        }
    }
    public void CancelarAccion()
    {
        FinishPath();
        trabajo = "";
        objetivoTrabajo = null;
        fsm.SendEvent((int)EventosAldeano.Stop);
    }
    public void BuscarAlmacenMasCercano()
    {
       
        for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
        {
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Centro Urbano" || SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Deposito Minero")
            {
                depositoMasCercano = SceneManager.GetActiveScene().GetRootGameObjects()[i].gameObject;
                Depositos.Add(SceneManager.GetActiveScene().GetRootGameObjects()[i]);
            }
        }
        for (int i = 0; i < Depositos.Count; i++)
        {
            if (Depositos[i].transform.position.magnitude < depositoMasCercano.transform.position.magnitude)
            {
                depositoMasCercano = Depositos[i].gameObject;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("COLISIONE");
        //Debug.Log("TRIGGEREO CON " + other.gameObject.tag);
        if (other.gameObject == objetivoTrabajo && trabajo == "Minar" && other.gameObject.tag == "Mineral" && cantOro < capacity)
        {
            fsm.SendEvent((int)EventosAldeano.CollisionMine);
            other.gameObject.GetComponent<Recurso>().CantidadDeRecurso = other.gameObject.GetComponent<Recurso>().CantidadDeRecurso - Time.deltaTime;
            path.Clear();
        }

        if (cantOro >= capacity)
        {
            fsm.SendEvent((int)EventosAldeano.FullCapasity);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((trabajo == "Minar" || trabajo == "Llevar Oro") && other.gameObject.tag == "Centro Urbano" && cantOro > 0)
        {
            fsm.SendEvent((int)EventosAldeano.CollisionHouse);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((trabajo == "Minar" || trabajo == "Llevar Oro") && other.gameObject.tag == "Mineral")
        {
            fsm.SendEvent((int)EventosAldeano.FullCapasity);
        }
    }
    private void OnDrawGizmos()
    {
        if (path == null)
        {
            return;
        }
        foreach (Node n in path)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(n.transform.position, n.transform.localScale * 2.5f);
        }
    }
    //[SerializeField] private float cantOro;
    //[SerializeField] private float cantPiedra;
    //[SerializeField] private float cantAlimento;
    //[SerializeField] private float cantMadera;
    #region SetersAndGetters
    public void SetOro(float _cantOro) => cantOro = _cantOro;
    public float GetOro() { return cantOro; }

    public void SetPiedra(float _cantPiedra) => cantPiedra = _cantPiedra;
    public float GetPiedra() { return cantPiedra; }

    public void SetAlimento(float _cantAlimento) => cantAlimento = _cantAlimento;
    public float GetAlimento() { return cantAlimento; }

    public void SetMadera(float _cantMadera) => cantMadera = _cantMadera;
    public float GetMadera() { return cantMadera; }
    #endregion

}
