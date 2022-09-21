using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    [SerializeField]
    private float pushBackForce = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if(rb!=null)
        {
            Debug.Log("Player Pushed Back");
            rb.AddForce(-rb.transform.forward.normalized * pushBackForce, ForceMode.Impulse);
        }


    }

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("Player Pushed Back");
            rb.AddForce(-rb.transform.forward.normalized * pushBackForce, ForceMode.Impulse);
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("Player Pushed Back");
            rb.velocity = Vector3.zero;
        }
    }


}
