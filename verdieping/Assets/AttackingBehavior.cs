using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingBehavior : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private Transform player;
    
    [SerializeField] private List<Transform> Enemies;

    [SerializeField] private float speed;

    [SerializeField] private float offset;

    [SerializeField] private GameObject AttackPrefab;
    

    private Transform closestEnemy;
    
    void Update()
    {
        enemieinrange();
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
                }
                getClosestEnemy();
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
            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.position, speed * Time.deltaTime);
        }
    }

    void stopmoving()
    {
        //Instantiate(AttackPrefab, 2f, Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
