using System.Collections;
using UnityEngine;




    public class ChingChiCharacter : MonoBehaviour, ICharacter
    {
        [SerializeField]
        private bool isPlayable = false;

        [SerializeField]
        private string playerName = " ";

        public bool isGamePlaying;


        public bool isActive = false;



        public Rigidbody rb;
        public UnityEngine.AI.NavMeshAgent myAgent;
        public int Kills = 0;
        public int Deaths = 0;

        public Renderer[] childRenderers;
        public GameObject spawnVfx;
        public GameObject deathVfx;
        public GameObject healthBarGameObject;
        public float spawnTime = 2.0f;
        public float deathTime = 5.0f;
        public GameObject deathExplosionVfx;
        public AudioClip explosionSFx;
        public ScoreManager scoreManager;


        public string GetName()
        {
            return playerName;
        }


        public void SetName(string name)
        {
            playerName = name;
        }

        public bool IsPlayable()
        {
            return isPlayable;
        }

        public void SetActivation(bool activationState)
        {
            isActive = activationState;
        }


        public void Spawn()
        {
            StartCoroutine(SpawnPlayer());
        }


        private IEnumerator SpawnPlayer()
        {
            spawnVfx.SetActive(true);
            rb.isKinematic = true;
            myAgent.isStopped = true;
            DisableColliders();
            healthBarGameObject.SetActive(false);

            yield return new WaitForSeconds(spawnTime);

            for (int i = 0; i < childRenderers.Length; i++)
            {
                for (int j = 0; j < childRenderers.Length; j++)
                {
                    if (childRenderers[j].gameObject != spawnVfx)
                    {
                        childRenderers[j].gameObject.SetActive(false);
                    }
                }

                yield return new WaitForSeconds(0.1f);
                for (int j = 0; j < childRenderers.Length; j++)
                {
                    if (childRenderers[j].gameObject != spawnVfx)
                    {
                        childRenderers[j].gameObject.SetActive(true);
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }

            spawnVfx.SetActive(false);
            healthBarGameObject.SetActive(true);
            EnableColliders();
            myAgent.isStopped = false;
            rb.isKinematic = false;
            SetActivation(true);
        }




        public void Die()
        {
            StartCoroutine(PlayerDeath());
        }



        private IEnumerator PlayerDeath()
        {
            SetActivation(false);
            DisableColliders();
            myAgent.isStopped = true;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            deathVfx.SetActive(true);
            AudioSource.PlayClipAtPoint(explosionSFx, transform.position);
            healthBarGameObject.SetActive(false);

            for (int j = 0; j < childRenderers.Length; j++)
            {
                if (childRenderers[j].gameObject != deathVfx)
                {
                    childRenderers[j].gameObject.SetActive(false);
                }
            }

            yield return new WaitForSeconds(deathTime);

            deathVfx.SetActive(false);
            gameObject.SetActive(false);
        }



        private void DisableColliders()
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                if (col.enabled)
                {
                    col.enabled = false;
                }

            }
        }



        private void EnableColliders()
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                if (!col.enabled)
                {
                    col.enabled = true;
                }
            }
        }

    }

