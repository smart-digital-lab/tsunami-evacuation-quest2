using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNavigation : MonoBehaviour
{

    //   public float mouseSpeed = 100f;
    //  public Transform playerBody;
    //  float xRotation = 0f;

    //  void Start()
    // {
    //     Cursor.lockState = CursorLockMode.Locked;
    //  }

    //  void Update()
    //  {
    //     float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
    //     float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;


    //      xRotation -= mouseY;
    //     xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    //    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    //    playerBody.Rotate(Vector3.up * mouseX);
    // }
    //}





    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
