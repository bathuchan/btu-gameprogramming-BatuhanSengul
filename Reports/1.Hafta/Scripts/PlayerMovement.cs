using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed=3f;

    private float horizontalVal, verticalVal;
    void Start()
    {
        Debug.Log("Game Started");
    }

    
    void Update()
    {
        horizontalVal = Input.GetAxis("Horizontal");
        verticalVal = Input.GetAxis("Vertical");

        //Oyun nesnesinin Sag veya Sola haraketi
        transform.Translate(horizontalVal* Vector3.right * speed * Time.deltaTime);

        //Oyun nesnesinin Yukari veya Asagi haraketi  
        transform.Translate(verticalVal * Vector3.up * speed * Time.deltaTime);

        //Ekran ciktilari
        if (horizontalVal != 0) Debug.Log("Horizontal value: " +horizontalVal);

        if (verticalVal != 0) Debug.Log("Vertical value: " + verticalVal);

    }
}



