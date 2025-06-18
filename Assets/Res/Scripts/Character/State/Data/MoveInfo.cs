using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveInfo",  menuName = "Create MoveInfo")]
public class MoveInfo : ScriptableObject
{
   public LocomotionData _locomotionData;
   public MovementData _movementData;
}
