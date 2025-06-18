using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboInfo", menuName = "ComboInfo")]
public class ComboInfo : ScriptableObject
{
    public List<WeaponComboData> _ComboData;
    public HeavyAttackData _HeavyAttackData;
    public SkillData _skillData;
}
