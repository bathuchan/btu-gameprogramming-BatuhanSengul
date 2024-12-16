using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spaceship Enemy Spawn Settings")]
    [SerializeField] private GameObject spaceshipEnemyPrefab;
    [SerializeField]private float spaceshipEnemySpawnTime = 2f;

    [Header("Astroid Enemy Spawn Settings")]
    [SerializeField] private GameObject astroidEnemyPrefab;    
    [SerializeField] private float astroidEnemySpawnTime = 8f;

    

    private PlayerMovement pm;
    GameObject lastSpawnedEnemy;

    

    [Header("TriplePowerup Spawn Settings")]
    [SerializeField] private GameObject triplePowerUpPrefab;
    [Range(0f, 1f)][SerializeField] private float tripSpawnChance = .5f;
    GameObject lastSpawnedTriplePowerUp;

    [Header("SpeedPowerup Spawn Settings")]
    [SerializeField]private GameObject speedPowerUpPrefab;
    [Range(0f, 1f)][SerializeField] private float speedSpawnChance = .5f;
    GameObject lastSpawnedSpeedPowerUp;

    
    void Start()
    {
        StartCoroutine(EnemySpawner(spaceshipEnemyPrefab, spaceshipEnemySpawnTime));
        StartCoroutine(EnemySpawner(astroidEnemyPrefab, astroidEnemySpawnTime));
        StartCoroutine(PowerUpSpawner(triplePowerUpPrefab, tripSpawnChance));
        StartCoroutine(PowerUpSpawner(speedPowerUpPrefab, speedSpawnChance));
    }

    
    IEnumerator EnemySpawner(GameObject enemy,float spawnTime) 
    {
        while (PlayerMovement.Instance.lives > 0) 
        {
            yield return new WaitForSeconds(spawnTime);
            lastSpawnedEnemy =Instantiate(enemy,
                (new Vector3(Random.Range(-PlayerMovement.Instance.xBorderValue, PlayerMovement.Instance.xBorderValue),PlayerMovement.Instance.yBorderValue+1.2f,0)),
                Quaternion.identity);
            lastSpawnedEnemy.transform.parent = Referances.Instance.enemyContainer.transform;
            
        }
    }
   
    
    IEnumerator PowerUpSpawner(GameObject prefab,float spawnProb)
    {
        while (PlayerMovement.Instance.lives > 0)
        {
            float spawnChanceLocal = Random.Range(0f, 1f);
            if (spawnChanceLocal < spawnProb)
            {
                lastSpawnedTriplePowerUp = Instantiate(prefab,
                (new Vector3(Random.Range(-PlayerMovement.Instance.xBorderValue, PlayerMovement.Instance.xBorderValue), PlayerMovement.Instance.yBorderValue + 1.2f, 0)),
                Quaternion.identity);
                lastSpawnedTriplePowerUp.transform.parent = Referances.Instance.powerUpContainer.transform;
                yield return new WaitForSeconds(5f);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    
}
