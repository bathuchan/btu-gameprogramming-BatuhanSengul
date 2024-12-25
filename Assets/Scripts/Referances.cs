using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Referances : MonoBehaviour
{
    public static Referances Instance { get; private set; } // Singleton Instance
    private void Awake()
    {
        // Ensure only one instance of the singleton exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    public GameObject powerUpContainer, enemyContainer,laserContainer,vfxContainer,audioContainer;
    public Image tripTimerImage ,speedTimerImage;

    public void ClearContainers() 
    {
        foreach (Transform child in powerUpContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in enemyContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in laserContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in vfxContainer.transform)
        {
            Destroy(child.gameObject);
        }

    }
    
}
