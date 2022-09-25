using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingchiController : ChingChiCharacter
{
    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private float turnSpeed = 360;

    [SerializeField]
    private Rigidbody rb;



    private Vector3 _input;

    [SerializeField]
    private Transform frontTireTransform;


    [SerializeField]
    private ParticleSystem[] movementVFx;


    [SerializeField]
    private AudioSource playerAudioSource;

    [SerializeField]
    private AudioClip playerIdleSound;

    [SerializeField]
    private AudioClip playerMovingSound;

    //private Vector3 chingchiVelocity;


    private void Awake()
    {

    }


    public override void Spawn()
    {
        StartCoroutine(SpawnPlayer());
    }


    private IEnumerator SpawnPlayer()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].setActive(false);
            }

            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].setActive(true);
            }

            yield return new WaitForSeconds(0.1f);
        }

        SetActivation(true);
    }


    public override void Die()
    {
        StartCoroutine(PlayerDeath());
    }



    private IEnumerator PlayerDeath()
    {
        SetActivation(false);
        yield return new WaitForSeconds(0.1f);
    }




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager.OnGamePlay += OnGamePlay;
        Spawn();
    }

    private void OnDisable()
    {
        GameManager.OnGamePlay -= OnGamePlay;
    }


    private void OnGamePlay(bool gamePlayingState)
    {
        isGamePlaying = gamePlayingState;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGamePlaying || !isActive)
        {
            return;
        }

        GatherInput();
        Look();
        UpdateVfxAndSfx();
        //CalculateMovementVelocity();
    }


    private void FixedUpdate()
    {
        //rb.velocity = chingchiVelocity * speed;

        Move();
    }



    void GatherInput()
    {
        _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }


    void Look()
    {
        if (_input != Vector3.zero)
        {
            var _relative = (transform.position + _input) - transform.position;
            var rot = Quaternion.LookRotation(_relative, Vector3.up);
           // frontTireTransform.transform.rotation = Quaternion.RotateTowards(frontTireTransform.rotation, rot, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }

    }

    void Move()
    {
        Vector3 pos = transform.position + (transform.forward * _input.magnitude) * Time.deltaTime *speed;
        rb.MovePosition(pos);
    }


    private void UpdateVfxAndSfx()
    {
        if (_input.magnitude > 0)
        {
            StartMovementVfx();
            StartMovementSfx();
        }
        else 
        {
            StopMovementVfx();
            StopMovementSfx();
        }
    }

    private void StartMovementVfx()
    {
        for (int i = 0; i < movementVFx.Length; i++)
        {
            if (!movementVFx[i].isPlaying)
            {
                movementVFx[i].Play();
            }
        }
    }

    private void StartMovementSfx()
    {
        if (playerAudioSource.clip != playerMovingSound)
        {
            playerAudioSource.clip = playerMovingSound;
            playerAudioSource.Stop();
            if (!playerAudioSource.isPlaying)
            {
                playerAudioSource.Play();
            }
        }
    }

    private void StopMovementSfx()
    {
        if (playerAudioSource.clip != playerIdleSound)
        {
            playerAudioSource.clip = playerIdleSound;
            playerAudioSource.Stop();
            if (!playerAudioSource.isPlaying)
            {
                playerAudioSource.Play();
            }
        }
    }

    private void StopMovementVfx()
    {
        for (int i = 0; i < movementVFx.Length; i++)
        {
            if (movementVFx[i].isPlaying)
            {
                movementVFx[i].Stop();
            }
        }
    }

}