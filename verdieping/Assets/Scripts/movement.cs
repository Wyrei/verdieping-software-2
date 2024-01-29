using UnityEngine;

public class Movement : MonoBehaviour
{
    Stats stats;
    public float changeInterval = 2f;
    public float followRadius = 5f;
    public float followOffSet = 1.5f;
    public float RotationSpeed;
    private Vector3 formationOffSet;

    private float timer;
    private Vector3 randomDirection;
    private Transform leader;
    public Animator animator;

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
        
    }

    public void manageMovement()
    {
        if (IsLeader())
        {
            MoveRandomly();
            CheckForLeader();
        }
        else
        {
            FollowLeader();
        }
    }

    void MoveRandomly()
    {
        transform.Translate(randomDirection * stats.Speed * Time.deltaTime);
        
        Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);

        animator.SetTrigger("passivewalking");
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            GetRandomDirection();
        }
    }

    void GetRandomDirection()
    {
        float angle = Random.Range(-90f, 90f) * Mathf.Deg2Rad;
        
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);
        
        randomDirection = new Vector3(x, 0f, z).normalized;

        timer = changeInterval;
    }

    void FollowLeader()
    {
        if (leader != null)
        {
            Vector3 directionToLeader = (leader.position - transform.position).normalized;
            
            float distanceToLeader = Vector3.Distance(transform.position, leader.position);
            
            Vector3 targetPosition = leader.position - directionToLeader * followOffSet;
            
            targetPosition -= directionToLeader * followOffSet;
            
            float effectiveSpeed = stats.Speed;
            if (distanceToLeader > followOffSet * 2)
            {
                effectiveSpeed *= 2f;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, effectiveSpeed * Time.deltaTime);
            
            Quaternion targetRotation = Quaternion.LookRotation(directionToLeader);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
        }
    }

    void CheckForLeader()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, followRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Ally") && collider.transform != transform)
            {
                collider.GetComponent<Movement>().SetLeader(transform);
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
