using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


[Serializable]
public class StartActionData
{
    [Space] public TimelineAsset _StandToCrouch;
    public TimelineAsset _CrouchToStand;

    [Space] public TimelineAsset _DodgeBack;
    public TimelineAsset _DodgeRoll;

    [Space] public TimelineAsset _CrouchMoveStart;

    [Space] public TimelineAsset _AirAttackDownStart;

}
