using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;


[Serializable]
public class ChangeStateEventArg : GameEventArgs
{
    public string targetStateName;
    [HideInInspector] public bool isEnd;
    public TimelineAsset _targetTimeline;
    
    public static readonly int EventId = typeof(ChangeStateEventArg).GetHashCode();

    public static ChangeStateEventArg Create(string targetStateName, TimelineAsset targetTimeline)
    {
        var e = ReferencePool.Acquire<ChangeStateEventArg>();
        e.targetStateName = targetStateName;
        e._targetTimeline = targetTimeline;
        return e;
    }
    
    public override void Clear()
    {
        targetStateName = null;
        isEnd = false;
        _targetTimeline = null;
    }

    public override int Id => EventId;
   
}

public class ConditionalTrackAsset : PlayableAsset
{
    
    public ChangeStateEventArg changeStateEventArg;
    public bool _needTriggerInterrupt;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playableScript = ScriptPlayable<ConditionalTrackBehaviour>.Create(graph);
        var playableBehaviour = playableScript.GetBehaviour();
        
        playableBehaviour.Init(changeStateEventArg, _needTriggerInterrupt);
        
        return playableScript;
    }
}
