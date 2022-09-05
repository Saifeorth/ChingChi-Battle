using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace Gameplay
{
    public class PlayerInput : MonoBehaviour
    {
        public InputAction carControls;
        public InputAction weaponControls;
        public PlayerControls playerControls;

        public static event Action OnFireInput;


        private void Awake()
        {
            playerControls = new PlayerControls();            
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }

}
