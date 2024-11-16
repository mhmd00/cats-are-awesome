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
        GameData savedData = SaveSystem.LoadGameData();
        gridManager.GenerateGrid(savedData.selectedDifficulty);
    }
}
