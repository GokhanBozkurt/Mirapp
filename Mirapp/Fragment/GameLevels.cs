namespace Mirapp
{
    public enum GameLevels
    {
        Easy,Medium,Hard
    }

    public static class GameLevelOperation
    {
        public static GameLevels GetGameLevel(string gameLevel)
        {
            switch (gameLevel)
            {
                case "Easy":
                    return GameLevels.Easy;
                case "Medium":
                    return GameLevels.Medium;
                case "Hard":
                    return GameLevels.Hard;
                default:
                    return GameLevels.Easy;
            }

        }
    }
}