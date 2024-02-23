using TMPro;
using UnityEngine;

public class UserScoreDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreTMP;
    [SerializeField] private TextMeshProUGUI _levelTMP;

    private const int MinScore = 0;
    private const int MinLevel = 1;

    private void OnEnable()
    {
        ScoreManager.OnLevelChange += ScoreManager_OnLevelChange;
        ScoreManager.OnScoreChange += ScoreManager_OnScoreChange;
    }

    private void OnDisable()
    {
        ScoreManager.OnLevelChange -= ScoreManager_OnLevelChange;
        ScoreManager.OnScoreChange -= ScoreManager_OnScoreChange;
    }

    private void InitializeScoreAndLevel()
    {
        _scoreTMP.text = MinScore.ToString();
        _levelTMP.text = MinLevel.ToString();
    }

    private void Awake()
    {
        InitializeScoreAndLevel();
    }

    private void ScoreManager_OnScoreChange(int score)
    {
        _scoreTMP.text = score.ToString();
    }

    private void ScoreManager_OnLevelChange(int level)
    {
        _levelTMP.text = level.ToString();
    }
}
