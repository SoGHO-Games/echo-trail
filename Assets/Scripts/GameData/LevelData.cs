public class LevelData
{
    public string LevelName { get; set; }
    public int LevelIndex { get; set; }
    public int EchoCount { get; set; }
    public int DeadCount { get; set; }

    public LevelData(string levelName, int levelIndex)
    {
        LevelName = levelName;
        LevelIndex = levelIndex;
    }

    public string GetDisplayName()
    {
        if (EchoCount > 0)
        {
            return $"{LevelName} (E: {EchoCount} / D: {DeadCount})";
        }
        else
        {
            return LevelName;
        }
    }
}