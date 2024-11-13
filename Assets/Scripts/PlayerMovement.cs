using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;


    public float xBorderValue, yBorderValue;

    private float horizontalVal, verticalVal;

    [SerializeField]
    private GameObject laserPrefab, laserContainer;

    [SerializeField]
    private float projSpeed = 10f, projLifespan = 1f;
    [SerializeField]
    float fireCooldown = 0.5f, timer = 0.5f;
    [SerializeField]
    public int lives = 3;

    [Header("Sprite Tilt Settings")]
    [SerializeField] private GameObject playerSprite;
    public float tiltAmount = 5f;    
    public float tiltSpeed = 5f;     
    public float maxTiltAngle = 5f;  

    private float targetZRotation;

    [Header("Power-Up Settings")]
    [SerializeField] private bool tripleShotIsActive=false;
    [SerializeField] private GameObject tripleLaserPrefab;
    [SerializeField] private float tripleShotDuration = 5f;

    [Header("VFX Settings")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform vfxContainer;
    CameraShake cameraShake;
    void Start()
    {
        Debug.Log("Game Started");
        timer= fireCooldown;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        
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


        
        targetZRotation = -horizontalVal * tiltAmount;

        
        float currentZRotation = playerSprite.transform.localEulerAngles.z;
        
        if (currentZRotation > 180) currentZRotation -= 360;

        float zRotation = Mathf.Lerp(currentZRotation, targetZRotation, Time.deltaTime * tiltSpeed);

        
        zRotation = Mathf.Clamp(zRotation, -maxTiltAngle, maxTiltAngle);

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
            GameObject laser;
            if (tripleShotIsActive) 
            {
                laser = Instantiate(tripleLaserPrefab, transform.position + Vector3.up, Quaternion.identity);
            }
            else 
            {
                laser = Instantiate(laserPrefab, transform.position + Vector3.up, Quaternion.identity);
            }
            
            laser.transform.parent=laserContainer.transform;
            
            if(laser.TryGetComponent<Rigidbody>(out Rigidbody rb)) 
            {
                rb.AddForce(Vector3.up * projSpeed,ForceMode.Impulse);
                Destroy(laser, projLifespan);
                
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
    public void SpawnExp(Vector3 expPos) 
    {
        GameObject exp = Instantiate(explosionPrefab, expPos, Quaternion.identity);
        exp.transform.parent = vfxContainer;
        Destroy(exp, 4f);
        cameraShake.StartShake(0.5f, 0.3f);
    }

    Coroutine tripCoroutine;

    public void StartTriplePowerup() 
    {
        
        if (tripCoroutine == null)
        {
            tripleShotIsActive = true;
            tripCoroutine = StartCoroutine(TripleEffect());
        }
        else
        {
            StopCoroutine(tripCoroutine);
            tripleShotIsActive = true;
            tripCoroutine = StartCoroutine(TripleEffect());
        }

    }

    public IEnumerator TripleEffect()
    {

        yield return new WaitForSeconds(tripleShotDuration);
        tripleShotIsActive = false;
    }

   

    
}



