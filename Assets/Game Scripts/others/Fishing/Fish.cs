using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 40f;
    [SerializeField] private float fishAlertRadius = 40f;
    private BoxCollider fishTankCollider;
    private Rigidbody rb;
    private List<Vector3> patrolPoints = new List<Vector3>();
    private int currentPatrolIndex = 0;
    private float speed;
    private Animator animator;

    private float panicTimer = 0;
    private float panictimerMax = 10;

    [SerializeField] private HoldableObjectSO spearSO;
    [SerializeField] private GatherableSO fishSO;
    [SerializeField] private Transform toFollowTransform = null;

    public enum FishState
    {
        Partrol,Captured,Panic
    }
    private FishState state;

    private void OnEnable()
    {
        EventManager.Instance.OnPlayerThrowedSpear += EventManager_Instance_OnPlayerThrowedSpear;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnPlayerThrowedSpear -= EventManager_Instance_OnPlayerThrowedSpear;
    }

    private void EventManager_Instance_OnPlayerThrowedSpear(object sender, EventManager.OnPlayerThrowedSpearArgs e)
    {
        Alert(e.throwedPosition, e.impactRadius);     
    }

    private void Alert(Vector3 throwedPosition, float impactRadius)
    {
        float fishToPlayerDistance = Vector3.Distance(transform.position, throwedPosition);

        if (fishToPlayerDistance < fishAlertRadius) // Spear throwed Near This Fish
        {
            Debug.Log("Im Alert");
            if (state == FishState.Captured) return;

            state = FishState.Panic;
        }
    }

    void Start()
    {
        state = FishState.Partrol;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        speed = Random.Range(minSpeed, maxSpeed);
        panicTimer = panictimerMax;
        GeneratePatrolPoints();
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (patrolPoints.Count == 0) return;

        if(state == FishState.Partrol)
        {
            Vector3 direction = patrolPoints[currentPatrolIndex] - transform.position;
            rb.velocity = direction.normalized * speed * Time.deltaTime;

            // Rotate towards the next patrol point
            RotateTowardsPoint(patrolPoints[currentPatrolIndex],10f);

            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex]) < 0.1f)
            {
                MoveToNextPatrolPoint();
            }
        }
        else if(state == FishState.Captured) 
        { 
            if(toFollowTransform != null)
            {
                transform.SetPositionAndRotation(toFollowTransform.position, toFollowTransform.rotation);
            }
        }
        else if (state == FishState.Panic)
        {
            panicTimer -= Time.deltaTime;

            if (panicTimer < 0)
            {
                panicTimer = panictimerMax;
                speed = Mathf.Lerp(speed, 10f, 5f); // Adjust the speed gradually using Mathf.Lerp
                state = FishState.Partrol;
            }
            else
            {
                Vector3 direction = patrolPoints[currentPatrolIndex] - transform.position;
                rb.velocity = direction.normalized * 300f * Time.deltaTime;

                // Rotate towards the next patrol point
                RotateTowardsPoint(patrolPoints[currentPatrolIndex], 30f);

                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex]) < 0.1f)
                {
                    MoveToNextPatrolPoint();
                }
            }
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

    private void RotateTowardsPoint(Vector3 targetPosition,float rotSpeed)
    {
        Vector3 direction = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null && state == FishState.Partrol)
        {
            if(other.gameObject.TryGetComponent(out HoldableObject holdableObject))
            {
                HoldableObjectSO spearSO = holdableObject.GetHoldableObjectSO();

                if(spearSO == this.spearSO && holdableObject.IsThrowedByPlayer())
                {
                    Debug.Log("Spear Hitted");
                    toFollowTransform = holdableObject.GetObjectToFollowTransform();
                    toFollowTransform.rotation = transform.rotation;
                    animator.SetBool("IsFastSwim", true);

                    gameObject.AddComponent<GatherableObject>().SetGatherableObjectSO(fishSO);
                    gameObject.layer = LayerMask.NameToLayer("Interactable");

                    state = FishState.Captured;
                }
               
            }
        }
    }
}
