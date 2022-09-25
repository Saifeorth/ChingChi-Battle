using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingchiBullet : MonoBehaviour
{

    Rigidbody Rb;
    [SerializeField]
    float moveSpeed = 100f;

    [SerializeField]
    float damageForce = 60f;

    [SerializeField]
    private float selfDestroy= 3f;

    [SerializeField]
    private float Damage = 10f;

    [SerializeField]
    private ChingChiCharacter owner;

    [SerializeField]
    private GameObject hitImpactPrefab;


    [SerializeField]
    private AudioClip hitsoundFX;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }




    private void OnEnable()
    {
        Destroy(this.gameObject, selfDestroy);
    }


    public void Setup(Vector3 shootDir, ChingChiCharacter myOwner)
    {
        owner = myOwner;
        if (owner.IsPlayable())
        {
            CameraShaker.Instance.ShakeOnce(1f, 1f, 0.15f, 0.15f);
        }
        Rb.AddForce(shootDir * moveSpeed, ForceMode.Impulse);
    }



    private void OnTriggerEnter(Collider other)
    {
        ChingchiDamage damaged = other.gameObject.GetComponent<ChingchiDamage>();
        if (damaged!=null && damaged.Owner != owner && damaged.Owner.isActive)
        {
            damaged.ApplyDamage(Damage, owner);
            Rigidbody rb =  damaged.GetComponent<Rigidbody>();
            Vector3 pushDirection = (rb.transform.position- transform.position);
            rb.AddForce(pushDirection * damageForce, ForceMode.Impulse);
            GameObject hitImpactVfx = Instantiate(hitImpactPrefab, transform.position, Quaternion.identity);
            Destroy(hitImpactVfx, 2f);
            AudioSource.PlayClipAtPoint(hitsoundFX, transform.position);
            Rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
            return;
            //Vector3 tempContactPoint = pushDirection + transform.position;        
            //gameObject.SetActive(false);
        }

        if (damaged ==null)
        {
            //GameObject hitImpactVfx = Instantiate(hitImpactPrefab, transform.position, Quaternion.identity);
            //Destroy(hitImpactVfx, 5f);
            AudioSource.PlayClipAtPoint(hitsoundFX, transform.position);
            Rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
            return;
        }


         if (damaged.Owner == owner)
        {

        }
       





      
    }


}
