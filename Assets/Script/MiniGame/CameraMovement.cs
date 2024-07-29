using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    public bool canMove;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (canMove == true)
        {
            Vector3 movement = Vector3.zero;

            if (Input.GetKey(KeyCode.Z))
            {
                movement += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement += Vector3.back;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                movement += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement += Vector3.right;
            }

            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                float newFOV = cam.fieldOfView - scroll * zoomSpeed;
                Debug.Log(newFOV);
                cam.fieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);
            }
        }
    }
}
