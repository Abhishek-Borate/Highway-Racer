using UnityEngine;
using System.Collections;

public class TrafficSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] vehiclePrefabs; // Array of traffic vehicle prefabs
    [SerializeField] private Transform[] spawnPoints; // Array of spawn positions
    [SerializeField] private float spawnInterval = 1.0f; // Time between spawns

    void Start()
    {
        StartCoroutine(SpawnTraffic());
    }

    IEnumerator SpawnTraffic()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            SpawnVehicle();
        }
    }

    void SpawnVehicle()
    {
        if (vehiclePrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No vehicle prefabs or spawn points assigned!");
            return;
        }

        // Select a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Select a random vehicle prefab
        GameObject vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];

        // Spawn the vehicle at the spawn point
        Instantiate(vehiclePrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
