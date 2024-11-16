using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevleUIManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameManager gameManager;

    [SerializeField] Image levelModeImageUI;
    [SerializeField] Image levelBackground;

    [SerializeField] private TMP_Text levelNameTextUI;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text matchesText;
    [SerializeField] private TMP_Text turnsText;
    [SerializeField] private TMP_Text trialsText;

    private int totalMatches = 0;
    private int totalTurns = 0;
    private int totalTrials = 3;

    private bool isCountdownActive = false;

    public bool IsCountdownActive()
    {
        return isCountdownActive;
    }


    public void DisplayLevelModeImage(LevelData levelData)
    {
        if (levelModeImageUI != null && levelData.levelModeImage != null)
        {
            levelModeImageUI.sprite = levelData.levelModeImage;
            levelModeImageUI.enabled = true;
        }
        else
        {
            levelModeImageUI.enabled = false;
        }
    }
    public void SetLevelBackground(LevelData levelData)
    {
        if (levelBackground != null && levelData.backgroundSprite != null)
        {
            levelBackground.sprite = levelData.backgroundSprite;
        }
    }
    public void DisplayLevelName(LevelData levelData)
    {
        if (levelNameTextUI != null && !string.IsNullOrEmpty(levelData.levelName))
        {
            levelNameTextUI.text = levelData.levelName;
            levelNameTextUI.enabled = true;
        }
        else
        {
            levelNameTextUI.enabled = false;
        }
    }
    public void UpdateMatches()
    {
        totalMatches++;
        matchesText.text = totalMatches.ToString();
    }

    public void UpdateTurns()
    {
        totalTurns++;
        turnsText.text = totalTurns.ToString();
    }

    private void UpdateTrialsUI()
    {
        trialsText.text = totalTrials.ToString();
    }
    public int GetTotalMatches()
    {
        return totalMatches;
    }

    public void StartCountdown(GameObject[] cards, float duration)
    {
        isCountdownActive = true;
        cardManager.LockCardInteraction();
        StartCoroutine(ShowAndHideCardsWithCountdown(cards, duration));
    }

    private IEnumerator ShowAndHideCardsWithCountdown(GameObject[] cards, float showDuration)
    {
        UpdateCountdownUI(showDuration);

        foreach (GameObject card in cards)
        {
            if (card != null)
            {
                CardBehaviour cardBehaviour = card.GetComponent<CardBehaviour>();
                if (cardBehaviour != null)
                {
                    cardBehaviour.FlipCard();
                }
            }
        }

        float timeLeft = showDuration;
        while (timeLeft > 0)
        {
            UpdateCountdownUI(timeLeft);
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        foreach (GameObject card in cards)
        {
            if (card != null)
            {
                CardBehaviour cardBehaviour = card.GetComponent<CardBehaviour>();
                if (cardBehaviour != null)
                {
                    cardBehaviour.ResetCard();
                }
            }
        }

        HideCountdownUI();
        isCountdownActive = false;
        cardManager.UnlockCardInteraction();
    }

    private void UpdateCountdownUI(float timeLeft)
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = $"{timeLeft:F0}";
    }

    private void HideCountdownUI()
    {
        countdownText.gameObject.SetActive(false);
    }

    public void ResetLevel()
    {
        totalMatches = 0;
        totalTurns = 0;
        totalTrials = gameManager.totalTrials;

        UpdateTurns();
        UpdateMatches();
        UpdateTrialsUI();
    }
}
