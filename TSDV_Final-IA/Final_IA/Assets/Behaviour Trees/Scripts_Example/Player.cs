using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Player : MonoBehaviour
    {
        public float speed;
        public static Player InstancePlayer;
        void Awake()
        {
            if (InstancePlayer == null)
                InstancePlayer = this;
            else
                Destroy(gameObject);
        }
        void Update()
        {
            Movement();
        }
        public void Movement()
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
        }
    }
}
