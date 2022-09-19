using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChingchiDamage : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> allChildMeshes;
    [SerializeField]
    private List<Color> originalColors;

    [SerializeField]
    private float flashTime = 0.2f;


    public event Action<float> OnDamageTaken;

    private IEnumerator defaultIenumerator;

    // Start is called before the first frame update
    void Awake()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        allChildMeshes = new List<MeshRenderer>();
        originalColors = new List<Color>();

        foreach (MeshRenderer mesh in meshes)
        {
            if (mesh.material.HasProperty("_Color"))
            {
                allChildMeshes.Add(mesh);
                originalColors.Add(mesh.material.color);
            }
        }
    }


   

    private void ApplyDamage(float Damage)
    {
        OnDamageTaken?.Invoke(Damage);

        if (defaultIenumerator != null)
        {
            StopCoroutine(defaultIenumerator);
        }

        defaultIenumerator = FlashDamage();
        StartCoroutine(defaultIenumerator);
    }


    private IEnumerator FlashDamage()
    {

        List<Color> preChangecolors = new List<Color>();

        foreach (var mesh in allChildMeshes)
        {
            mesh.material.color = Color.red;
            preChangecolors.Add(mesh.material.color);
        }

       

        float elapsed = 0.0f;

        while (elapsed < flashTime)
        {
            elapsed += Time.deltaTime;

            for (int i = 0; i < allChildMeshes.Count; i++)
            {
                allChildMeshes[i].material.color = Color.Lerp(preChangecolors[i], originalColors[i], elapsed / flashTime);
            }
            yield return null;
        }



        for (int i = 0; i < allChildMeshes.Count; i++)
        {
            allChildMeshes[i].material.color = originalColors[i];
        }

        preChangecolors.Clear();
        defaultIenumerator = null;
        yield break;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyDamage(-10);
        }
    }









}
