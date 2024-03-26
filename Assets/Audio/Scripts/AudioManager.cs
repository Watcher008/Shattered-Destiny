using UnityEngine;
using SD.Primitives;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private FloatReference _masterVolumeRef;
    [SerializeField] private FloatReference _musicVolumeRef;
    [SerializeField] private FloatReference _sFXVolumeRef;

    private AudioSource _themeSource;
    private AudioSource _sFXSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        _themeSource = gameObject.AddComponent<AudioSource>();
        _themeSource.loop = true;

        _sFXSource = gameObject.AddComponent<AudioSource>();
        _sFXSource.loop = false;

        // Set to default values at start
        // This must be done after setting audio sources
        // Updating their values references them below
        _masterVolumeRef.Value = 1.0f;
        _musicVolumeRef.Value = 1.0f;
        _sFXVolumeRef.Value = 1.0f;
    }

    public static void SetTheme(AudioClip clip)
    {
        if (clip == null) return;

        instance._themeSource.clip = clip;
        instance._themeSource.Play();
    }

    public static void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        instance._sFXSource.PlayOneShot(clip);
    }

    // The following three methods are called by GameEventListeners on the AudioManager object.
    // The events are invoked by the master/music/sfx volume SOs when their values are changed.
    public void UpdateMasterVolume()
    {
        UpdateMusicVolume();
        UpdateSFXVolume();
    }

    public void UpdateMusicVolume()
    {
        _themeSource.volume = _masterVolumeRef.Value * _musicVolumeRef.Value;
    }

    public void UpdateSFXVolume()
    {
        _sFXSource.volume = _masterVolumeRef.Value * _sFXVolumeRef.Value;
    }
}
