using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public Toggle toggleEasy;
    public Toggle toggleMedium;
    public Toggle toggleHard;
    [SerializeField] SettingsMenu settingsMenu;

    private const string DifficultyKey = "SelectedDifficulty";

    void Start()
    {
        GameData savedData = SaveSystem.LoadGameData();
        SetDifficulty(savedData.selectedDifficulty);

        toggleEasy.onValueChanged.AddListener((value) => OnDifficultyChanged(0, value));
        toggleMedium.onValueChanged.AddListener((value) => OnDifficultyChanged(1, value));
        toggleHard.onValueChanged.AddListener((value) => OnDifficultyChanged(2, value));
    }

    private void OnDifficultyChanged(int difficulty, bool isOn)
    {
        settingsMenu.PlayButtonClickSound();
        if (isOn)
        {
            GameData data = SaveSystem.LoadGameData();
            data.selectedDifficulty = difficulty;
            SaveSystem.SaveGameData(data);
        }
    }

    private void SetDifficulty(int difficulty)
    {
        toggleEasy.isOn = (difficulty == 0);
        toggleMedium.isOn = (difficulty == 1);
        toggleHard.isOn = (difficulty == 2);
    }
}
