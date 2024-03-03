using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    public static event Action<int> OnSpawn;

    [SerializeField] private BlockMover[] _blocks;

    private int _nextBlockIndex;

    public GameObject SpawnBlock()
    {
        GameObject spawnedBlock = Instantiate(
            _blocks[_nextBlockIndex].gameObject,
            transform.position,
            Quaternion.identity);

        _nextBlockIndex = Random.Range(0, _blocks.Length);

        OnSpawn?.Invoke(_nextBlockIndex);

        return spawnedBlock;
    }

    private void Awake()
    {
        _nextBlockIndex = Random.Range(0, _blocks.Length);
    }
}
