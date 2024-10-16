using UnityEngine;

/// <summary>
/// This is example how you can store data.
/// </summary>
public class GameResult
{
    // Score in "game".
    public int score;

    /// <summary>
    /// Method used to generate random game result.
    /// </summary>
    /// <returns>Random  game result.</returns>
    public static GameResult GetRandomResult()
    {
        return new GameResult { score = Random.Range(0, 100) };
    }
}
