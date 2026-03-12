// GameState.cs
public static class GameState
{
    // Edit this to your real name now, e.g. "Jane Doe"
    public static string playerName = "Sanay Jog";

    // runtime score (updated by Enemy on death)
    public static int score = 0;

    public static void ResetForNewRun()
    {
        score = 0;
    }
}