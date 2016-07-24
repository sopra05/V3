namespace V3.Screens
{
    public interface IScreenFactory
    {
        GameScreen CreateGameScreen();

        HudScreen CreateHudScreen();

        LoadScreen CreateLoadScreen();

        MainScreen CreateMainScreen();

        PauseScreen CreatePauseScreen();

        OptionsScreen CreateOptionsScreen();

        DeathScreen CreateDeathScreen();

        VictoryScreen CreateVictoryScreen();

        TechdemoScreen CreaTechdemoScreen();

        StatisticsScreen CreateStatisticsScreen();

        AchievementsScreen CreateAchievementsScreen();
    }
}
