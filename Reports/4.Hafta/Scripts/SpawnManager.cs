using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab,enemyContainer;
    private PlayerMovement pm;
    [SerializeField]
    private float spawnTime=5f;
    GameObject lastSpawnedEnemy;

    void Start()
    {
        pm= FindObjectOfType<PlayerMovement>();
        StartCoroutine(EnemySpawner());
    }

    
    IEnumerator EnemySpawner() 
    {
        while (pm.lives > 0) 
        {    
            lastSpawnedEnemy=Instantiate(enemyPrefab,
                (new Vector3(Random.Range(-pm.xBorderValue, pm.xBorderValue),pm.yBorderValue+1.2f,0)),
                Quaternion.identity);
            lastSpawnedEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
