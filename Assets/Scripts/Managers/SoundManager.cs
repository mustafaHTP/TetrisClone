using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("SFX")]
    [Space(5)]
    [SerializeField] private AudioSource _blockHorizontalMovementAS;
    [SerializeField] private AudioSource _blockRotationAS;
    [SerializeField] private AudioSource _blockMovementErrorClipAS;
    [SerializeField] private AudioSource _blockLandSuccessAS;
    [SerializeField] private AudioSource _rowCleaningAS;

    [Header("Music")]
    [Space(5)]
    [SerializeField] private List<AudioSource> _backgroundMusic;

    private AudioSource _currentBackgroundMusicAS;

    public void PlayBlockHorizontalMovement()
    {
        if (!_blockHorizontalMovementAS.isPlaying)
        {
            _blockHorizontalMovementAS.Play();
        }
    }

    public void PlayBlockRotation()
    {
        if (!_blockRotationAS.isPlaying)
        {
            _blockRotationAS.Play();
        }
    }

    public void PlayBlockMoveError()
    {
        if (!_blockMovementErrorClipAS.isPlaying)
        {
            _blockMovementErrorClipAS.Play();
        }
    }

    public void PlayBlockLandSuccess()
    {
        if (!_blockLandSuccessAS.isPlaying)
        {
            _blockLandSuccessAS.Play();
        }
    }

    public void PlayRowCleaning()
    {
        if (!_rowCleaningAS.isPlaying)
        {
            _rowCleaningAS.Play();
        }
    }

    private IEnumerator PlayRandomBackgroundMusic()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, _backgroundMusic.Count);
            _backgroundMusic[randomIndex].Play();

            yield return new WaitForSeconds(_backgroundMusic[randomIndex].clip.length);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(PlayRandomBackgroundMusic());
    }
}
