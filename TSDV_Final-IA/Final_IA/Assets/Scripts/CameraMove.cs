using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraAnchor;

    private int border;
    private Vector2 screenBorder;
    private Vector2 middleScreen;

    private Vector3 movement;
    void Start()
    {
        border = 5;
        middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        this.screenBorder = new Vector2(Screen.width - border, Screen.height - border);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        //{
            Vector2 mouse = Input.mousePosition;
            if ((Input.mousePosition.x < border) ||
                (Input.mousePosition.x > screenBorder.x) ||
                (Input.mousePosition.y < this.border) ||
                (Input.mousePosition.y > this.screenBorder.y))
            {
                movement = mouse - middleScreen;
                movement.z = movement.y;
                movement.y = 0;
                movement = movement.normalized / 2f;

                cameraAnchor.Translate(movement);
            }


        //}
    }
}
