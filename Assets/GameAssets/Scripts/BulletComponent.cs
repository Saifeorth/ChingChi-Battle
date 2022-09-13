using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class BulletComponent : MonoBehaviour
    {
        private float timer = 0.0f;
        [SerializeField] private float maxTimeAllowedInSec = 1.0f;

        [SerializeField] private float damage;

        private void Start()
        {
            timer = 0.0f;
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(transform.forward * 100f, ForceMode.Impulse);
            }
        }

        private void Update()
        {
            if (timer < maxTimeAllowedInSec)
            {
                timer += Time.deltaTime;
            }
            else
            {
                this.gameObject.SetActive(false);
                timer = 0.0f;
            }
        }


        private void OnCollisionEnter(Collision other)
        {

            if (other.transform == other.transform.parent) // target is the attacker
            {
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (other.transform.TryGetComponent(out CarHealth carHealth))
                {
                    if (carHealth.currentCarHealth <= 0)
                    {
                        Destroy(other.gameObject);
                        return;
                    }

                    carHealth.currentCarHealth -= damage;
                }
            }
        }
    }

}
