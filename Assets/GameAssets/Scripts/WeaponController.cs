using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Gameplay
{
    public class WeaponController : MonoBehaviour
    {
        public float bulletForce;
        public Transform bulletOrigin;
        public GameObject bullet;
        [SerializeField] private int amountToPool;

        public List<GameObject> pooledObjects;
        private PlayerControls playerControls;

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

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(bullet);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }

            playerControls.ShootingControls.Fire.performed += FireBullet;
        }

        private void FireBullet(InputAction.CallbackContext context)
        {            
            if(context.control.IsPressed())
            {
                Debug.Log("Firing bullet");
                GameObject cloneBullet = GetPooledObject();
                if (cloneBullet != null)
                {
                    cloneBullet.transform.position = bulletOrigin.transform.position;
                    cloneBullet.transform.rotation = bulletOrigin.transform.rotation;
                    cloneBullet.GetComponent<Rigidbody>().AddForce(bulletForce * Vector3.forward, ForceMode.Impulse);
                    cloneBullet.SetActive(true);
                }
            }   
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            return null;
        }


    }
}

