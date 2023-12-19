using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 40f;
    private BoxCollider fishTankCollider;
    private Rigidbody rb;
    private List<Vector3> patrolPoints = new List<Vector3>();
    private int currentPatrolIndex = 0;
    private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = Random.Range(minSpeed, maxSpeed);
        GeneratePatrolPoints();
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (patrolPoints.Count == 0) return;

        Vector3 direction = patrolPoints[currentPatrolIndex] - transform.position;
        rb.velocity = direction.normalized * speed * Time.deltaTime;

        // Rotate towards the next patrol point
        RotateTowardsPoint(patrolPoints[currentPatrolIndex]);

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex]) < 0.1f)
        {
            MoveToNextPatrolPoint();
        }
    }

    private void GeneratePatrolPoints()
    {
        if (fishTankCollider == null)
        {
            Debug.LogError("Fish tank collider reference missing.");
            return;
        }

        for (int i = 0; i < 5; i++) // Change 5 to the number of patrol points you want
        {
            Vector3 randomPoint = GetRandomPositionWithinBounds();
            patrolPoints.Add(randomPoint);
        }
    }

    private void MoveToNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }

    private Vector3 GetRandomPositionWithinBounds()
    {
        Vector3 bounds = fishTankCollider.size * 0.5f;
        Vector3 randomPosition = new Vector3(
            Random.Range(-bounds.x, bounds.x),
            Random.Range(-bounds.y, bounds.y),
            Random.Range(-bounds.z, bounds.z)
        );

        randomPosition += fishTankCollider.transform.position;

        return randomPosition;
    }

    public void SetPatrolCollider(BoxCollider boxCollider)
    {
        this.fishTankCollider = boxCollider;
    }

    private void RotateTowardsPoint(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5f);
    }

    private void OnDrawGizmos()
    {
        // ... (same as your previous OnDrawGizmos method)
    }
}
