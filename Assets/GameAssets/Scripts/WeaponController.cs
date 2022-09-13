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
        public float rateOfFire;
        public GameObject bullet;
        [SerializeField] private int amountToPool;

        public List<GameObject> pooledObjects;
        private PlayerControls playerControls;

        private bool canFire = true;

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

        }

        private void Update()
        {
            if(canFire)
            {
                StartCoroutine(IE_DelayedShot(rateOfFire));
            }
        }

        private void FireBullet()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (pooledObjects[i].activeInHierarchy)
                {
                    if (i >= 3)
                    {
                        pooledObjects[i - 3].SetActive(false);
                    }
                }
            }

            if (playerControls.ShootingControls.Fire.IsPressed())
            {
                Debug.Log("Firing bullet");
                GameObject cloneBullet = GetPooledObject();
                if (cloneBullet != null)
                {
                    cloneBullet.transform.position = bulletOrigin.transform.position;
                    cloneBullet.transform.rotation = bulletOrigin.transform.rotation;
                    cloneBullet.SetActive(true);
                    cloneBullet.GetComponent<Rigidbody>().AddForce(bulletForce * transform.forward, ForceMode.Impulse);
                    
                }
            }

        }

        public IEnumerator IE_DelayedShot(float sec)
        {
            canFire = false;
            FireBullet();
            yield return new WaitForSeconds(sec);
            canFire = true;
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

