using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f,speedRandomMultiplier=1f;
    private float xPos;
    private PlayerMovement pm;
    

    void Start() 
    {
        pm = FindObjectOfType<PlayerMovement>();
        xPos = Random.Range(-pm.xBorderValue, pm.xBorderValue);
        transform.position = new Vector3(xPos, 6, 0);
        speed = speed+ Random.Range(0, speedRandomMultiplier);

    }
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, -speed * Time.deltaTime, 0);
        ResetPosition();
    }

    private void ResetPosition() 
    {
        if (transform.position.y < -(pm.yBorderValue+1.20f)) 
        {
            xPos = Random.Range(-pm.xBorderValue, pm.xBorderValue);
            transform.position = new Vector3(xPos, 6, 0);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            pm.SpawnExp(transform.position);
            pm.LowerLives();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Laser")) 
        {
            pm.SpawnExp(transform.position);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    
}
