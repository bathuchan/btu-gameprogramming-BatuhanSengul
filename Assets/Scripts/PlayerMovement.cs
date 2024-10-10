using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed=3f;
    [SerializeField]
    private float xBorderValue,yBorderValue;

    [SerializeField]
    private GameObject laserPrefab;
    private float horizontalVal, verticalVal;

    [SerializeField]
    private float projSpeed=10f, projLifespan=1f;
    void Start()
    {
        Debug.Log("Game Started");
        timer= fireCooldown;
    }
    [SerializeField]
    float fireCooldown=0.5f,timer = 0.5f;
    void Update()
    {
        if (fireCooldown >= 0) 
        {
            fireCooldown -= Time.deltaTime;
        }
        
        HandleMovement();
        HandleShoot();
    }
    
    private void HandleMovement() 
    {

        horizontalVal = Input.GetAxis("Horizontal");
        verticalVal = Input.GetAxis("Vertical");



        //Oyun nesnesinin Sag veya Sola haraketi
        transform.Translate(new Vector3(horizontalVal * speed * Time.deltaTime, verticalVal * speed * Time.deltaTime, transform.position.z));

        transform.position = new Vector3(Math.Clamp(transform.position.x, -xBorderValue, xBorderValue), Math.Clamp(transform.position.y, -yBorderValue, yBorderValue), transform.position.z);
        //Oyun nesnesinin Yukari veya Asagi haraketi  
        //transform.Translate(verticalVal * Vector3.up * speed * Time.deltaTime);

        //Ekran ciktilari
        //if (horizontalVal != 0) Debug.Log("Horizontal value: " + horizontalVal);

        //if (verticalVal != 0) Debug.Log("Vertical value: " + verticalVal);
    }

    private void HandleShoot() 
    {
        
        if(Input.GetKey(KeyCode.Space) && fireCooldown <= 0) 
        {
            fireCooldown = timer;
            GameObject go= Instantiate(laserPrefab, transform.position+Vector3.up, Quaternion.identity);
            
            if(go.TryGetComponent<Rigidbody>(out Rigidbody rb)) 
            {
                rb.AddForce(Vector3.up * projSpeed,ForceMode.Impulse);
                Destroy(go,projLifespan);
            }
        }

    }
}



