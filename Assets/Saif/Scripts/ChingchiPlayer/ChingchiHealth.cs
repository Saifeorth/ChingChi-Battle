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

    public ChingChiCharacter Owner;


    private void Awake()
    {       
        myDamageHandler = GetComponent<ChingchiDamage>();
        Owner = GetComponent<ChingChiCharacter>();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        myDamageHandler.OnDamageTaken += ModifyHealth;   
    }


    private void OnDisable()
    {
        myDamageHandler.OnDamageTaken -= ModifyHealth;
    }


    public void ModifyHealth(float amount, ChingChiCharacter damager)
    {
        //Debug.Log("Health Modifid");
        currentHealth += amount;
        float currentHealthPct = currentHealth / maxHealth;
        OnHealthPctChanged?.Invoke(currentHealthPct);
        CheckDeath(damager);
    }


    private void CheckDeath(ChingChiCharacter damager)
    {
        if(currentHealth>0)
        {
            return;
        }
        else
        {
            damager.Kills += 1;
            OnDeath();
        }
    }

    public void OnDeath()
    {
        //GameObject explosion =  Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //Destroy(explosion, 3f);
        Owner.Deaths += 1;
        Owner.Die();
    }
}
