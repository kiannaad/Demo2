using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[Serializable]
public class LoopActionData
{
    public TimelineAsset _IdleTimeline;
    public TimelineAsset MoveMixerTimeline;
    public TimelineAsset _JumpTimeline;
    public TimelineAsset _FallTimeline;
    public TimelineAsset _CrouchMoveTimeline;
    public TimelineAsset _CrouchIdleTimeline;
    public TimelineAsset _CrouchRunTimeline;
    public TimelineAsset _AirAttackDownTimeline;
}
