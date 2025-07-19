using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Import for UI elements
using TMPro; 


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TextMeshProUGUI  finalScoreText;  // Added this!

    void Start()
    {
        startMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        Time.timeScale = 0;  // Pause game initially
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        Time.timeScale = 1;  // Resume game
        
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.StartScoring();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button Clicked!");  // Ensure this appears in Console
        Application.Quit();
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0; // Pause game

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.StopScoring();
            finalScoreText.text = "Final Score: " + ScoreManager.instance.GetFinalScore();
        }
    }
}
