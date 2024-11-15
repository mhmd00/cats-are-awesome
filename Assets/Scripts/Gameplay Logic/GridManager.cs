using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject cardPrefab;
    public Transform gridParent;
    public GridLayoutGroup gridLayout;
    public Image background;
    public Image levelModeImageUI;
    public TMP_Text levelNameTextUI;
    [SerializeField] TMP_Text countdownText;

    private int rows;
    private int columns;

    private LevelData levelData;

    public void GenerateGrid(int difficulty)
    {
        levelData = LoadLevelData(difficulty);

        if (levelData == null)
        {
            Debug.LogError("Level Data could not be loaded.");
            return;
        }

        SetDifficultyGridSize();

        SetLevelBackground();
        SetBackgroundMusic();
        DisplayLevelModeImage();
        DisplayLevelName();

        ClearGrid();

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        GameObject[] cards = new GameObject[rows * columns];
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject card = Instantiate(cardPrefab, gridParent);
            cards[i] = card;
        }

        AssignImagesToCards(cards);

        AdjustCardSize();

        StartCoroutine(ShowAndHideCardsWithCountdown(cards, levelData.showDuration));
    }

    private IEnumerator ShowAndHideCardsWithCountdown(GameObject[] cards, float showDuration)
    {
        UpdateCountdownUI(showDuration);

        foreach (GameObject card in cards)
        {
            CardBehaviour cardBehaviour = card.GetComponent<CardBehaviour>();
            if (cardBehaviour != null)
            {
                cardBehaviour.FlipCard();
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
            CardBehaviour cardBehaviour = card.GetComponent<CardBehaviour>();
            if (cardBehaviour != null)
            {
                cardBehaviour.ResetCard();
            }
        }

        HideCountdownUI();
    }
    private void UpdateCountdownUI(float timeLeft)
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = $"{timeLeft:F0}";
        }
    }

    private void HideCountdownUI()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }
    private LevelData LoadLevelData(int difficulty)
    {
        string levelPath = GetLevelPathForDifficulty(difficulty);
        return Resources.Load<LevelData>(levelPath);
    }

    private string GetLevelPathForDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                return "Scriptable/Levels/EasyLevelData";
            case 1:
                return "Scriptable/Levels/MediumLevelData";
            case 2:
                return "Scriptable/Levels/HardLevelData";
            default:
                Debug.LogError("Invalid difficulty level.");
                return null;
        }
    }

    private void SetDifficultyGridSize()
    {
        if (levelData != null)
        {
            rows = levelData.rows;
            columns = levelData.columns;
        }
        else
        {
            Debug.LogError("Level Data is not assigned.");
        }
    }

    private void SetLevelBackground()
    {
        if (background != null && levelData.backgroundSprite != null)
        {
            background.sprite = levelData.backgroundSprite;
        }
    }

    private void SetBackgroundMusic()
    {
        if (levelData.backgroundMusicName != null)
        {
            SoundManager.Instance.PlayMusic(levelData.backgroundMusicName);
        }
    }

    private void DisplayLevelModeImage()
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

    private void DisplayLevelName()
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

    private void ClearGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void AdjustCardSize()
    {
        RectTransform gridRect = gridParent.GetComponent<RectTransform>();
        float gridWidth = gridRect.rect.width;
        float gridHeight = gridRect.rect.height;

        float cardWidth = gridWidth / columns - gridLayout.spacing.x;
        float cardHeight = gridHeight / rows - gridLayout.spacing.y;

        float cardSize = Mathf.Min(cardWidth, cardHeight);
        gridLayout.cellSize = new Vector2(cardSize, cardSize);
    }

    private void AssignImagesToCards(GameObject[] cards)
    {
        Sprite[] shuffledImages = ShuffleImages(levelData.cardImages);

        for (int i = 0; i < cards.Length; i++)
        {
            CardBehaviour cardBehaviour = cards[i].GetComponent<CardBehaviour>();
            if (cardBehaviour != null && shuffledImages.Length > i)
            {
                cardBehaviour.frontImage.GetComponent<Image>().sprite = shuffledImages[i];
            }
        }
    }

    private Sprite[] ShuffleImages(Sprite[] originalImages)
    {
        Sprite[] images = new Sprite[rows * columns];
        int totalPairs = (rows * columns) / 2;

        for (int i = 0; i < totalPairs; i++)
        {
            images[2 * i] = originalImages[i % originalImages.Length];
            images[2 * i + 1] = originalImages[i % originalImages.Length];
        }

        for (int i = images.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = images[i];
            images[i] = images[randomIndex];
            images[randomIndex] = temp;
        }

        return images;
    }
}
