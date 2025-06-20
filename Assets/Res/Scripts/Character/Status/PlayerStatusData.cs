using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "PlayerStatusData")]
public class PlayerStatusData : ScriptableObject
{
   public float MaxHp;
   public float Attack;
   public float Defense;
}
