using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementData
{
   public MoveData moveData;
   public MoveData rushData;
   
   [Header("跳跃和下落参数设置")]
   public float RunJumpRootOffset;
   public float QuickRunJumpRootOffset;
   public float JumpHightRootOffset;
   
   public float FallSpeedRootOffset;
}
