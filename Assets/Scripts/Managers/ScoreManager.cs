using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> OnScoreChange;
    public static event Action<int> OnLevelChange;

    private int _level = 1;
    private int _blockLandSuccessScore = 10;
    private int _rowCleaningScore = 100;
    private int _nextLevelScore = 100;
    private int _currentScore = 0;

    public void AddBlockSuccessScore()
    {
        _currentScore += _blockLandSuccessScore * _level;
        OnScoreChange?.Invoke(_currentScore);
        CheckLevelUp();
    }

    public void AddRowCleaningScore(int numOfCleanedRow)
    {
        _currentScore += _rowCleaningScore * _level * numOfCleanedRow;
        OnScoreChange?.Invoke(_currentScore);
        CheckLevelUp();
    }

    public int GetHighScore()
    {
        int highScore = PlayerPrefs.GetInt(Utilities.ScoreSystem.HighScoreKey, 0);
        return highScore;
    }

    private void CheckLevelUp()
    {
        while (_currentScore > _nextLevelScore)
        {
            ++_level;
            OnLevelChange?.Invoke(_level);
            _nextLevelScore *= Mathf.RoundToInt(Mathf.Pow(_level, 2));
        }
    }

    private void SaveHighScore(int score)
    {
        int highScore = PlayerPrefs.GetInt(Utilities.ScoreSystem.HighScoreKey, 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt(Utilities.ScoreSystem.HighScoreKey, score);
        }
    }

    private void OnEnable()
    {
        OnScoreChange += SaveHighScore;
    }

    private void OnDisable()
    {
        OnScoreChange -= SaveHighScore;
    }
}
