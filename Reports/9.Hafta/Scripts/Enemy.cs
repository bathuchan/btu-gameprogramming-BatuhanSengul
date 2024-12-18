using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float speed = 1f, speedRandomMultiplier = 1f;
    private float xPos;
    
    
    
    private Collider col;

    [Header("Asteroid Settings")]
    public bool isAsteroid = false;  
    public GameObject asteroidPrefab; 
    private Vector3 moveDirection;   
    private float spinSpeed;        

    private GameObject spriteGO;
    int[] possibleXValues= {-2,-1,1,2 };

    float xBorder, yBorder;
    private Animator animator;
    void Start()
    {

        
        animator = GetComponent<Animator>();

        col = GetComponent<Collider>();
        StartCoroutine(SpawnProtect(0.1f));
        
        spriteGO= GetComponentInChildren<SpriteRenderer>().GameObject();

        xBorder = PlayerMovement.Instance.xBorderValue;
        yBorder = PlayerMovement.Instance.yBorderValue;

        if (isAsteroid && moveDirection == Vector3.zero)
        {
            moveDirection = new Vector3(possibleXValues[Random.Range(0, possibleXValues.Length)], Random.Range(-speed, -1.5f), 0f).normalized;
        }
 
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

        
        if (isAsteroid && spinSpeed == 0)
        {
            spinSpeed = Random.Range(50f, 100f); 
        }
        else 
        {
            spinSpeed *= 1.1f;
        }
    }

    void Update()
    {
        if (isAsteroid)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
            SpinAsteroid();
            
        }
        else
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        if (isAsteroid)
        {
            WrapAsteroidPosition();
        }

        ResetPosition();
    }
    private void RotateAsteroid()
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float rotationSpeed = speed * 50f; 
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    private void SpinAsteroid()
    {
        
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
        
        if (transform.position.x > xBorder + 2f)
        {
            
            transform.position = new Vector3(-xBorder-1.2f, transform.position.y, transform.position.z);
            
        }
        else if (transform.position.x < -xBorder - 2f)
        {
            
            transform.position = new Vector3(xBorder + 1.2f, transform.position.y, transform.position.z);
            
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
            speed = 0;
            col.enabled = false;
            Destroy(gameObject, 1f);
        }
        else
        {
            speed = speed / 2f;
            
            Destroy(gameObject,0.01f);
        }
    }

    private void DuplicateAsteroid()
    {
       
        if (transform.localScale.x > 0.4f)
        {
            Vector3 currentScale = transform.localScale;

            GameObject duplicate1 = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            duplicate1.transform.localScale = currentScale * 0.70f;
            Enemy asteroid1 = duplicate1.GetComponent<Enemy>();
            asteroid1.isAsteroid = true;
            asteroid1.moveDirection = moveDirection.normalized;
            asteroid1.transform.parent=Referances.Instance.transform;


            GameObject duplicate2 = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            duplicate2.transform.localScale = currentScale * 0.70f;
            Enemy asteroid2 = duplicate2.GetComponent<Enemy>();
            asteroid2.isAsteroid = true;
            asteroid2.moveDirection = new Vector3(-moveDirection.x, moveDirection.y, moveDirection.z).normalized;
            asteroid2.transform.parent = Referances.Instance.transform;

            asteroid1.speed += duplicate1.transform.localScale.y*1.5f;
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
