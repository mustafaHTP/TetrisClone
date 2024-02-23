using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NextBlockDisplayer : MonoBehaviour
{
    [SerializeField] private Sprite[] _blockSprites;
    [SerializeField] private Image _nextBlockImage;

    private void OnEnable()
    {
        BlockSpawner.OnSpawn += BlockSpawner_OnSpawn;
    }

    private void OnDisable()
    {
        BlockSpawner.OnSpawn -= BlockSpawner_OnSpawn;
    }

    private void BlockSpawner_OnSpawn(int blockIndex)
    {
        _nextBlockImage.GetComponent<Transform>().localScale = Vector3.zero;
        _nextBlockImage.sprite = _blockSprites[blockIndex];
        _nextBlockImage.GetComponent<Transform>().DOScale(1f, 0.8f);
    }
}
