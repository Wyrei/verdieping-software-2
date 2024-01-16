using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class movement : MonoBehaviour
{
    Stats stats; 
    public float changeInterval = 2f;
    public float followRadius = 5f;
    public float followOffSet = 1.5f;
    
    private float timer;
    private Vector3 randomDirection;
    private Transform leader;
    private Vector3 formationOffSet;

    void Start()
    {
        GetRandomDirection();
        stats = GetComponent<Stats>();
        
        if (stats == null)
        {
            Debug.LogError("Stats script not found. Make sure it's on the same GameObject or a parent GameObject.");
        }
    }

    void Update()
    {
        if (IsLeader())
        {
            MoveRandomly();
        }
        else
        {
            followLeader();
        }
        
    }

   
    void MoveRandomly()
    {
        transform.Translate(randomDirection * stats.Speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            GetRandomDirection();
        }
        CheckForFollowers();
    }

    void GetRandomDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        timer = changeInterval;
    }

    void CheckForFollowers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, followRadius);

        
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") && collider.transform != transform)
            {
                collider.GetComponent<movement>().SetLeader(transform);
                
            }
        }
    }

    void SetLeader(Transform newLeader)
    {
        leader = newLeader;
        if (!IsLeader())
        {
            formationOffSet = transform.position - leader.position; 
        }
        
    }

    void followLeader()
    {
        if (leader != null)
        {
            Vector3 directionToLeader = (leader.position - transform.position).normalized;
            
            Vector3 targetPosition = leader.position + directionToLeader * (followOffSet + formationOffSet.magnitude * 1.5f);
            
            transform.Translate(directionToLeader * stats.Speed * Time.deltaTime / 20);
            
            transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime);

        }
    }

    bool IsLeader()
    {
        return leader == null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
}
