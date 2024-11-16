using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LevelData levelData;

    public LevelData LoadLevelData(int difficulty)
    {
        string levelPath = GetLevelPathForDifficulty(difficulty);
        levelData = Resources.Load<LevelData>(levelPath);
        return levelData;
    }

    public Sprite[] ShuffleImages()
    {
        Sprite[] images = new Sprite[levelData.cardImages.Length];
        int totalPairs = (levelData.cardImages.Length) / 2;

        for (int i = 0; i < totalPairs; i++)
        {
            images[2 * i] = levelData.cardImages[i % levelData.cardImages.Length];
            images[2 * i + 1] = levelData.cardImages[i % levelData.cardImages.Length];
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

    private string GetLevelPathForDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0: return "Scriptable/Levels/EasyLevelData";
            case 1: return "Scriptable/Levels/MediumLevelData";
            case 2: return "Scriptable/Levels/HardLevelData";
            default: return null;
        }
    }
    public void SetBackgroundMusic(LevelData levelData)
    {
        if (levelData.backgroundMusicName != null)
        {
            SoundManager.Instance.PlayMusic(levelData.backgroundMusicName);
        }
    }
}
