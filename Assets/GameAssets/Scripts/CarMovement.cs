using System;
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

        [SerializeField] private Transform centerOfMass;
        [SerializeField] private WheelCollider frontLeftWheelCollider;
        [SerializeField] private WheelCollider frontRightWheelCollider;
        [SerializeField] private WheelCollider backLeftWheelCollider;
        [SerializeField] private WheelCollider backRightWheelCollider;

        [SerializeField] private Transform frontLeftWheelTransform;
        [SerializeField] private Transform frontRightWheelTransform;
        [SerializeField] private Transform backLeftWheelTransform;
        [SerializeField] private Transform backRightWheelTransform;

        public enum CarState
        {
            Idle,
            Moving,
            Stop,
        }

        public CarState carState;
        private float brakeForce;
        private bool isBreaking;
       

        private void Awake()
        {
            playerControls = new PlayerControls();
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.centerOfMass = centerOfMass.transform.position;
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
            UpdateWheels();
        }

        public void AcceleratedMovement()
        {
            movementInput = playerControls.CarControls.CarMovement.ReadValue<Vector2>();
            isBreaking = playerControls.CarControls.CarBrake.IsPressed();
            
            frontLeftWheelCollider.motorTorque = speed * movementInput.y;
            frontRightWheelCollider.motorTorque = speed * movementInput.y;

            brakeForce = isBreaking ? 3000f : 0f;

            frontLeftWheelCollider.brakeTorque = brakeForce;
            frontRightWheelCollider.brakeTorque = brakeForce;
            backLeftWheelCollider.brakeTorque = brakeForce;
            backRightWheelCollider.brakeTorque = brakeForce;
            carState = CarState.Moving;
        }

        public void SteeringMovement()
        {
            currentAngle = maxAngle * movementInput.x;
            frontLeftWheelCollider.steerAngle = currentAngle;
            frontRightWheelCollider.steerAngle = currentAngle; 
        }

        private void UpdateWheels()
        {
            UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
            UpdateWheelPos(backLeftWheelCollider, backLeftWheelTransform);
            UpdateWheelPos(backRightWheelCollider, backRightWheelTransform);
        }

        private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            trans.rotation = rot;
            trans.position = pos;
        }
    }
}

