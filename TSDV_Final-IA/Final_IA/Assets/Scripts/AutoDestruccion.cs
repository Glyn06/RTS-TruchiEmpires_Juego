using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruccion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float timeAutoDestruccion = 1.5f;
    

    // Update is called once per frame
    void Update()
    {
        if (timeAutoDestruccion > 0)
        {
            timeAutoDestruccion = timeAutoDestruccion - Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
