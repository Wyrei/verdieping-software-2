using System.Collections.Generic;
using UnityEngine;

public class HealerAI : MonoBehaviour
{
   [SerializeField] private List<Transform> Ally;
   
   [SerializeField] private float rotationSpeed;
   [SerializeField] private float offset;
   Stats stats;
   Movement _movement;
   
   [Space]
   [SerializeField] private float Timer;
   [SerializeField] private float ResetTimer;
   private float nullPoint;
   
   [Space]
   public Transform ally;
   public Transform lowestHPAlly;

   [SerializeField] private float range;
   
   void Start()
   {
      stats = GetComponent<Stats>();
      _movement = GetComponent<Movement>();
        
      if (stats == null)
      {
         Debug.LogError("Stats script not found. Make sure it's on the same GameObject or a parent GameObject.");
      }
   }
   private void Update()
   {
      Timer -= Time.deltaTime;
      allyInRange();
      Stats allyStats = lowestHPAlly.GetComponent<Stats>();
      if (allyStats.CurrentHP > 0.99f * allyStats.HPMax)
      {
         _movement.manageMovement();
      }
      
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

      foreach (Transform allyTransform in Ally)
      {
         Stats allyStats = allyTransform.GetComponent<Stats>();
         if (allyStats != null && allyStats.CurrentHP < lowestHP)
         {
            lowestHP = allyStats.CurrentHP;
            lowestHPAlly = allyTransform;
            moveTowardsAlly();
         }
      }
      
   }

   void moveTowardsAlly()
   {
      if (lowestHPAlly != null)
      {
         float DistanceToAlly = Vector3.Distance(transform.position, lowestHPAlly.position);

         if (DistanceToAlly < range && lowestHPAlly.GetComponent<Stats>().CurrentHP < lowestHPAlly.GetComponent<Stats>().HPMax)
         {
            Stats allyStats = lowestHPAlly.GetComponent<Stats>();
            if (DistanceToAlly < offset && allyStats.CurrentHP < 0.99f * allyStats.HPMax)
            {
               HealAlly(allyStats);
            }
            else
            {
               float effectiveSpeed = Mathf.Min(stats.Speed, DistanceToAlly);
               transform.position = Vector3.MoveTowards(transform.position,  lowestHPAlly.position, effectiveSpeed * Time.deltaTime);
            }

            Vector3 directionToAlly = (lowestHPAlly.position - transform.position).normalized;
            
            Quaternion rotationToAlly = Quaternion.LookRotation(directionToAlly);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToAlly, rotationSpeed * Time.deltaTime);
            
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
   }
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.black;
      Gizmos.DrawWireSphere(transform.position, range);
   }
}
