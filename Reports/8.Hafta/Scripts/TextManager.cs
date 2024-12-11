using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI scoreValueText;     
    public float incrementSpeed = 0.05f; 

    private int currentScore = 0;        
    private int targetScore = 0;         
    private Coroutine incrementCoroutine;

    [Header("Health Settings")]
    public GameObject healthSpritesParent; 
    public Color damageColor = Color.red;            
    private Color defaultHealthColor=Color.green;

    [Header("UI Settings")]
    public GameObject top;
    public GameObject middle,bottom, gameoverGameObject;
    public TextMeshProUGUI endScoreValueText;
    public float flickerSpeed = 0.2f;  

    private Coroutine flickerCoroutine;
    private bool gameEnded= false;

    private void Awake()
    {
        if (gameoverGameObject.activeSelf) 
        {
            gameoverGameObject.SetActive(false);
        }
        
    }
    private void Start()
    {

        scoreValueText.text = "000000";

        defaultHealthColor = healthSpritesParent.transform.GetChild(0).GetComponent<Image>().color;
        
    }

    private void Update()
    {
        if(gameEnded&& Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public void AddScore(int amount)
    {
        targetScore += amount;

        
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
            scoreValueText.text = currentScore.ToString("D6"); 

            yield return new WaitForSeconds(incrementSpeed);
        }

        incrementCoroutine = null;
    }

    public void DamagePlayer(int healthPoint)
    {
       
        Transform damagedSprite = healthSpritesParent.transform.GetChild(healthPoint-1);
        Image spriteImage = damagedSprite.GetComponent<Image>();
        
        if (spriteImage != null)
        {
            
            spriteImage.color = damageColor;

        }

    }

    public IEnumerator PowerupUIUpdate(Image powerUpImage, float duration)
    {

        powerUpImage.gameObject.transform.SetSiblingIndex(1);
        powerUpImage.gameObject.SetActive(true);
        powerUpImage.fillAmount = 1;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            powerUpImage.fillAmount = 1 - (elapsedTime / duration);
            yield return null;
        }

        powerUpImage.fillAmount = 0;
        powerUpImage.gameObject.SetActive(false);
    }

    public void GameoverUIUpdates() 
    {
        top.SetActive(false);
        gameoverGameObject.SetActive(true);
        bottom.SetActive(false);
        endScoreValueText.text = currentScore.ToString("D6");
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
        }
        flickerCoroutine = StartCoroutine(FlickerText());
        gameEnded = true;
    }

    

    private IEnumerator FlickerText()
    {
        TextMeshProUGUI[] texts = middle.GetComponentsInChildren<TextMeshProUGUI>();
        while (true)
        {
            foreach (TextMeshProUGUI text in texts) 
            {
                Color color = text.color;
                color.a = color.a == 1f ? 0f : 1f;
                text.color = color;
            }
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
