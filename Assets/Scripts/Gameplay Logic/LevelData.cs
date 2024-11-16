using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "MemoryGame/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Info")]
    public string levelName;
    public MusicType backgroundMusicType;

    [Header("Card Settings")]
    public Sprite[] cardImages;
    public Sprite backgroundSprite;
    public float showDuration;

    [Header("Grid Settings")]
    public int rows = 3;
    public int columns = 3;

    [Header("Level Mode Image")]
    public Sprite levelModeImage;
}
