using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const float RotationAngle = 90f;

    [Header("Gameplay Config")]
    [Space(5)]
    [Tooltip("The more increase it, the more game is slower")]
    [SerializeField] private float _gameDelay;
    [SerializeField] private float _movementAmountPerStep = 1f;
    [Tooltip("Dependent on Game Speed")]
    [SerializeField] private float _verticalAutoMovementDelay = 1f;
    [Tooltip("Dependent on Game Speed")]
    [SerializeField] private float _verticalMovementDelay = 0.1f;
    [Tooltip("Dependent on Game Speed")]
    [SerializeField] private float _horizontalMovementDelay = 0.3f;

    private BoardManager _boardManager;
    private BlockSpawner _blockSpawner;
    private GameObject _activeBlock;
    private ScoreManager _scoreManager;

    public static GameManager Instance { get; private set; }
    public float GameSpeed { get => _gameDelay; }
    public float MovementAmountPerStep { get => _movementAmountPerStep; }
    public float VerticalAutoMovementDelay { get => _verticalAutoMovementDelay; }
    public float VerticalMovementDelay { get => _verticalMovementDelay; }
    public float HorizontalMovementDelay { get => _horizontalMovementDelay; }
    public GameObject ActiveBlock { get => _activeBlock; }

    public bool IsBlockOnBoard(Vector2Int[] blockPieces)
    {
        foreach (var blockPiece in blockPieces)
        {
            int blockPositionX = blockPiece.x;
            int blockPositionY = blockPiece.y;
            if (blockPositionX < 0 || blockPositionX >= BoardManager.Width
                || blockPositionY < 0)
            {
                return false;
            }

            if (blockPositionY < BoardManager.Height
                && _boardManager.BoardTileStates[blockPositionY, blockPositionX] != null)
            {
                return false;
            }
        }

        return true;
    }

    public void SpawnBlockWrapper()
    {
        if (CheckGameOver())
        {
            HandleGameOver();
        }
        else
        {
            _boardManager.UpdateBoardTileState(_activeBlock.transform);
            //Update score
            _scoreManager.AddBlockSuccessScore();

            SoundManager.Instance.PlayBlockLandSuccess();

            int cleanedRow = _boardManager.ClearAllRows();
            //If there is cleaned row, then play sound
            if (cleanedRow > 0)
            {
                SoundManager.Instance.PlayRowCleaning();
            }
            //Update Score
            _scoreManager.AddRowCleaningScore(cleanedRow);

            _activeBlock = _blockSpawner.SpawnBlock();

            //When process dotween animation, do not allow block to move
            //It is also effect preview block scale
            _activeBlock.GetComponent<BlockMover>().enabled = false;

            _activeBlock.transform.localScale = Vector3.zero;
            _activeBlock.transform.DOScale(1, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                _activeBlock.GetComponent<BlockMover>().enabled = true;
            });
        }
    }

    private void HandleGameOver()
    {
        _activeBlock.GetComponent<BlockMover>().enabled = false;
        UIManager.Instance.OpenGameOverCanvas();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _boardManager = FindAnyObjectByType<BoardManager>();
        if (_boardManager == null)
        {
            Debug.LogWarning("Board Manager not found", gameObject);
        }

        _blockSpawner = FindAnyObjectByType<BlockSpawner>();
        if (_blockSpawner == null)
        {
            Debug.LogWarning("Block spawner not found", gameObject);
        }

        _scoreManager = FindAnyObjectByType<ScoreManager>();
        if (_scoreManager == null)
        {
            Debug.LogWarning("Score Manager not found", gameObject);
        }
    }

    private void Start()
    {
        _activeBlock = _blockSpawner.SpawnBlock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.OpenPauseCanvas();
        }
    }

    private bool CheckGameOver()
    {
        foreach (Transform blockPiece in _activeBlock.transform)
        {
            if (blockPiece.position.y >= BoardManager.Height)
            {
                return true;
            }
        }

        return false;
    }
}
