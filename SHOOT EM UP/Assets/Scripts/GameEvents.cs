using System;

public static class GameEvents
{
    // Pass the points scored for that enemy
    public static event Action<int> EnemyDestroyed;
    public static void RaiseEnemyDestroyed(int points) => EnemyDestroyed?.Invoke(points);

    public static event Action PlayerDied;
    public static void RaisePlayerDied() => PlayerDied?.Invoke();
}