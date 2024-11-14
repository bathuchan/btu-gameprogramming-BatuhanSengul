using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField]private float enemySpawnTime=5f;
    private PlayerMovement pm;
    GameObject lastSpawnedEnemy;

    [SerializeField] private GameObject powerUpContainer;
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
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        StartCoroutine(EnemySpawner());
        StartCoroutine(TriplePowerUpSpawner());
        StartCoroutine (SpeedPowerUpSpawner());
    }

    
    IEnumerator EnemySpawner() 
    {
        while (pm.lives > 0) 
        {    
            lastSpawnedEnemy=Instantiate(enemyPrefab,
                (new Vector3(Random.Range(-pm.xBorderValue, pm.xBorderValue),pm.yBorderValue+1.2f,0)),
                Quaternion.identity);
            lastSpawnedEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(enemySpawnTime);
        }
    }
    IEnumerator TriplePowerUpSpawner()
    {
        while (pm.lives > 0)
        {
            float spawnChanceLocal = Random.Range(0f, 1f);
            if(spawnChanceLocal < tripSpawnChance) 
            {
                lastSpawnedTriplePowerUp = Instantiate(triplePowerUpPrefab,
                (new Vector3(Random.Range(-pm.xBorderValue, pm.xBorderValue), pm.yBorderValue + 1.2f, 0)),
                Quaternion.identity);
                lastSpawnedTriplePowerUp.transform.parent = powerUpContainer.transform;
                yield return new WaitForSeconds(5f);
            }
            
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SpeedPowerUpSpawner()
    {
        while (pm.lives > 0)
        {
            float spawnChanceLocal = Random.Range(0f, 1f);
            if (spawnChanceLocal < tripSpawnChance)
            {
                lastSpawnedSpeedPowerUp = Instantiate(speedPowerUpPrefab,
                (new Vector3(Random.Range(-pm.xBorderValue, pm.xBorderValue), pm.yBorderValue + 1.2f, 0)),
                Quaternion.identity);
                lastSpawnedSpeedPowerUp.transform.parent = powerUpContainer.transform;
                yield return new WaitForSeconds(5f);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
