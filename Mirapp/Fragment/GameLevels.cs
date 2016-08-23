namespace Mirapp
{
    public enum GameLevels
    {
        Easy,Middle,Hard
    }

    public static class GameLevelOperation
    {
        public static GameLevels GetGameLevel(string gameLevel)
        {
            switch (gameLevel)
            {
                case "Easy":
                    return GameLevels.Easy;
                case "Middle":
                    return GameLevels.Middle;
                case "Hard":
                    return GameLevels.Hard;
                default:
                    return GameLevels.Easy;
            }

        }
    }
}