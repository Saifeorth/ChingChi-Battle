using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{

    [SerializeField]
    private Vector3 offset;

    private Transform targetTransform;


    private void OnEnable()
    {
        Spawner.OnPlayeSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        Spawner.OnPlayeSpawned -= OnPlayerSpawned;
    }

    private void FixedUpdate()
    {
        if (targetTransform != null)
        {
            transform.position = targetTransform.position + offset;
        }

    }


    private void OnPlayerSpawned(ChingChiCharacter player)
    {
        targetTransform = player.transform;
    }






}
