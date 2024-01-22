using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MageAI : MonoBehaviour
{
    [SerializeField] private List<Transform> Enemies;

    [Space]
    Stats stats;
    [SerializeField] private float rotationSpeed;

    [Space]
    [SerializeField] private float Timer;
    [SerializeField] private float ResetTimer;
    private float nullPoint;
    
    [Space]
    [SerializeField] private float offset;
    [SerializeField] private float range;
    [SerializeField] private GameObject AttackPrefab;
    
    private Transform closestEnemy;
    private Transform enemy;
    private Transform enemyTransform;

    void Start()
    {
        stats = GetComponent<Stats>();
        
        if (stats == null)
        {
            Debug.LogError("Stats script not found. Make sure it's on the same GameObject or a parent GameObject.");
        }
    }

    void Update()
    {
        if (stats.CurrentHP < stats.HPMax / 2)
        {
            
        }
        else
        {
            enemieinrange(); 
        }
        
    }
    void enemieinrange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.transform != transform)
            {
                if (!Enemies.Contains(collider.transform))
                {
                    Enemies.Add(collider.transform);
                    enemy = Enemies[0];
                    enemyTransform = enemy;
                }
                getClosestEnemy();
            }
            else
            {
                Enemies.Clear();
            }
        }
    }
    void getClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in Enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
                moveTowardsClosestEnemy();
            }
        }
    }
    
    void moveTowardsClosestEnemy()
    {
        if (Vector3.Distance(transform.position,closestEnemy.position) < offset)
        {
            stopmoving();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.position, stats.Speed * Time.deltaTime);
        }
        
        Vector3 directionToEnemy = (enemy.position - transform.position).normalized;
        Quaternion rotationToEnemy = Quaternion.LookRotation(directionToEnemy);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToEnemy, rotationSpeed * Time.deltaTime);
    }

    void stopmoving()
    {
        Timer -= Time.deltaTime;
        if (Timer <= nullPoint)
        {
            Vector3 SpawnPos = transform.position + 2 * transform.forward;
        
            GameObject a = Instantiate(AttackPrefab, SpawnPos, Quaternion.identity);

            if (enemyTransform)
            {
                Vector3 directionToEnemy = (enemyTransform.position - a.transform.position).normalized;
                a.GetComponent<Rigidbody>().velocity = directionToEnemy * stats.Speed;
            }
            Timer = ResetTimer;
        }
    }
    
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
