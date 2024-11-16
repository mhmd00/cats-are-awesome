using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public Button homeButton;

    private void Start()
    {
        homeButton.onClick.AddListener(OnHomeButtonClicked);
    }

    public void OnHomeButtonClicked()
    {
        PlayButtonClickSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayButtonClickSound()
    {
        SoundManager.Instance.PlaySound(SoundEffectType.Click);
    }
}
