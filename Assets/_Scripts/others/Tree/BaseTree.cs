using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTree : MonoBehaviour
{
    [Header("Fruit Spawn")]
    [SerializeField] protected FruitType fruitType;
    [SerializeField] protected Transform fruitSpawnPosition;
    [SerializeField][Range(1, 10)] protected int fruitAmountToSpawn = 3;
    [SerializeField] protected float fruitSpawnRadius = 1.0f;

    [Header("Stone Spawn")]
    [SerializeField] protected float stoneSpawnRadius = 10f;
    [SerializeField] protected int stoneAmountToSpawn = 2;

    private void Start()
    {
        SpawnFruit();

        SpawnStone();
    }
    protected virtual void SpawnFruit()
    {
        List<Vector3> fruitPositions = new();

        for (int i = 0; i < fruitAmountToSpawn; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * fruitSpawnRadius;
            Vector3 newFruitPosition = fruitSpawnPosition.position + randomOffset;

            if (!IsOverlapping(newFruitPosition, fruitPositions, 0.5f))
            {
                var fruitPrefab = Instantiate(fruitType == FruitType.Coconut ? Prefabs.Instance.GetCoconutPrefab() :
                                          Prefabs.Instance.GetApplePrefab(), fruitSpawnPosition);
                fruitPrefab.transform.position = newFruitPosition;
                fruitPositions.Add(newFruitPosition);

                //fruitPrefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
            else
            {
                i--; // Retry to spawn a fruit at a different position
            }
        }
    }

    private bool IsOverlapping(Vector3 newPosition, List<Vector3> existingPositions, float minDistanceOffset)
    {
        foreach (Vector3 existingPos in existingPositions)
        {
            float distance = Vector3.Distance(newPosition, existingPos);
            if (distance < minDistanceOffset) // Change 1.0f to your desired minimum distance between fruits
            {
                return true; // Overlapping
            }
        }
        return false; // Not overlapping
    }

    protected virtual void SpawnStone()
    {
        for(int i = 0; i < stoneAmountToSpawn;  i++)
        {
            var stonePrefab = Instantiate(Prefabs.Instance.GetStonePrefab(), transform);
            Vector3 randPositon = Random.insideUnitSphere * stoneSpawnRadius + fruitSpawnPosition.position;
            stonePrefab.transform.position = randPositon;
        }
    }

    private void OnDrawGizmos()
    {
        if(fruitSpawnPosition != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(fruitSpawnPosition.position, 0.2f);

            Gizmos.DrawWireSphere(fruitSpawnPosition.position, fruitSpawnRadius);
        }
       
    }

}
[System.Serializable]
public enum FruitType
{
    Coconut,
    Apple
}
