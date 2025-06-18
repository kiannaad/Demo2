using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AniTimeLineSO", menuName = "AniTimeLineSO")]
public class AniDataSO : ScriptableObject
{
    [SerializeField] public WeaponsData _weaponsData;
}
