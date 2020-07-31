using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAudio : MonoBehaviour
{

    private  bool lastOver;
    public GameObject nextRoom;
    public GameObject lastRoom;
    public AudioClip[] roomAudios;

    [HideInInspector]
    protected AudioSource player;

    [HideInInspector]
    protected bool[] havePlayed;
    // Start is called before the first frame update
    void Start()
    {
        lastOver = gameObject.GetComponent<RoomManager>().lastOver;
        havePlayed = new bool[roomAudios.Length];
        for(int i=0;i<roomAudios .Length;++i)
        {
            havePlayed[i] = false;
        }
        player = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roomAudios.Length != 0 )  
        {
            for (int i = 0; i < roomAudios.Length; i++) 
            {
                if(Canplay (i))
                {
                    player.clip = roomAudios[i];
                    PlayAudio(i);
                    havePlayed[i] = true;
                }
            }
        }
        if (nextRoom != null)
        {
            if (!player.isPlaying)
            {
                nextRoom.GetComponent<RoomManager>().lastOver = true;
            }
            else
            {
                nextRoom.GetComponent<RoomManager>().lastOver = false;
            }
        }
    }

    //根据录音不同决定不同的判定
    protected virtual bool Canplay(int audioIndex)
    {
        if(lastOver)
            if (!player.isPlaying && (!havePlayed[audioIndex])) 
                return true;
        return false;
    }

    //根据录音不同决定不同播放方式
    protected virtual void PlayAudio(int audioIndex)
    {
        player.Play();
    }
}
