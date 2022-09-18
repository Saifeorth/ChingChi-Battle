using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingchiCameraController : MonoBehaviour
{
    public Transform followTransform;
    public Transform cameraTransform;

    public float movementSpeed;
    public float movementTime;

    public float rotationSpeed;
    public Vector3 zoomAmount;


    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;


    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!followTransform)
        {
            HandleMovementInput();
        }
        else 
        {
            
        }


      
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
        }

        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }


    private void LateUpdate()
    {
        if (followTransform)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, movementTime * Time.deltaTime);
            //transform.rotation = Quaternion.Lerp(transform.rotation, followTransform.position, Time.deltaTime * movementTime);
        }
    }
}
