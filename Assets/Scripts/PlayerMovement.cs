using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;


    public float xBorderValue, yBorderValue;

    private float horizontalVal, verticalVal;

    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private float projSpeed = 10f, projLifespan = 1f;
    [SerializeField]
    float fireCooldown = 0.5f, timer = 0.5f;
    [SerializeField]
    public int lives = 3;


    [SerializeField] private GameObject playerSprite;
    public float tiltAmount = 5f;    // Max tilt angle in degrees
    public float tiltSpeed = 5f;     // Speed of tilting
    public float maxTiltAngle = 5f;  // Maximum tilt angle for clamping

    private float targetZRotation;
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


        // Calculate target rotation based on input
        targetZRotation = -horizontalVal * tiltAmount;

        // Smoothly interpolate the current z-rotation towards the target angle
        float currentZRotation = playerSprite.transform.localEulerAngles.z;
        // Adjust for negative angles by converting ranges from 0-360 to -180 to 180
        if (currentZRotation > 180) currentZRotation -= 360;

        float zRotation = Mathf.Lerp(currentZRotation, targetZRotation, Time.deltaTime * tiltSpeed);

        // Clamp the rotation to the specified range
        zRotation = Mathf.Clamp(zRotation, -maxTiltAngle, maxTiltAngle);

        // Apply the clamped rotation
        playerSprite.transform.localEulerAngles = new Vector3(
            playerSprite.transform.localEulerAngles.x,
            playerSprite.transform.localEulerAngles.y,
            zRotation
        );
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

    public void LowerLives()
    {
        lives--;
        if (lives <= 0) 
        {
            Destroy(gameObject);
            Debug.Log("No Lives Left GAME OVER!!");
        }

    }
}



