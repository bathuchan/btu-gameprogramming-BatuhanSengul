using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float speed = 1f, speedRandomMultiplier = 1f;
    private float xPos;
    
    
    private Animator animator;
    private Collider col;

    [Header("Asteroid Settings")]
    public bool isAsteroid = false;  // Is this enemy an asteroid?
    public GameObject asteroidPrefab; // Prefab for smaller duplicates
    private Vector3 moveDirection;   // Direction of movement
    private float spinSpeed;         // Speed of spinning animation

    private GameObject spriteGO;

    void Start()
    {
        
        
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        StartCoroutine(SpawnProtect(0.1f));
        
        spriteGO= GetComponentInChildren<SpriteRenderer>().GameObject();


        if (isAsteroid && moveDirection == Vector3.zero)
        {
            moveDirection = new Vector3(Random.Range(-3f, 3f), Random.Range(-1f, -1.5f), 0f).normalized;
        }
        // Set random x-position and speed
        xPos = Random.Range(-PlayerMovement.Instance.xBorderValue, PlayerMovement.Instance.xBorderValue);
        if (!isAsteroid)
        {
            transform.position = new Vector3(xPos, 6, 0);
            speed += Random.Range(0, speedRandomMultiplier);
        }
        else 
        {
            RotateAsteroid();
        }

        // If this is an asteroid, initialize its diagonal movement
        

        // Initialize spin speed for asteroids
        if (isAsteroid && spinSpeed == 0)
        {
            spinSpeed = Random.Range(50f, 100f); // Randomize spin speed for variety
        }
        else 
        {
            spinSpeed *= 1.5f;
        }
    }

    void Update()
    {
        // Move the enemy or asteroid
        if (isAsteroid)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
            SpinAsteroid();
            
        }
        else
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        // Check for screen wrapping for asteroids
        if (isAsteroid)
        {
            WrapAsteroidPosition();
        }

        ResetPosition();
    }
    private void RotateAsteroid()
    {
        // Calculate rotation angle based on movement direction
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Add rotation speed based on movement speed
        float rotationSpeed = speed * 50f; // Adjust multiplier for desired effect
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    private void SpinAsteroid()
    {
        // Continuously rotate the asteroid on its Z-axis
        if (moveDirection.x > 0) 
        {
            spriteGO.transform.Rotate(0, 0, -spinSpeed * Time.deltaTime);
        }
        else 
        {
            spriteGO.transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }
        
    }

    private void WrapAsteroidPosition()
    {
        float xBorder = PlayerMovement.Instance.xBorderValue;
        float yBorder = PlayerMovement.Instance.yBorderValue;

        // Wrap horizontally
        if (transform.position.x > xBorder + 2f)
        {
            //GetComponentInChildren<TrailRenderer>().Clear();
            transform.position = new Vector3(-xBorder, transform.position.y, transform.position.z);
            
        }
        else if (transform.position.x < -xBorder - 2f)
        {
            //GetComponentInChildren<TrailRenderer>().Clear();
            transform.position = new Vector3(xBorder, transform.position.y, transform.position.z);
            
        }

    }

    private void ResetPosition()
    {
        if (transform.position.y < -(PlayerMovement.Instance.yBorderValue + 2f))
        {
            xPos = Random.Range(-PlayerMovement.Instance.xBorderValue, PlayerMovement.Instance.xBorderValue);
            transform.position = new Vector3(xPos, 6, 0);
            

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& PlayerMovement.Instance.canTakeDamage)
        {
            //Debug.Log("Oyuncu temasi");
            PlayerMovement.Instance. SpawnExp(transform.position + new Vector3(0, 0, 0.05f));
            PlayerMovement.Instance.LowerLives();

            PlayerMovement.Instance.StartImmunity(2f);
            if (isAsteroid)
            {
                DuplicateAsteroid();
            }
            HandleDestruction();

        }
        else if (other.CompareTag("Laser"))
        {
            //Debug.Log("Lazer temasi");
            
            
            Destroy(other.gameObject);

            if (isAsteroid)
            {

                TextManager.Instance.AddScore(100 * Mathf.CeilToInt(1f/transform.localScale.x));
                DuplicateAsteroid();
            }
            else 
            {
                TextManager.Instance.AddScore(100);
            }
            PlayerMovement.Instance.SpawnExp(transform.position + new Vector3(0, 0, 0.05f));
            HandleDestruction();
        }
    }

    private void HandleDestruction()
    {
        
        if (!isAsteroid)
        {
            animator.SetTrigger("PlayDestroy");
            speed = speed / 2f;
            col.enabled = false;
            Destroy(gameObject, 1f);
        }
        else
        {
            speed = speed / 2f;
            
            Destroy(gameObject);
        }
    }

    private void DuplicateAsteroid()
    {
        // Only duplicate if asteroid scale is above a threshold
        if (transform.localScale.x > 0.4f)
        {
            Vector3 currentScale = transform.localScale;

            // Create the first duplicate (same direction)
            GameObject duplicate1 = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            duplicate1.transform.localScale = currentScale * 0.70f;
            Enemy asteroid1 = duplicate1.GetComponent<Enemy>();
            asteroid1.isAsteroid = true;
            asteroid1.moveDirection = moveDirection + new Vector3(0, -duplicate1.transform.localScale.y, 0);
            



            // Create the second duplicate (mirrored direction)
            GameObject duplicate2 = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            duplicate2.transform.localScale = currentScale * 0.70f;
            Enemy asteroid2 = duplicate2.GetComponent<Enemy>();
            asteroid2.isAsteroid = true;
            asteroid2.moveDirection = new Vector3(-moveDirection.x, moveDirection.y, moveDirection.z) + new Vector3(0, -duplicate2.transform.localScale.y, 0);
            

            asteroid1.speed += duplicate1.transform.localScale.y;
            asteroid2.speed = asteroid1.speed;


        }
        
            
        
    }

    IEnumerator SpawnProtect(float duration) 
    {
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        col.enabled = true;
    }
}
