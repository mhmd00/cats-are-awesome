using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject cardPrefab;
    public Transform gridParent;
    public GridLayoutGroup gridLayout;

    [SerializeField] CardManager cardManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] LevleUIManager levleUIManager;
    private int rows;
    private int columns;
    private LevelData levelData;

    private void Start()
    {
        int difficulty = PlayerPrefs.GetInt("SelectedDifficulty", 0);
        GenerateGrid(difficulty);
    }

    public void GenerateGrid(int difficulty)
    {
        levelData = levelManager.LoadLevelData(difficulty);
        ClearGrid();
        SetGridLayout();
        CreateCards();
        levelManager.SetBackgroundMusic(levelData);
        levleUIManager.DisplayLevelName(levelData);
        levleUIManager.SetLevelBackground(levelData);
        levleUIManager.DisplayLevelModeImage(levelData);
    }
    private void ClearGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }
    private void SetGridLayout()
    {
        SetDifficultyGridSize();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
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

    private void CreateCards()
    {
        GameObject[] cards = new GameObject[rows * columns];
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject card = Instantiate(cardPrefab, gridParent);
            cards[i] = card;
        }

        cardManager.AssignImagesToCards(levelData,cards);
        cardManager.AdjustCardSize(rows, columns, gridLayout, gridParent);
        levleUIManager.StartCountdown(cards, levelData.showDuration);
    }

}
