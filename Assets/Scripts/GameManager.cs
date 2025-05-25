using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using TMPro;             

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText; 
    public float scoreMultiplier = 10f;

    private float currentScore = 0f;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        Time.timeScale = 1f; 
        isGameOver = false;
        currentScore = 0f; 
        UpdateScoreDisplay();
    }

    void Update()
    {
        if (!isGameOver)
        {
           
            currentScore += Time.deltaTime * scoreMultiplier;
            UpdateScoreDisplay();
        }
        else
        {
         
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(currentScore);
        }
    }

    public void PlayerHitObstacle()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Time.timeScale = 0f; 
            Debug.Log("Game Over!");
            if (scoreText != null) 
            {
                scoreText.text = "Game Over!\nScore: " + Mathf.FloorToInt(currentScore) + "\nPress R to Restart";
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}