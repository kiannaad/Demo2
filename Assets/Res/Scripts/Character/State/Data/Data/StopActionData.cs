using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


[Serializable]
public class StopActionData
{
    public TimelineAsset _MoveStop;
    
    public TimelineAsset _RushStop;

    public TimelineAsset _CrouchMoveStop;
    public TimelineAsset _CrouchRunStop;

    public TimelineAsset _Land;
    public TimelineAsset _AirAttackDownLand;
}
