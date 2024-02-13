using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public MusicScriptableObj[] musicData;
    bool bIsMusicPlaying;
    public AudioSource musicSource;

    public bool bForceStartSong;
    public int songIDToForceStart;


    int songID;
    float currentSongLength;


    void Setup()
    {
        

        if(bIsMusicPlaying)
        {
            ChangeMusic();
        }
        else
        {
            StartMusic();
        }

        
    }

    void Start()
    {
        if(bForceStartSong)
        {
            musicSource.clip = musicData[songIDToForceStart].musicClip;
            musicSource.Play();

        }

        else
        {
            Setup();

        }
    }


    void StartMusic()
    {
        GetNewSongID();
        musicSource.clip = musicData[songID].musicClip;
        musicSource.Play();
        

        bIsMusicPlaying = true;
    }

    void ChangeMusic()
    {

    }

    int GetNewSongID()
    {
        songID = Random.Range(0, musicData.Length);
        
        return songID;
    }
}
