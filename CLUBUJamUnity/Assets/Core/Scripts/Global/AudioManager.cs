using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _backgroundMusic;
    [Range(0f, 1f)] [SerializeField] private float _fixedVolume;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _backgroundMusic.Play();
    }

    public void SetMute(bool toMute)
    {
        if (toMute)
        {
            _backgroundMusic.volume = 0f;
        }
        else
        {
            _backgroundMusic.volume = _fixedVolume;
        }
        
    }
}
