using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingchiBullet : MonoBehaviour
{

    Rigidbody Rb;
    [SerializeField]
    float moveSpeed = 100f;

    [SerializeField]
    private float selfDestroy= 3f;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }




    private void OnEnable()
    {
        Destroy(this.gameObject, selfDestroy);
    }


    public void Setup(Vector3 shootDir)
    {
        Rb.AddForce(shootDir * moveSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ChingchiHealth>())
        {
            //Damage Player




        }
        Rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }


}
