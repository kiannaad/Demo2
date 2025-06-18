using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class MoveData
{
    public float TargetSpeed;
    public float SpeedSmoothTime;
    
    public float RotateSmoothTime;
    public float RotateSpeed;
}
