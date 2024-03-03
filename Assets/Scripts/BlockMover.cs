using System.Collections;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [SerializeField] private BlockType _blockType;
    [SerializeField] private bool _canRotate;
    [SerializeField] private Transform[] _blockPieces;

    public const int BlockCount = 4;

    private PlayerInput _playerInput;
    private bool _isMovingHorizontal = false;
    private bool _isMovingVertical = false;
    private bool _isBlockStopped = false;

    public Transform[] BlockPieces { get => _blockPieces; }
    public BlockType BlockType { get => _blockType; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        StartCoroutine(MoveDownAutomatically());
    }

    private void Update()
    {
        if (_isBlockStopped) return;

        float horizontalInput = GetHorizontalMovementInput();
        bool moveDownInput = GetMoveDownInput();

        if (GetRotationalInput() && _canRotate)
        {
            Rotate();
        }
        else if (horizontalInput != 0 && !_isMovingHorizontal)
        {
            StartCoroutine(MoveHorizontal(horizontalInput));
        }
        else if (moveDownInput && !_isMovingVertical)
        {
            StartCoroutine(MoveDown());
        }
    }

    private void Rotate()
    {
        //First rotate block
        transform.Rotate(Vector3.forward, GameManager.RotationAngle);

        Vector2Int[] nextPoses = new Vector2Int[BlockCount];

        for (int i = 0; i < BlockCount; i++)
        {
            Transform blockPiece = transform.GetChild(i);
            Vector2 blockPiecePos = blockPiece.position;
            int posX = Mathf.RoundToInt(blockPiecePos.x);
            int posY = Mathf.RoundToInt(blockPiecePos.y);

            nextPoses[i].x = posX;
            nextPoses[i].y = posY;
        }

        //After rotation, check if it is on block
        //otherwise reverse
        if (!GameManager.Instance.IsBlockOnBoard(nextPoses))
        {
            transform.Rotate(Vector3.forward, -1f * GameManager.RotationAngle);
        }
        else
        {
            SoundManager.Instance.PlayBlockRotation();
        }
    }

    private void OnBlockStop()
    {
        _isBlockStopped = true;
        GameManager.Instance.SpawnBlockWrapper();
        Destroy(this);
    }

    private IEnumerator MoveDownAutomatically()
    {
        while (true)
        {
            if (_isMovingVertical) continue;

            //Calculate next position for every block piece
            Vector3 movementAmount = Vector3.down * GameManager.Instance.MovementAmountPerStep;
            Vector2Int[] blockPieceNextPositions = GetPreviewMoveOperation(movementAmount);

            if (GameManager.Instance.IsBlockOnBoard(blockPieceNextPositions))
            {
                transform.position += Vector3.down * GameManager.Instance.MovementAmountPerStep;
            }
            else
            {
                OnBlockStop();
                yield break;
            }

            yield return new WaitForSeconds(GameManager.Instance.VerticalAutoMovementDelay * GameManager.Instance.GameSpeed);
        }
    }

    private IEnumerator MoveDown()
    {
        _isMovingHorizontal = true;

        //Calculate next position for every block piece
        Vector3 movementAmount = Vector3.down * GameManager.Instance.MovementAmountPerStep;
        Vector2Int[] blockPieceNextPositions = GetPreviewMoveOperation(movementAmount);

        if (GameManager.Instance.IsBlockOnBoard(blockPieceNextPositions))
        {
            transform.position += Vector3.down * GameManager.Instance.MovementAmountPerStep;
        }

        yield return new WaitForSeconds(GameManager.Instance.VerticalMovementDelay * GameManager.Instance.GameSpeed);

        _isMovingHorizontal = false;
    }

    private IEnumerator MoveHorizontal(float horizontalInput)
    {
        _isMovingHorizontal = true;

        //Calculate next position for every block piece
        Vector3 movementAmount = Vector3.right * (horizontalInput * GameManager.Instance.MovementAmountPerStep);
        Vector2Int[] blockPieceNextPositions = GetPreviewMoveOperation(movementAmount);

        if (GameManager.Instance.IsBlockOnBoard(blockPieceNextPositions))
        {
            transform.position += Vector3.right * (horizontalInput * GameManager.Instance.MovementAmountPerStep);
            SoundManager.Instance.PlayBlockHorizontalMovement();
        }
        else
        {
            SoundManager.Instance.PlayBlockMoveError();
        }

        yield return new WaitForSeconds(GameManager.Instance.HorizontalMovementDelay * GameManager.Instance.GameSpeed);

        _isMovingHorizontal = false;
    }

    private Vector2Int[] GetPreviewMoveOperation(Vector3 movementAmount)
    {
        //Calculate next position for every block piece
        Vector2Int[] nextBlockPiecePositions = new Vector2Int[BlockCount];
        for (int i = 0; i < _blockPieces.Length; i++)
        {
            Vector2 newPosition = _blockPieces[i].position + movementAmount;
            int newPositionX = Mathf.RoundToInt(newPosition.x);
            int newPositionY = Mathf.RoundToInt(newPosition.y);

            Vector2Int newPositionSnapped = new(newPositionX, newPositionY);

            nextBlockPiecePositions[i] = newPositionSnapped;
        }

        return nextBlockPiecePositions;
    }

    private bool GetRotationalInput()
    {
        return _playerInput.FrameInput.RotateInput;
    }

    private float GetHorizontalMovementInput()
    {
        return _playerInput.FrameInput.MoveHorizontalInput;
    }

    private bool GetMoveDownInput()
    {
        return _playerInput.FrameInput.MoveDownInput;
    }
}

public enum BlockType
{
    I, J, L, O, S, T, Z
}
