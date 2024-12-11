using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;     // The text object displaying the score
    public float incrementSpeed = 0.05f; // Time between each increment animation

    private int currentScore = 0;        // Current visible score
    private int targetScore = 0;         // Final score to reach
    private Coroutine incrementCoroutine;

    [Header("Health Settings")]
    public GameObject healthSpritesParent; // Parent GameObject containing health sprites
    public Color damageColor = Color.red;  // Color to indicate damage
    public float colorResetDuration = 0.5f; // Time to reset color after damage
    private int currentHealth;             // Tracks current health
    private Color defaultHealthColor;

    private void Start()
    {
        // Initialize score display with six digits
        scoreText.text = "asdasd"/*currentScore.ToString("D6")*/;

        defaultHealthColor = healthSpritesParent.transform.GetChild(0).GetComponent<Image>().color;
    }
    public void AddScore(int amount)
    {
        targetScore += amount;

        // If already incrementing, stop and start fresh
        if (incrementCoroutine != null)
        {
            StopCoroutine(incrementCoroutine);
        }

        incrementCoroutine = StartCoroutine(UpdateScore());
    }

    private IEnumerator UpdateScore()
    {
        while (currentScore < targetScore)
        {
            currentScore+=5;
            scoreText.text = currentScore.ToString("D6"); // Format as six-digit number

            // Wait before incrementing again
            yield return new WaitForSeconds(incrementSpeed);
        }

        incrementCoroutine = null; // Reset coroutine reference when done
    }

    public void DamagePlayer(int healthPoint)
    {
       
        Transform damagedSprite = healthSpritesParent.transform.GetChild(healthPoint-1);
        Image spriteImage = damagedSprite.GetComponent<Image>();
        
        if (spriteImage != null)
        {
            
            // Change the sprite color to indicate damage
            spriteImage.color = damageColor;

            // Reset the color after a delay
            //StartCoroutine(ResetSpriteColor(spriteImage));
        }

        
    }

    // Reset sprite color to its default
    private IEnumerator ResetSpriteColor(Image spriteImage)
    {
        yield return new WaitForSeconds(colorResetDuration);

        // Reset to white (default color for UI Images)
        spriteImage.color = Color.white;
    }
}
