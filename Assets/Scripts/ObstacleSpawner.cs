using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;   
    public Transform playerTransform;       
    public float spawnInterval = 2.0f;     
    public float spawnDistanceAhead = 20f; 
    public float laneWidth = 2.5f;          
    public float obstacleLifetime = 10f;    
    private float[] laneXPositions;

    void Start()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogError("Obstacle Prefabs array not assigned or empty in ObstacleSpawner!");
            enabled = false; 
            return;
        }
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform not assigned in ObstacleSpawner!");
            enabled = false;
            return;
        }

        laneXPositions = new float[] { -laneWidth, 0f, laneWidth };
        if (laneXPositions.Length != 3)
        {
            Debug.LogError("laneXPositions should represent 3 lanes for the current logic!");
            enabled = false;
            return;
        }

        StartCoroutine(SpawnObstaclesRoutine());
    }

    IEnumerator SpawnObstaclesRoutine()
    {
        while (true) 
        {
            yield return new WaitForSeconds(spawnInterval); 
            SpawnObstaclePair();
        }
    }

    void SpawnObstaclePair() 
    {
        if (obstaclePrefabs.Length == 0) return;

        List<int> allLaneIndices = new List<int> { 0, 1, 2 }; 

        int firstChosenListIndex = Random.Range(0, allLaneIndices.Count);
        int laneArrayIndexToSpawnIn1 = allLaneIndices[firstChosenListIndex];
        allLaneIndices.RemoveAt(firstChosenListIndex); 
        int secondChosenListIndex = Random.Range(0, allLaneIndices.Count);
        int laneArrayIndexToSpawnIn2 = allLaneIndices[secondChosenListIndex];
        GameObject prefabType1 = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        float spawnX1 = laneXPositions[laneArrayIndexToSpawnIn1];
        Vector3 spawnPos1 = new Vector3(
            spawnX1,
            prefabType1.transform.position.y, 
            playerTransform.position.z + spawnDistanceAhead
        );
        GameObject newObstacle1 = Instantiate(prefabType1, spawnPos1, Quaternion.identity);
        Destroy(newObstacle1, obstacleLifetime);

        GameObject prefabType2 = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]; 
        float spawnX2 = laneXPositions[laneArrayIndexToSpawnIn2];
        Vector3 spawnPos2 = new Vector3(
            spawnX2,
            prefabType2.transform.position.y, 
            playerTransform.position.z + spawnDistanceAhead
        );
        GameObject newObstacle2 = Instantiate(prefabType2, spawnPos2, Quaternion.identity);
        Destroy(newObstacle2, obstacleLifetime);

    }
}