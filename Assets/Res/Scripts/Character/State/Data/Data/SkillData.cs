using System;
using System.Collections;
using System.Collections.Generic;
using Animancer.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public enum SkillType
{
    Charge,
    Instant,
}

[System.Serializable]
public class SkillData
{
    public SkillType _skillType;


    public TimelineAsset _ToDefenseTimeline;
    public TimelineAsset _ExitDefenseTimeline;
    public TimelineAsset _DefenseWalkFTimeline;
    public TimelineAsset _DefenseAttackTimeline;
    public TimelineAsset _DefenseIdleTimeline;
    
    
    public TimelineAsset _InstantTimeline;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SkillData))]
public class SkillDataDrawer : PropertyDrawer
{
    private SerializedProperty _SkillTypePro;
    
    private SerializedProperty _ToDefenseTimeline;
    private SerializedProperty _ExitDefenseTimeline;
    private SerializedProperty _DefenseWalkFTimeline;
    private SerializedProperty _DefenseAttackTimeline;
    private SerializedProperty _DefenseIdleTimeline;
    
    private SerializedProperty _InstantTimelinePro;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var skillType = property.FindPropertyRelative("_skillType");
        if (skillType.enumValueIndex == (int)SkillType.Charge)
            return EditorGUIUtility.singleLineHeight * 7;
        else
            return EditorGUIUtility.singleLineHeight * 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.isExpanded, label);

        if (!property.isExpanded)
            return;

        FindProperty(property);

        EditorGUI.BeginProperty(position, label, property);
        
        if (_SkillTypePro == null)
        {
            Debug.LogError("SkillDataDrawer is missing SkillTypePro");
            EditorGUI.EndProperty();
            return;
        }
        
        Rect typeRect = new Rect(position.x, position.y + 20f, position.width, EditorGUIUtility.singleLineHeight);
        
        EditorGUI.PropertyField(typeRect, _SkillTypePro, new GUIContent("技能类型"));

        switch ((SkillType)_SkillTypePro.enumValueIndex)
        {
            case SkillType.Charge : DrawChargeTimelinePro(position,  position.y + 40f);
                break;
            case SkillType.Instant : DrawInstantTimelinePro(position,  position.y + 40f);
                break;
                
        }
        
        EditorGUI.EndProperty();
    }

    private void FindProperty(SerializedProperty property)
    {
        _SkillTypePro = property.FindPropertyRelative("_skillType");
        _ToDefenseTimeline = property.FindPropertyRelative("_ToDefenseTimeline");
        _ExitDefenseTimeline = property.FindPropertyRelative("_ExitDefenseTimeline");
        _DefenseWalkFTimeline = property.FindPropertyRelative("_DefenseWalkFTimeline");
        _DefenseAttackTimeline = property.FindPropertyRelative("_DefenseAttackTimeline");
        _DefenseIdleTimeline = property.FindPropertyRelative("_DefenseIdleTimeline");
        _InstantTimelinePro = property.FindPropertyRelative("_InstantTimeline");
    }

    private void DrawChargeTimelinePro(Rect position, float y)
    {
        Rect chargeProRect1 = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
        Rect chargeProRect2 = new Rect(position.x, y + 20f, position.width, EditorGUIUtility.singleLineHeight);
        Rect chargeProRect3 = new Rect(position.x, y + 40f, position.width, EditorGUIUtility.singleLineHeight);
        Rect chargeProRect4 = new Rect(position.x, y + 60f, position.width, EditorGUIUtility.singleLineHeight);
        Rect chargeProRect5 = new Rect(position.x, y + 80f, position.width, EditorGUIUtility.singleLineHeight);
        
        EditorGUI.PropertyField(chargeProRect1, _ToDefenseTimeline, new GUIContent("转换成架势状态"));
        EditorGUI.PropertyField(chargeProRect2, _ExitDefenseTimeline, new GUIContent("退出架势状态"));
        EditorGUI.PropertyField(chargeProRect3, _DefenseWalkFTimeline, new GUIContent("架势移动姿态"));
        EditorGUI.PropertyField(chargeProRect4, _DefenseAttackTimeline, new GUIContent("架势反击姿态"));
        EditorGUI.PropertyField(chargeProRect5, _DefenseIdleTimeline, new GUIContent("架势空闲姿态"));
    }

    private void DrawInstantTimelinePro(Rect position, float y)
    {
        Rect InstantRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
        
        EditorGUI.PropertyField(InstantRect, _InstantTimelinePro, new GUIContent("瞬发Timeline"));
    }
}

#endif
