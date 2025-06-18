using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;



[System.Serializable]
public struct WeaponComboData
{
    public string _name;
    public float _damage;
    public TimelineAsset _Timeline;
}

/*[System.Serializable]
public struct WeaponExpecialCombo
{
    public string _name;
    public float _damage;
    public TimelineAsset _Timeline;
}*/

[Serializable]
public class WeaponData
{
    public WeaponType _WeaponType;
    public GameObject _WeaponPrefab;
    public MoveInfo _MoveInfo;
    public ComboInfo _ComboInfo;
    public TimelineAsset _EquipTimeline;
    public TimelineAsset _UnArmedTimeline;
    public Vector3 _LocalPosition;
    public Vector3 _LocalRotation;
    public Vector3 _LocalScale;
}
