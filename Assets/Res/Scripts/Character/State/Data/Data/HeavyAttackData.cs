using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public class HeavyAttackData
{
    public TimelineAsset HeavyAttack_Charge;
    public TimelineAsset HeavyAttack;
    public TimelineAsset HeavyAttack_End;
    public float damage;
    public float FirstChargeTimePoint;
    public float SecondChargeTimePoint;
    public float FirstDamageAdd;
    public float SecondDamageAdd;
}
