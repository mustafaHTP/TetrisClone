using System.Collections;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [SerializeField] private BlockType _blockType;
    [SerializeField] private bool _canRotate;
    [SerializeField] private Transform[] _blockPieces;

    public const int BlockCount = 4;

    private bool _isMovingHorizontal = false;
    private bool _isMovingVertical = false;
    private bool _isBlockStopped = false;

    public Transform[] BlockPieces { get => _blockPieces; }
    public BlockType BlockType { get => _blockType; }

    private void Start()
    {
        StartCoroutine(MoveVerticalAutomatically());
    }

    private void Update()
    {
        if (_isBlockStopped) return;

        float horizontalInput = GetHorizontalMovementInput();
        float verticalInput = GetVerticalMovementInput();

        if (GetRotationalInput() && _canRotate)
        {
            Rotate();
        }
        else if (horizontalInput != 0 && !_isMovingHorizontal)
        {
            StartCoroutine(MoveHorizontal(horizontalInput));
        }
        else if (verticalInput == -1f && !_isMovingVertical)
        {
            StartCoroutine(MoveVertical(verticalInput));
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
    }

    private void OnBlockStop()
    {
        _isBlockStopped = true;
        GameManager.Instance.SpawnBlockWrapper();
        Destroy(this);
    }

    private IEnumerator MoveVerticalAutomatically()
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

    private IEnumerator MoveVertical(float verticalInput)
    {
        _isMovingHorizontal = true;

        //Calculate next position for every block piece
        Vector3 movementAmount = Vector3.up * (verticalInput * GameManager.Instance.MovementAmountPerStep);
        Vector2Int[] blockPieceNextPositions = GetPreviewMoveOperation(movementAmount);

        if (GameManager.Instance.IsBlockOnBoard(blockPieceNextPositions))
        {
            transform.position += Vector3.up * (verticalInput * GameManager.Instance.MovementAmountPerStep);
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
            SoundManager.Instance.PlayBlockHorizontalMove();
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
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
    }

    private float GetHorizontalMovementInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    private float GetVerticalMovementInput()
    {
        return Input.GetAxisRaw("Vertical");
    }
}

public enum BlockType
{
    I, J, L, O, S, T, Z
}
