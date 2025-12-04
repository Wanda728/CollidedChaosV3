using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Source")]
    public AudioSource musicSource;

    [Header("Playlist")]
    public List<AudioClip> songs = new List<AudioClip>();

    private int currentSongIndex = 0;

    private void Awake()
    {
        // Singleton so it persists across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (songs.Count > 0)
        {
            PlaySong(currentSongIndex);
        }
    }

    private void Update()
    {
        // When the current song finishes, play the next one
        if (!musicSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    public void PlaySong(int index)
    {
        if (songs.Count == 0) return;
        if (index < 0 || index >= songs.Count) return;

        currentSongIndex = index;
        musicSource.clip = songs[currentSongIndex];
        musicSource.Play();
    }

    public void PlayNextSong()
    {
        if (songs.Count == 0) return;

        currentSongIndex++;

        if (currentSongIndex >= songs.Count)
        {
            currentSongIndex = 0; // loop back to start
        }

        PlaySong(currentSongIndex);
    }
}