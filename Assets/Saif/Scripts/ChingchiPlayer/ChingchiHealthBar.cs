using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class ChingchiHealthBar : MonoBehaviour
{

    [SerializeField]
    private Image healthBarImage;

    [SerializeField]
    private float updateSpeedSeconds = 0.5f;


    private IEnumerator defaultIenumerator;

    [SerializeField]
    private ChingchiHealth playerHealth;





    [SerializeField]
    private GameObject explosionPrefab;


    [SerializeField]
    private TextMeshProUGUI playerNameText;

    private void Awake()
    {
        playerHealth = GetComponentInParent<ChingchiHealth>();
        playerNameText.text = GetComponentInParent<ChingChiCharacter>().GetName();       
        playerHealth.OnHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        if (defaultIenumerator != null)
        {
            StopCoroutine(defaultIenumerator);
        }

        defaultIenumerator = ChangeToPct(pct);
        StartCoroutine(defaultIenumerator);   
    }


    private void OnDisable()
    {
        playerHealth.OnHealthPctChanged -= HandleHealthChanged;
    }


    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = healthBarImage.fillAmount;
        float elapsed = 0.0f;


        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthBarImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        healthBarImage.fillAmount = pct;
        defaultIenumerator = null;
        yield break;

    }

 

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
