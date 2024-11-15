using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridManager gridManager;

    void Start()
    {
        int difficulty = PlayerPrefs.GetInt("SelectedDifficulty", 0); // Default: Easy
        gridManager.GenerateGrid(difficulty);
    }
}
