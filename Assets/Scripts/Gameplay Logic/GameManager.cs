using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LevleUIManager levelUIManager;

    [SerializeField] private TMP_Text matchesText;
    [SerializeField] private TMP_Text turnsText;
    [SerializeField] private TMP_Text trialsText;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button restartButton;

    public int totalTrials = 3;

    void Start()
    {
        GameData savedData = SaveSystem.LoadGameData();
        gridManager.GenerateGrid(savedData.selectedDifficulty);
        UpdateTrialsUI();

        restartButton.onClick.AddListener(RestartGame);
    }

    public void CheckTrialStatus(bool isMatchCorrect)
    {
        if (!isMatchCorrect)
        {
            totalTrials--;
            UpdateTrialsUI();

            if (totalTrials <= 0)
            {
                ActivateGameOverMenu();
            }
        }
    }

    private void UpdateTrialsUI()
    {
        trialsText.text = totalTrials.ToString();
    }

    private void ActivateGameOverMenu()
    {
        SoundManager.Instance.PlayMusic("GameOver");
        UIAnimator.ShowUI(gameOverMenu);
    }

    public void RestartGame()
    {
        totalTrials = 3;
        UpdateTrialsUI();
        UIAnimator.HideUI(gameOverMenu);

        GameData savedData = SaveSystem.LoadGameData();
        gridManager.ResetGrid(savedData.selectedDifficulty);
        levelUIManager.ResetLevel();
    }
}
