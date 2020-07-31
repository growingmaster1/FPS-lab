using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAudio1 : RoomAudio
{
    protected override bool Canplay(int audioIndex)
    {
        return base.Canplay(audioIndex);
    }

    protected override void PlayAudio(int audioIndex)
    {
        base.PlayAudio(audioIndex);
    }
}
