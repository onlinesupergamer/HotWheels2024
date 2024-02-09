using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Create Music Data", menuName = "Music Data")]
public class MusicScriptableObj : ScriptableObject
{
    public string songName;
    public string artist;
    public Texture art;
    public AudioClip musicClip;

}
