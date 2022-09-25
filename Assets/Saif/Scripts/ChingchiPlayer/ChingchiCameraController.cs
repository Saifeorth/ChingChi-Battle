using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingchiCameraController : MonoBehaviour
{
    public Transform followTransform;
    //public Transform cameraTransform;

   // public float movementSpeed;
    public float movementTime;

    private void OnEnable()
    {
        Spawner.OnPlayeSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        Spawner.OnPlayeSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(ChingChiCharacter player)
    {
        followTransform = player.transform;
    }


    private void FixedUpdate()
    {
        if (followTransform !=null)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, movementTime * Time.deltaTime);
            //transform.rotation = Quaternion.Lerp(transform.rotation, followTransform.position, Time.deltaTime * movementTime);
        }
    }
}
