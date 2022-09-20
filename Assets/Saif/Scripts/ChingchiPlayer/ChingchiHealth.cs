using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChingchiHealth : MonoBehaviour
{

    [SerializeField]
    private float maxHealth = 100f;


    [SerializeField]
    private float currentHealth;


    [SerializeField]
    private ChingchiDamage myDamageHandler;


    public event Action<float> OnHealthPctChanged;


    [SerializeField]
    private GameObject explosionPrefab;


    private void Awake()
    {       
        myDamageHandler = GetComponent<ChingchiDamage>();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        myDamageHandler.OnDamageTaken += ModifyHealth;   
    }


    private void OnDisable()
    {
        myDamageHandler.OnDamageTaken -= ModifyHealth;
    }


    public void ModifyHealth(float amount)
    {
        //Debug.Log("Health Modifid");
        currentHealth += amount;
        float currentHealthPct = currentHealth / maxHealth;
        OnHealthPctChanged?.Invoke(currentHealthPct);
        CheckDeath();
    }


    private void CheckDeath()
    {
        if(currentHealth>0)
        {
            return;
        }
        else
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
