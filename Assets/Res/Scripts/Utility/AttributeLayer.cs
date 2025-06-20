using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 属性层级管理器，负责管理不同来源的属性修改器
/// </summary>
public class AttributeLayer 
{
    private Dictionary<AttributeType, List<DataModify>> _attributeDataDict = new Dictionary<AttributeType, List<DataModify>>();
    
    /// <summary>
    /// 添加属性修改器
    /// </summary>
    /// <param name="dataModify">要添加的修改器</param>
    public void AddModify(DataModify dataModify)
    {
        if (_attributeDataDict.TryGetValue(dataModify.attributeType, out var modifiers))
        {
            modifiers.Add(dataModify);
        }
        else
        {
            _attributeDataDict.Add(dataModify.attributeType, new List<DataModify> { dataModify });
        }
    }

    /// <summary>
    /// 移除指定属性类型的所有修改器
    /// </summary>
    /// <param name="attributeType">要移除的属性类型</param>
    /// <returns>是否成功移除了修改器</returns>
    public bool RemoveModify(AttributeType attributeType)
    {
        return _attributeDataDict.Remove(attributeType);
    }

    /// <summary>
    /// 根据ID移除指定的修改器
    /// </summary>
    /// <param name="modifierId">修改器ID</param>
    /// <returns>是否成功移除</returns>
    public bool RemoveModifierById(string modifierId)
    {
        foreach (var kvp in _attributeDataDict)
        {
            var modifiers = kvp.Value;
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                if (modifiers[i].id == modifierId)
                {
                    modifiers.RemoveAt(i);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 获取所有修改器的ID列表
    /// </summary>
    /// <returns>所有修改器ID的列表</returns>
    public List<string> GetAllModifierIds()
    {
        var ids = new List<string>();
        foreach (var modifiers in _attributeDataDict.Values)
        {
            foreach (var modifier in modifiers)
            {
                ids.Add(modifier.id);
            }
        }
        return ids;
    }
    
    /// <summary>
    /// 清空所有属性修改器
    /// </summary>
    public void Clear()
    {
        _attributeDataDict.Clear();
    }

    /// <summary>
    /// 检查是否存在指定属性类型的修改器
    /// </summary>
    /// <param name="attributeType">要检查的属性类型</param>
    /// <returns>是否存在修改器</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasModifiers(AttributeType attributeType)
    {
        return _attributeDataDict.ContainsKey(attributeType);
    }

    /// <summary>
    /// 获取指定属性类型的修改器数量
    /// </summary>
    /// <param name="attributeType">属性类型</param>
    /// <returns>修改器数量</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetModifierCount(AttributeType attributeType)
    {
        return _attributeDataDict.TryGetValue(attributeType, out var modifiers) ? modifiers.Count : 0;
    }

    /// <summary>
    /// 获取所有修改器的总数量
    /// </summary>
    /// <returns>总修改器数量</returns>
    public int GetTotalModifierCount()
    {
        int count = 0;
        foreach (var modifiers in _attributeDataDict.Values)
        {
            count += modifiers.Count;
        }
        return count;
    }

    /// <summary>
    /// 计算指定属性类型的最终属性数据
    /// 优雅地应用所有修改器并返回计算结果
    /// </summary>
    /// <param name="attributeType">要计算的属性类型</param>
    /// <param name="attributeData">基础属性数据（引用传递，会被修改）</param>
    /// <returns>计算后的属性数据</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AttributeData CalculateAttributeData(AttributeType attributeType, ref AttributeData attributeData)
    {
        // 快速检查是否存在该属性类型的修改器
        if (!_attributeDataDict.TryGetValue(attributeType, out var modifiers))
        {
            return attributeData; // 没有修改器，直接返回原数据
        }

        // 应用所有修改器
        foreach (var modifier in modifiers)
        {
            ApplyModifier(ref attributeData, modifier);
        }
        
        return attributeData;
    }

    /// <summary>
    /// 应用单个修改器到属性数据
    /// </summary>
    /// <param name="attributeData">要修改的属性数据</param>
    /// <param name="modifier">要应用的修改器</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ApplyModifier(ref AttributeData attributeData, DataModify modifier)
    {
        // 获取要修改的属性字段的引用
        ref float targetField = ref GetAttributeField(ref attributeData, modifier.dateModifyType);
        
        // 应用操作
        ApplyOperation(ref targetField, modifier);
    }

    /// <summary>
    /// 根据修改类型获取对应的属性字段引用
    /// </summary>
    /// <param name="attributeData">属性数据</param>
    /// <param name="dataModifyType">修改类型</param>
    /// <returns>对应字段的引用</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ref float GetAttributeField(ref AttributeData attributeData, DataModifyType dataModifyType)
    {
        switch (dataModifyType)
        {
            case DataModifyType.BaseValue:
                return ref attributeData._baseValue;
            case DataModifyType.AdditionalValue:
                return ref attributeData._additionalValue;
            case DataModifyType.PercentageValue:
                return ref attributeData._percentageValue;
            default:
                return ref attributeData._baseValue; // 默认返回基础值
        }
    }

    /// <summary>
    /// 应用操作到目标字段
    /// </summary>
    /// <param name="targetField">目标字段的引用</param>
    /// <param name="modifier">修改器</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ApplyOperation(ref float targetField, DataModify modifier)
    {
        switch (modifier.operateType)
        {
            case DataOperateType.Addition:
                targetField += modifier.modify;
                break;
            case DataOperateType.Set:
                targetField = modifier.modify;
                break;
            case DataOperateType.Percentage:
                targetField *= modifier.modify;
                break;
            case DataOperateType.PercentageTrigger:
                // 百分比触发类型，可以根据需要扩展
                targetField *= modifier.modify;
                break;
        }
    }
}
