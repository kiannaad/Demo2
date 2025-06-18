using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public class LocomotionData
{
    public StartActionData _startActionData;
    public StopActionData _stopActionData;
    public LoopActionData _loopActionData;
    public List<TimelineAsset>  _MoveSearchTimeline;
    public List<TimelineAsset> _DodgeRollSearchTimeline;
    
}
