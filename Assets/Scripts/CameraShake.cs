using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private float perlinSeedX;
    private float perlinSeedY;

    private void Awake()
    {
        originalPosition = transform.localPosition;

        // Generate random seeds for Perlin noise to add variety
        perlinSeedX = Random.Range(0f, 100f);
        perlinSeedY = Random.Range(0f, 100f);
    }

    // Coroutine for camera shake
    public IEnumerator Shake(float duration, float magnitude, float dampingSpeed = 1.0f)
    {
        float elapsed = 0f;


        while (elapsed < duration)
        {
            // Use Perlin noise to get smooth, random offsets
            float shakeX = Mathf.PerlinNoise(perlinSeedX, Time.time * dampingSpeed) * 2 - 1;
            float shakeY = Mathf.PerlinNoise(perlinSeedY, Time.time * dampingSpeed) * 2 - 1;

            // Apply shake magnitude and adjust camera position
            Vector3 shakeOffset = new Vector3(shakeX, shakeY, 0) * magnitude;
            transform.localPosition = originalPosition + shakeOffset;

            // Increase elapsed time
            elapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Reset the camera position after the shake is over
        transform.localPosition = originalPosition;
    }

    // Function to call the coroutine from other scripts
    public void StartShake(float duration, float magnitude, float dampingSpeed = 5.0f)
    {
        // Stop any ongoing shake before starting a new one
        StopAllCoroutines();
        StartCoroutine(Shake(duration, magnitude, dampingSpeed));
    }
}
