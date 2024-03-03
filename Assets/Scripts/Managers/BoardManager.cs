using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Debug")]
    [Space(5)]
    [SerializeField] private bool _drawTileStates;
    [SerializeField] private bool _drawRowOccupancyStates;

    [Header("VFX")]
    [Space(5)]
    [SerializeField] private Transform[] _vfxRowComplete;

    [Header("General Settings")]
    [Space(5)]
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _tileStatePrefab;
    [SerializeField] private Color _tileOccupiedColor;
    [SerializeField] private Color _tileEmptyColor;

    public const int Width = 10;
    public const int Height = 20;

    private Transform[,] _boardTileStates = new Transform[Height, Width];

    public Transform[,] BoardTileStates { get => _boardTileStates; }

    public void UpdateBoardTileState(Transform block)
    {
        foreach (Transform blockPiece in block)
        {
            int blockPieceY = Mathf.RoundToInt(blockPiece.position.y);
            int blockPieceX = Mathf.RoundToInt(blockPiece.position.x);
            _boardTileStates[blockPieceY, blockPieceX] = blockPiece;
        }
    }

    public int ClearAllRows()
    {
        int clearedRow = 0;
        int rowIndex = 0;
        while (rowIndex < Height)
        {
            if (IsRowFull(rowIndex))
            {
                ++clearedRow;
                ClearRow(rowIndex);
                if (rowIndex < _vfxRowComplete.Length)
                {
                    PlayRowCompleteVFX(rowIndex);
                }
                //Start with upper row to move down
                MoveAllRowsDown(rowIndex + 1);
            }
            else
            {
                ++rowIndex;
            }
        }

        return clearedRow;
    }

    private void PlayRowCompleteVFX(int rowIndex)
    {
        Transform rowParent = _vfxRowComplete[rowIndex];
        foreach (Transform rowMember in rowParent)
        {
            if (rowMember.TryGetComponent(out ParticleSystem rowCompleteVFX))
            {
                rowCompleteVFX.Play();
            }
        }
    }

    private bool IsRowFull(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= Height)
        {
            Debug.LogError("Row index out of range");
            return false;
        }

        for (int columnIndex = 0; columnIndex < Width; columnIndex++)
        {
            if (_boardTileStates[rowIndex, columnIndex] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void ClearRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= Height)
        {
            Debug.LogError("Row index out of range");
            return;
        }

        for (int columnIndex = 0; columnIndex < Width; columnIndex++)
        {
            if (_boardTileStates[rowIndex, columnIndex] != null)
            {
                GameObject destroyedObject = _boardTileStates[rowIndex, columnIndex].gameObject;
                Destroy(destroyedObject);
            }

            _boardTileStates[rowIndex, columnIndex] = null;
        }
    }

    private void MoveRowDown(int rowIndex)
    {
        if (rowIndex <= 0 || rowIndex >= Height)
        {
            Debug.LogError("Row index out of range");
            return;
        }

        for (int columnIndex = 0; columnIndex < Width; columnIndex++)
        {
            if (_boardTileStates[rowIndex, columnIndex] != null)
            {
                _boardTileStates[rowIndex - 1, columnIndex] = _boardTileStates[rowIndex, columnIndex];
                _boardTileStates[rowIndex, columnIndex] = null;
                _boardTileStates[rowIndex - 1, columnIndex].position += Vector3.down * GameManager.Instance.MovementAmountPerStep;
            }
        }
    }

    private void MoveAllRowsDown(int startingRowIndex)
    {
        for (int rowIndex = startingRowIndex; rowIndex < Height; rowIndex++)
        {
            MoveRowDown(rowIndex);
        }
    }

    private void OnDrawGizmos()
    {
        if (_drawTileStates)
        {
            //Draw Tile States
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Gizmos.color = _boardTileStates[i, j] switch
                    {
                        null => _tileEmptyColor,
                        _ => _tileOccupiedColor
                    };
                    Gizmos.DrawSphere(new Vector2(j, i), 0.2f);

                }
            }
        }

        if (_drawRowOccupancyStates)
        {
            //Draw Row Occupancy States
            int columnIndex = -1;
            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                if (IsRowFull(rowIndex))
                {
                    Gizmos.color = _tileOccupiedColor;
                }
                else
                {
                    Gizmos.color = _tileEmptyColor;
                }

                Gizmos.DrawSphere(new Vector2(columnIndex, rowIndex), 0.3f);
            }
        }

    }

    private void Start()
    {
        CreateEmptyTiles();
    }

    private void CreateEmptyTiles()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Vector3 spawnPosition = new(j, i, 0);
                GameObject spawnedObject = Instantiate(_tilePrefab, spawnPosition, Quaternion.identity);
                spawnedObject.name = $"{j}, {i}";
                spawnedObject.transform.parent = transform;
            }
        }
    }
}
