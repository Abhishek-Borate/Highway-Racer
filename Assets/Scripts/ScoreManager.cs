using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    private float score = 0;
    private bool isScoring = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false); // Hide score text initially
        }
        else
        {
            Debug.LogError("ScoreText UI is not assigned in ScoreManager!");
        }
    }

    void Update()
    {
        if (!isScoring || scoreText == null) return;

        if (BikeController.instance == null)
        {
            Debug.LogError("BikeController instance is NULL! Make sure BikeController exists in the scene.");
            return;
        }

        float speed = BikeController.instance.GetCurrentSpeed();
        score += speed * Time.deltaTime * 10;
        scoreText.text = "Score: " + Mathf.RoundToInt(score).ToString();
    }

    public void StartScoring()
    {
        isScoring = true;
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true); // Show score when game starts
        }
    }

    public void StopScoring()
    {
        isScoring = false;
    }

    public float GetFinalScore()
    {
        return score;
    }
}
