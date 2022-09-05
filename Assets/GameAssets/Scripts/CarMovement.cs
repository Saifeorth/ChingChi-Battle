using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class CarMovement : MonoBehaviour
    {
        public float speed;
        public Vector3 movementInput;
        private PlayerControls playerControls;
        private float currentAngle;
        public float maxAngle = 35f;

        private Rigidbody rigidbody;

        [SerializeField] private WheelCollider frontLeftWheelCollider;
        [SerializeField] private WheelCollider frontRightWheelCollider;
        [SerializeField] private WheelCollider backLeftWheelCollider;
        [SerializeField] private WheelCollider backRightWheelCollider;


        public enum CarState
        {
            Idle,
            Moving,
            Stop,
        }

        public CarState carState;

        private void Awake()
        {
            playerControls = new PlayerControls();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            playerControls.Enable();

        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void FixedUpdate()
        {
            AcceleratedMovement();
            SteeringMovement();
        }

        public void AcceleratedMovement()
        {
            movementInput = playerControls.CarControls.CarMovement.ReadValue<Vector2>();
            frontLeftWheelCollider.motorTorque = speed * movementInput.y;
            frontRightWheelCollider.motorTorque = speed * movementInput.y;
            carState = CarState.Moving;
        }

        public void SteeringMovement()
        {
            currentAngle = maxAngle * movementInput.x;
            frontLeftWheelCollider.steerAngle = currentAngle;
            frontRightWheelCollider.steerAngle = currentAngle; 
        }

    }
}

