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
            CheckForLeader();
        }
        else
        {
            FollowLeader();
        }
    }

    void MoveRandomly()
    {
        // Move in the current direction
        transform.Translate(randomDirection * stats.Speed * Time.deltaTime);

        // Rotate towards the next direction
        Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            GetRandomDirection();
        }
    }

    void GetRandomDirection()
    {
        // Generate a random angle within the desired arc (180 degrees)
        float angle = Random.Range(-90f, 90f) * Mathf.Deg2Rad;

        // Calculate the random direction within the specified arc
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);

        // Set the random direction
        randomDirection = new Vector3(x, 0f, z).normalized;

        timer = changeInterval;
    }

    void FollowLeader()
    {
        if (leader != null)
        {
            Vector3 directionToLeader = (leader.position - transform.position).normalized;

            // Calculate the distance to the leader
            float distanceToLeader = Vector3.Distance(transform.position, leader.position);

            // Calculate the target position based on the leader's position and the offset
            Vector3 targetPosition = leader.position - directionToLeader * followOffSet;

            // Apply additional offset from behind the leader
            targetPosition -= directionToLeader * followOffSet;

            // Move towards the target position with adjusted speed based on distance to leader
            float effectiveSpeed = stats.Speed;
            if (distanceToLeader > followOffSet * 2)
            {
                // Increase speed until the distance is twice the offset
                effectiveSpeed *= 2f;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, effectiveSpeed * Time.deltaTime);

            // Rotate towards the leader's direction
            Quaternion targetRotation = Quaternion.LookRotation(directionToLeader);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
        }
    }

    void CheckForLeader()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, followRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") && collider.transform != transform)
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
            Debug.Log(gameObject.name + " is following " + leader.name);
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
