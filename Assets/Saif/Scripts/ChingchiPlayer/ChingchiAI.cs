using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChingchiAI : ChingChiCharacter
{

    private NavMeshAgent myAgent;

    [Header("Wander")]


    public float minArenaWidth;
    public float maxArenaWidth;
    public float minArenaDepth;
    public float maxArenaDepth;


    [SerializeField]
    private Vector3 destination;

    [SerializeField]
    private float minWanderSpeed;

    [SerializeField]
    private float maxWanderSpeed;

    [SerializeField]
    private float wanderSpeed;




    [Header("Chase")]


    [SerializeField]
    private List<Transform> targetEnemies;

    [SerializeField]
    private Transform currentTarget;

    [SerializeField]
    private float enemiesSortTimeLimit = 1f;
    [SerializeField]
    private float timeSinceEnemiesSorted = 0f;

    [SerializeField]
    private float minChaseSpeed;

    [SerializeField]
    private float maxChaseSpeed;

    [SerializeField]
    private float chaseSpeed;




    [Header("Search")]
    [SerializeField]
    private float timeSinceEnemiesSearched;

    [SerializeField]
    private float enemiesSearchTimeLimit;

    [SerializeField]
    private float detectionRange = 20;


    [SerializeField]
    private LayerMask enemyLayer;



    private NavMeshPath currentPath;
    // private NavMeshPath closestPath;

   


    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        chaseSpeed = Random.Range(minChaseSpeed, maxChaseSpeed);
        wanderSpeed = Random.Range(minWanderSpeed, maxWanderSpeed);


    }


    private void Update()
    {
        if (!isGamePlaying)
        {
            return;
        }
        Search();
        Move();
    }

    private void OnEnable()
    {
        GameManager.OnGamePlay += OnGamePlay;
    }

    private void OnDisable()
    {
        GameManager.OnGamePlay -= OnGamePlay;
    }


    private void OnGamePlay(bool gamePlayingState)
    {
        isGamePlaying = gamePlayingState;
    }




















    private void ChooseTarget()
    {
        //Transform closestTarget = null;
        float closestTargetDistance = float.MaxValue;
        NavMeshPath path = null;
        NavMeshPath shortestPath = null;

        for (int i = 0; i < targetEnemies.Count; i++)
        {
            if (targetEnemies[i] == null)
            {
                continue;
            }

            path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, targetEnemies[i].position, myAgent.areaMask, path))
            {
                float distance = Vector3.Distance(transform.position, path.corners[0]);

                for (int j = 1; j < path.corners.Length; j++)
                {
                    distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    shortestPath = path;
                }
            }
        }

        if (shortestPath != null)
        {
            myAgent.SetPath(shortestPath);
        }
    }

    private void GetRandomPathFromArena()
    {
        NavMeshPath path = new NavMeshPath(); ;
        NavMeshPath shortestPath = null;
        float closestTargetDistance = float.MaxValue;

        destination = new Vector3(Random.Range(minArenaWidth, maxArenaWidth), 0, Random.Range(minArenaDepth, maxArenaDepth));
        if (NavMesh.CalculatePath(transform.position, destination, myAgent.areaMask, path))
        {
            float distance = Vector3.Distance(transform.position, path.corners[0]);

            for (int j = 1; j < path.corners.Length; j++)
            {
                distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
            }

            if (distance < closestTargetDistance)
            {
                closestTargetDistance = distance;
                shortestPath = path;
            }
        }
        if (shortestPath != null)
        {
            myAgent.SetPath(shortestPath);
        }
    }


    private void Search()
    {
        if (timeSinceEnemiesSearched >= enemiesSearchTimeLimit)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                ChingChiCharacter enemy = hitCollider.GetComponent<ChingChiCharacter>();
                if (enemy != null && enemy != this)
                {
                    if (!targetEnemies.Contains(enemy.transform))
                    {
                        targetEnemies.Add(enemy.transform);
                        //Debug.Log("Found Enemy");
                    }
                }
            }
            timeSinceEnemiesSearched = 0.0f;
        }
        timeSinceEnemiesSearched += Time.deltaTime;
    }



    //---------------------------------------------OLD MOVE----------------------------------------------//


    private void Move()
    {
        if (timeSinceEnemiesSorted >= enemiesSortTimeLimit)
        {
            currentTarget = GetClosestEnemy(targetEnemies);
            timeSinceEnemiesSorted = 0.0f;
        }

        if (currentTarget != null)
        {
            Chase();
        }
        else
        {
            Wander();
        }
        timeSinceEnemiesSorted += Time.deltaTime;
    }

    private void Chase()
    {
        myAgent.SetDestination(currentTarget.position);
        if (myAgent.speed != chaseSpeed)
        {
            myAgent.speed = chaseSpeed;
        }
        if (Vector3.Distance(transform.position, currentTarget.position) > detectionRange)
        {
            currentTarget = null;
        }
        else
        {
            Wander();
        }

    }


    private void Wander()
    {
        if (myAgent.speed != wanderSpeed)
        {
            myAgent.speed = wanderSpeed;
        }

        if (myAgent.destination == null || Vector3.Distance(transform.position, myAgent.destination) <= myAgent.stoppingDistance)
        {
            myAgent.SetDestination(GetRandomPointFromArena());
        }
    }

    private Vector3 GetRandomPointFromArena()
    {
        //Debug.Log("Got New Waypoint");
        return new Vector3(Random.Range(minArenaWidth, maxArenaWidth), 0, Random.Range(minArenaDepth, maxArenaDepth));
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
                if (t != null)
                {
                    float dist = Vector3.Distance(t.position, currentPos);
                    if (dist > detectionRange)
                    {
                        farEnemies.Add(t);
                        //Debug.Log("Enemy Removed From List");
                    }
                }
            }
            //Sort Enemies
            if (allEnemies.Count > 0)
            {
                foreach (Transform t in allEnemies)
                {
                    if (t != null)
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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }


}
