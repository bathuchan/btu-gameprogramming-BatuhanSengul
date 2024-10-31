using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundEffects : MonoBehaviour
{
    public float horizontalScrollSpeed = -0.1f; // Speed of the scroll
    public float verticalScrollSpeed = 0.1f;
    float horValue;
    [SerializeField]private Renderer rend;
    private PlayerMovement pm;
    float offsetX, offsetY;
    void Start()
    {
        // Get the Renderer component from the object
        rend = GetComponent<Renderer>();
        pm = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (pm.lives>0) 
        {
            horValue = Input.GetAxis("Horizontal");
            offsetY= Time.time * horizontalScrollSpeed;
            if (horValue > 0)
            {
                offsetX = Time.deltaTime * verticalScrollSpeed;
            }
            else if (horValue < 0)
            {
                offsetX = -Time.deltaTime * verticalScrollSpeed;
            }
            else
            {
                offsetX = 0;
            }
        }
        else 
        {
            offsetX = 0;
        }
        
        
        rend.material.mainTextureOffset = new Vector2(rend.material.mainTextureOffset.x+offsetX, offsetY); 
    }
}
