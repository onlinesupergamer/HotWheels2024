using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    public MusicScriptableObj[] musicData;
    public AudioSource musicSource;
    bool bHasTriggered = false;
    public int i = 2;


    int ChangeSongID()
    {
        int song = Random.Range(0,musicData.Length + 1);
        i = song;
        print("Change");
        return song;
    }
   

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.root.tag == "Player")
            

            if(bHasTriggered == true)
            return;
        

        



        if(musicSource == null)
        {
            FileLog.CrashErrorHandler("Music Entry is NULL", false);
            print("Music Not Found");
            
        }

        
        

        if(!musicSource.isPlaying)
        {
            ChangeSongID();
            
            musicSource.clip = musicData[i].musicClip;
            musicSource.Play();
            bHasTriggered = true;
        }
     
    }
  

}
