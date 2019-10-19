using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicked : MonoBehaviour
{
    public float range;
    //public string objectSelect;
    // Update is called once per frame
    public static ObjectClicked instanceObjectClicked;
    private void Awake()
    {
        if (instanceObjectClicked == null)
        {
            instanceObjectClicked = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void Update()
    {
    }
    public Vector3 CheckHitMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform != null)
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        return Vector3.zero;

    }
}
