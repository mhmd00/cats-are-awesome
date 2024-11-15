using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [System.Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
        public float pitch;
    }

    private AudioSource musicSource;
    private AudioSource soundEffectsSource;

    public Sound[] musicTracks;
    public Sound[] soundEffects;

    private const float DEFAULT_PITCH = 1.0f;

    private float musicVolume;
    private float soundEffectsVolume;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SOUND_EFFECTS_VOLUME_KEY = "SoundEffectsVolume";

    private void Awake()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1.0f);
        soundEffectsVolume = PlayerPrefs.GetFloat(SOUND_EFFECTS_VOLUME_KEY, 1.0f);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
        musicSource.pitch = DEFAULT_PITCH;

        soundEffectsSource = gameObject.AddComponent<AudioSource>();
        soundEffectsSource.playOnAwake = false;
        soundEffectsSource.volume = soundEffectsVolume;
        soundEffectsSource.pitch = DEFAULT_PITCH;
    }

    /// <summary>
    /// Plays the specified music track.
    /// </summary>
    /// <param name="soundName">The name of the music track to play.</param>
    public void PlayMusic(string soundName)
    {
        musicSource.Stop();

        foreach (var track in musicTracks)
        {
            if (track.name == soundName)
            {
                musicSource.clip = track.clip;
                musicSource.volume = musicVolume;
                musicSource.pitch = track.pitch != 0 ? track.pitch : DEFAULT_PITCH;
                musicSource.Play();
                break;
            }
        }
    }

    /// <summary>
    /// Plays the specified sound effect.
    /// </summary>
    /// <param name="soundName">The name of the sound effect to play.</param>
    public void PlaySound(string soundName)
    {
        foreach (var sound in soundEffects)
        {
            if (sound.name == soundName)
            {
                soundEffectsSource.PlayOneShot(sound.clip, soundEffectsVolume);
                break;
            }
        }
    }

    /// <summary>
    /// Sets the volume of the music.
    /// </summary>
    /// <param name="volume">The volume level to set (between 0 and 1).</param>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Sets the volume of the sound effects.
    /// </summary>
    /// <param name="volume">The volume level to set (between 0 and 1).</param>
    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = Mathf.Clamp01(volume);
        soundEffectsSource.volume = soundEffectsVolume;

        PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME_KEY, soundEffectsVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Gets the current volume level of the music.
    /// </summary>
    /// <returns>The current music volume (between 0 and 1).</returns>
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    /// <summary>
    /// Gets the current volume level of the sound effects.
    /// </summary>
    /// <returns>The current sound effects volume (between 0 and 1).</returns>
    public float GetSoundEffectsVolume()
    {
        return soundEffectsVolume;
    }
}
