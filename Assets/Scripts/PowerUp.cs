using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType 
    {
        TripleShotPowerup,
        Option2,
        Option3,
    }
    public PowerUpType type;
    private PlayerMovement pm;

    [Header("Downward Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedRandomMultiplier = 1f;



    private void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        speed = speed + Random.Range(0, speedRandomMultiplier);
    }
    private void Update()
    {
        transform.position = transform.position + new Vector3(0, -speed * Time.deltaTime, 0);
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (transform.position.y < -(pm.yBorderValue + 1.20f))
        {

            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            switch (type) 
            {
                case PowerUpType.TripleShotPowerup:
                    pm.StartTriplePowerup();
                    break;
                default:
                    Debug.LogWarning("PowerUp type doesnt declared!");
                    break;
            }
            Destroy(gameObject);
        }
    }
}
