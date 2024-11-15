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
        int savedDifficulty = PlayerPrefs.GetInt(DifficultyKey, 0); // 0 = Easy, 1 = Medium, 2 = Hard
        SetDifficulty(savedDifficulty);

        toggleEasy.onValueChanged.AddListener((value) => OnDifficultyChanged(0, value));
        toggleMedium.onValueChanged.AddListener((value) => OnDifficultyChanged(1, value));
        toggleHard.onValueChanged.AddListener((value) => OnDifficultyChanged(2, value));
    }

    private void OnDifficultyChanged(int difficulty, bool isOn)
    {
        settingsMenu.PlayButtonClickSound();
        if (isOn)
        {
            PlayerPrefs.SetInt(DifficultyKey, difficulty);
        }
    }

    private void SetDifficulty(int difficulty)
    {
        toggleEasy.isOn = (difficulty == 0);
        toggleMedium.isOn = (difficulty == 1);
        toggleHard.isOn = (difficulty == 2);
    }
}
