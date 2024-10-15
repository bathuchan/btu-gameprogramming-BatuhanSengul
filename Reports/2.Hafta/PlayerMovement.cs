using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed=3f;

    [SerializeField]
    private float xBorderValue,yBorderValue;

    private float horizontalVal, verticalVal;

    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private float projSpeed=10f, projLifespan=1f;
    [SerializeField]
    float fireCooldown = 0.5f, timer = 0.5f;
    void Start()
    {
        Debug.Log("Game Started");
        timer= fireCooldown;
    }
    
    void Update()// fpse bagli calisir
    {
        if (fireCooldown >= 0) 
        {
            fireCooldown -= Time.deltaTime;
        }
        
        HandleMovement();//Hareket fonksiyonu
        HandleShoot();//ates etme fonksiyonu
    }
    
    public void HandleMovement() 
    {

        horizontalVal = Input.GetAxis("Horizontal");
        verticalVal = Input.GetAxis("Vertical");

      
        //Oyun nesnesinin Sag veya Sola haraketi
        
        transform.Translate(new Vector3(horizontalVal * speed * Time.deltaTime, verticalVal * speed * Time.deltaTime, transform.position.z));

        //Ekran keanar sinirlari
        transform.position = new Vector3(Math.Clamp(transform.position.x, -xBorderValue, xBorderValue),
            Math.Clamp(transform.position.y, -yBorderValue, yBorderValue), 
            transform.position.z);
        
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



