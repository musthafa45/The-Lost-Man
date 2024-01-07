using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private int numberOfFishToSpawn = 10;
    [SerializeField] private BoxCollider fishTankCollider;

    private void Start()
    {
        SpawnFish();
    }

    private void SpawnFish()
    {
        if (fishPrefab == null || fishTankCollider == null)
        {
            Debug.LogError("Fish prefab or collider reference is missing.");
            return;
        }

        for (int i = 0; i < numberOfFishToSpawn; i++)
        {
            Vector3 randomPosition = GetRandomPositionWithinBounds();
            var fish = Instantiate(fishPrefab, randomPosition, Quaternion.identity);
            Fish fishScript = fish.GetComponent<Fish>();
            fishScript.SetPatrolCollider(fishTankCollider);
        }
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

    //private void OnDrawGizmos()
    //{
    //    if (fishTankCollider != null)
    //    {
    //        Gizmos.color = Color.blue;
    //        //Gizmos.DrawWireCube(transform.position, fishTankCollider.size);
    //    }
    //}
}
