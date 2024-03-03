using UnityEngine;

public class PreviewBlockManager : MonoBehaviour
{
    [SerializeField] private float _previewColorAlpha;
    private GameObject _currentActiveBlock;
    private GameObject _previewBlock;

    private void Update()
    {
        if (_currentActiveBlock != GameManager.Instance.ActiveBlock)
        {
            if (_previewBlock != null)
            {
                Destroy(_previewBlock);
            }

            _currentActiveBlock = GameManager.Instance.ActiveBlock;
            InitPreviewBlock();
            MoveDown();
        }
    }

    private void LateUpdate()
    {
        _previewBlock.transform.SetPositionAndRotation(
            _currentActiveBlock.transform.position,
            _currentActiveBlock.transform.rotation);
        MoveDown();
    }

    private void MoveDown()
    {
        //Calculate next position for every block piece
        Vector3 movementAmount = Vector3.down * GameManager.Instance.MovementAmountPerStep;
        Vector2Int[] blockPieceNextPositions = GetPreviewMoveOperation(movementAmount, _previewBlock.transform);

        while (GameManager.Instance.IsBlockOnBoard(blockPieceNextPositions))
        {
            _previewBlock.transform.position += Vector3.down * GameManager.Instance.MovementAmountPerStep;
            blockPieceNextPositions = GetPreviewMoveOperation(movementAmount, _previewBlock.transform);
        }
    }

    private Vector2Int[] GetPreviewMoveOperation(Vector3 movementAmount, Transform previewBlock)
    {
        Vector2Int[] blockPieceNextPositions = new Vector2Int[BlockMover.BlockCount];
        for (int i = 0; i < previewBlock.childCount; i++)
        {
            Vector2 blockPosition = previewBlock.GetChild(i).transform.position + movementAmount;

            int blockPositionX = Mathf.RoundToInt(blockPosition.x);
            int blockPositionY = Mathf.RoundToInt(blockPosition.y);

            blockPieceNextPositions[i].x = blockPositionX;
            blockPieceNextPositions[i].y = blockPositionY;
        }

        return blockPieceNextPositions;
    }

    private void InitPreviewBlock()
    {
        _previewBlock = Instantiate(
            _currentActiveBlock,
            _currentActiveBlock.transform.position,
            _currentActiveBlock.transform.rotation);

        //When spawn new active block in GameManager
        //For Dotween anim, scale set zero
        //so reset preview block scale to 1
        _previewBlock.transform.localScale = Vector3.one;

        if (_previewBlock.TryGetComponent(out BlockMover blockMover))
        {
            Destroy(blockMover);
        }
        else
        {
            Debug.LogWarning("Preview Block's Block Mover Not Found");
            return;
        }

        //Assign preview color
        foreach (Transform blockPiece in _previewBlock.transform)
        {
            if (blockPiece.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                Color blockPieceColor = spriteRenderer.color;
                blockPieceColor.a = _previewColorAlpha;
                spriteRenderer.color = blockPieceColor;
            }
        }
    }
}
