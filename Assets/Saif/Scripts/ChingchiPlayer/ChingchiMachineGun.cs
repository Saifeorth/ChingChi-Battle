using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingchiMachineGun : MonoBehaviour
{
    [SerializeField]
    private List<Transform> targetEnemies;

    [SerializeField]
    private LayerMask enemyLayer;


    [SerializeField]
    private Transform currentTarget;
  
    [SerializeField]
    private Transform turretTransform;

    [SerializeField]
    private float turretRotationSpeed = 0.1f;



    [Header("Sort")]
    [SerializeField]
    private float enemiesSortTimeLimit = 2f;
    [SerializeField]
    private float timeSinceEnemiesSorted = 0f;

    [Header("Search")]
    [SerializeField]
    private float detectionRange = 20f;
    [SerializeField]
    private float enemiesSearchTimeLimit = 1f;
    [SerializeField]
    private float timeSinceEnemiesSearched = 0f;



    [Header("Firing")]

    [SerializeField]
    private float fireRate = 2;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private GameObject bullet;



    private Quaternion gunTargetRotation;
    private Quaternion gunInitialRotation;

    void Start()
    {
        targetEnemies = new List<Transform>();
        gunInitialRotation = turretTransform.localRotation;
    }


    private void Search()
    {
        if (timeSinceEnemiesSearched >= enemiesSearchTimeLimit)
        {
            Collider[] hitColliders = Physics.OverlapSphere(turretTransform.position, detectionRange,enemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                ChingchiMachineGun gun = hitCollider.GetComponentInChildren<ChingchiMachineGun>();
                if (gun != null && gun != this)
                {
                    if (!targetEnemies.Contains(gun.transform))
                    {
                        targetEnemies.Add(gun.transform);
                        Debug.Log("Found Enemy");
                    }
                }
            }




            //RaycastHit hit;

            //if (Physics.SphereCast(turretTransform.position, detectionRange, turretTransform.position, out hit, enemyLayer))
            //{
            //    ChingchiMachineGun gun = hit.collider.GetComponentInChildren<ChingchiMachineGun>();

            //    if (gun != null && gun != this)
            //    {
            //        if (!targetEnemies.Contains(gun.transform))
            //        {
            //            targetEnemies.Add(gun.transform);
            //            Debug.Log("Found Enemy");
            //        }
            //    }
            //}
            timeSinceEnemiesSearched = 0.0f;
        }
        timeSinceEnemiesSearched += Time.deltaTime;

    }


    Transform GetClosestEnemy(List<Transform> targets)
    {
        Transform tMin = null;
        List<Transform> farEnemies = new List<Transform>();
        if (targets.Count > 0)
        {
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;

            List<Transform> allEnemies = targets;
           

            //Find Far Targets
            foreach (Transform t in targets)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist > detectionRange)
                {
                    farEnemies.Add(t);
                    Debug.Log("Enemy Removed From List");
                }
            }






            //Sort Enemies
            if (allEnemies.Count > 0)
            {
                foreach (Transform t in allEnemies)
                {

                    float dist = Vector3.Distance(t.position, currentPos);
                    if (dist < minDist)
                    {
                        tMin = t;
                        minDist = dist;
                    }
                }
            }          
        }


        //Clean targets
        for (int i = 0; i < targetEnemies.Count; i++)
        {
            for (int j = 0; j < farEnemies.Count; j++)
            {
                if (farEnemies[j] == targetEnemies[i])
                {
                    targetEnemies.Remove(targetEnemies[i].transform);
                }
            }
        }

        return tMin;
    }


    private void Update()
    {
        Search();
        LookAtTarget();

    }


    private void Fire()
    {
       
    }



    private void LookAtTarget()
    {
        if (timeSinceEnemiesSorted >= enemiesSortTimeLimit)
        {
            currentTarget = GetClosestEnemy(targetEnemies);
            timeSinceEnemiesSorted = 0.0f;
        }
       

        if (currentTarget == null)
        {
            gunTargetRotation = gunInitialRotation;
            turretTransform.localRotation = Quaternion.Lerp(turretTransform.localRotation, gunTargetRotation, Time.deltaTime * turretRotationSpeed);

        }
        else 
        {
            gunTargetRotation = Quaternion.LookRotation(currentTarget.position - turretTransform.position);
            turretTransform.rotation = Quaternion.Lerp(turretTransform.rotation, gunTargetRotation, Time.deltaTime * turretRotationSpeed);
        }

       
        timeSinceEnemiesSorted += Time.deltaTime;
    }



    //Add Targets On Detection
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == enemyLayer)
    //    {

    //        ChingchiMachineGun gun = other.gameObject.GetComponentInChildren<ChingchiMachineGun>();

    //        if (gun!=null && gun !=this)
    //        {
    //            targets.Add(gun.transform);
    //            Debug.Log("Found Enemy");
    //        }
    //    }
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == enemyLayer)
    //    {
    //        ChingchiMachineGun gun = other.gameObject.GetComponentInChildren<ChingchiMachineGun>();
    //        if (gun != null && gun != this)
    //        {
    //            targets.Remove(gun.transform);
    //            Debug.Log("Lost Enemy");
    //        }
    //    }
    //}

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(turretTransform.position, detectionRange);
    }





}
