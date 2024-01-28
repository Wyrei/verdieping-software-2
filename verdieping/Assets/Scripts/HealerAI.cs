using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HealerAI : MonoBehaviour
{
   [SerializeField] private List<Transform> Ally;
   
   [SerializeField] private float rotationSpeed;
   [SerializeField] private float offset;
   Stats stats;
   
   [Space]
   [SerializeField] private float Timer;
   [SerializeField] private float ResetTimer;
   private float nullPoint;
   
   [Space]
   private Transform ally;
   private Transform lowestHPAlly;

   [SerializeField] private float range;
   
   void Start()
   {
      stats = GetComponent<Stats>();
        
      if (stats == null)
      {
         Debug.LogError("Stats script not found. Make sure it's on the same GameObject or a parent GameObject.");
      }
   }
   private void Update()
   {
      Timer -= Time.deltaTime;
      allyInRange();
   }

   void allyInRange()
   {
      Collider[] colliders = Physics.OverlapSphere(transform.position, range);

      foreach (Collider col in colliders)
      {
         if (col.CompareTag("Ally") && col.transform != transform)
         {
            if (!Ally.Contains(col.transform))
            {
               Ally.Add(col.transform);
               ally = Ally[0];
            }
            getClosestAlly();
         }
         else
         {
            Ally.Clear();
         }
      }
   }

   void getClosestAlly()
   {
      float lowestHP = Mathf.Infinity;
      Transform newLowestHPAlly = null;

      foreach (Transform allyTransform in Ally)
      {
         Stats allyStats = allyTransform.GetComponent<Stats>();
         if (allyStats != null && allyStats.CurrentHP < lowestHP)
         {
            lowestHP = allyStats.CurrentHP;
            newLowestHPAlly = allyTransform;
         }
      }

      if (newLowestHPAlly != null && newLowestHPAlly != lowestHPAlly)
      {
         Debug.Log("hello");
         lowestHPAlly = newLowestHPAlly;
         moveTowardsAlly();
      }
   }

   void moveTowardsAlly()
   {
      if (lowestHPAlly != null)
      {
         float DistanceToAlly = Vector3.Distance(transform.position, lowestHPAlly.position);

         if (DistanceToAlly < range && lowestHPAlly.GetComponent<Stats>().CurrentHP < lowestHPAlly.GetComponent<Stats>().HPMax)
         {
            // If the healer is close enough and not at full health, heal the ally
            if (DistanceToAlly < offset && stats.CurrentHP < 0.99f * stats.HPMax)
            {
               HealAlly(lowestHPAlly.GetComponent<Stats>());
               Debug.Log("Healing");
            }
            else
            {
               Debug.Log("Moving towards ally");
               // Move towards the ally
               float effectiveSpeed = Mathf.Min(stats.Speed, DistanceToAlly);
               Vector3 directionToAlly = (lowestHPAlly.position - transform.position).normalized;
               transform.position += directionToAlly * effectiveSpeed * Time.deltaTime;

               // Rotate towards the ally
               Quaternion rotationToAlly = Quaternion.LookRotation(directionToAlly);
               transform.rotation = Quaternion.Slerp(transform.rotation, rotationToAlly, rotationSpeed * Time.deltaTime);
            }
         }
      }
   }

   void HealAlly(Stats allyStats)
   {
      if (allyStats.CurrentHP < allyStats.HPMax)
      {
         if (Timer <= 0)
         {
            float healingAmount = 20.0f;
            allyStats.Healing(healingAmount);
            Timer = ResetTimer;
         }
      }
      else
      {
         getClosestAlly();
      }
   }
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.black;
      Gizmos.DrawWireSphere(transform.position, range);
   }
}
