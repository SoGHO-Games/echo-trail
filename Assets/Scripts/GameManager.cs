using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int ActiveLevelIndex { get; set; } = 0;

    public List<LevelData> LevelDatas { get; set; } = new List<LevelData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadLevelDatas();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CloseLevel(int levelIndex, int echoesUsedCount, int totalDeadCount)
    {
        if (levelIndex < 0 || levelIndex >= LevelDatas.Count)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }

        LevelData levelData = LevelDatas[levelIndex];
        levelData.EchoCount = echoesUsedCount;
        levelData.DeadCount = totalDeadCount;
    }

    private void LoadLevelDatas()
    {
        LevelDatas.Clear();
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            var sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (sceneName.StartsWith("Level "))
            {
                LevelDatas.Add(new LevelData(sceneName, i));
            }
        }
    }
}
