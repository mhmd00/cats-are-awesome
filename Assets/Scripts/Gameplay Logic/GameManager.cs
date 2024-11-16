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
    [SerializeField] private GameObject victoryMenu;
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
    public void CheckUserVictory()
    {
        if (levelUIManager.GetTotalMatches() == gridManager.GetTotalCardPairs())
        {
            ActivateVictoryMenu();
        }
    }
    private void UpdateTrialsUI()
    {
        trialsText.text = totalTrials.ToString();
    }

    private void ActivateGameOverMenu()
    {
        SoundManager.Instance.PlayMusic(MusicType.GameOver);
        UIAnimator.ShowUI(gameOverMenu);
    }

    private void ActivateVictoryMenu()
    {
        SoundManager.Instance.PlayMusic(MusicType.Victory);
        UIAnimator.ShowUI(victoryMenu);
    }

    public void RestartGame()
    {
        totalTrials = 3;
        UpdateTrialsUI();
        UIAnimator.HideUI(gameOverMenu);
        UIAnimator.HideUI(victoryMenu);

        GameData savedData = SaveSystem.LoadGameData();
        gridManager.ResetGrid(savedData.selectedDifficulty);
        levelUIManager.ResetLevel();
        Time.timeScale = 1;
    }
}
