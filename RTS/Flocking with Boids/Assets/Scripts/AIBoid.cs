using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoid : MonoBehaviour
{

    public Vector3 myDirection; //para donde mira
    public Vector3 FinalPosition;
    public ObjectClicked objectClicked;
    public float rootSpeed;
    public float radioDeVision;
    [SerializeField]
    private LayerMask layerMask;
    private Collider[] BoidsColliders;
    void Start()
    {
        if (ObjectClicked.instanceObjectClicked != null)
        {
            objectClicked = ObjectClicked.instanceObjectClicked;
        }
        myDirection = transform.forward;
    }


    void Update()
    {
        CheckMyDireccion();
        MovementLerp();
        CheckFinalPosition();
        CheckBoidsEnRango();
    }
    public void CheckFinalPosition()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            FinalPosition = objectClicked.CheckHitMouse();// ponerle normalized si no lo normalizo en las 
        }
    }
    public void MovementLerp()
    {
        transform.position += transform.forward / 50;
        transform.forward = Vector3.Slerp(transform.forward, FinalPosition, rootSpeed * Time.deltaTime);
    }
    public void CheckMyDireccion()
    {
        myDirection = transform.forward;
    }
    public void CheckBoidsEnRango()
    {
        BoidsColliders = Physics.OverlapSphere(transform.position, radioDeVision, layerMask);
    }
    public Collider[] GetBoidCollider()
    {
        return BoidsColliders;
    }

}
