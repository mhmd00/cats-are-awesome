using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LevleUIManager levelUIManager;

    [SerializeField] private TMP_Text matchesText;
    [SerializeField] private TMP_Text turnsText;

    void Start()
    {
        int difficulty = PlayerPrefs.GetInt("SelectedDifficulty", 0); // Default: Easy
        gridManager.GenerateGrid(difficulty);
    }
}
