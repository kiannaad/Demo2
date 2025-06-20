using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

#region 枚举量
    public enum AttributeType
    {
        MaxHp,
        CurHp,
        Attack,
        Defense,
        MoveSpeed,
    }
    
    public enum DataModifyType
    {
        BaseValue,           //基础值
        AdditionalValue,        //附加值
        PercentageValue,        //百分比
    }
    
    public enum DataOperateType
    {
        Addition,               //加法，如果是减修改值设置成负数
        Set,                    //设置(只针对当前值)
        Percentage,             //百分比(被施法者)
        PercentageTrigger,      //百分比(施法者)
    }
    
    /// <summary>
    /// 属性来源枚举
    /// </summary>
    public enum AttributeSource
    {
        BaseLevel,        // 基础等级
        Equipment,        // 装备
        PassiveSkill,     // 被动技能
        Buff,             // 增益效果
        TeamBonus,        // 组队加成
        // 其他来源...
    }
    
    public enum AttributeLayerType
    {
        Base,
        Equiptment,
        Buff,
    }
#endregion

#region 结构体
    /// <summary>
    /// 数据修改器结构，支持唯一标识和精确回滚
    /// </summary>
    public struct DataModify
    {
        public string id;                      // 唯一标识符，用于精确回滚
        public AttributeType attributeType;     // 修改哪个属性
        public DataModifyType dateModifyType;   // 修改哪一块数据：当前值(curHp)，附加值，百分比
        public float modify;                    // 修改值
        public DataOperateType operateType;     // 怎么操作：加法，设置，被施法者(我)的百分比，施法者的百分比
        
        /// <summary>
        /// 创建数据修改器
        /// </summary>
        /// <param name="id">唯一标识符</param>
        /// <param name="attributeType">属性类型</param>
        /// <param name="dateModifyType">修改类型</param>
        /// <param name="modify">修改值</param>
        /// <param name="operateType">操作类型</param>
        public DataModify(string id, AttributeType attributeType, DataModifyType dateModifyType, float modify, DataOperateType operateType)
        {
            this.id = id;
            this.attributeType = attributeType;
            this.dateModifyType = dateModifyType;
            this.modify = modify;
            this.operateType = operateType;
        }
    }
    
    public struct AttributeData
    {
        public float _rootValue;         // 根值
        public float _baseValue;         // 基础值
        public float _percentageValue;   // 百分比
        public float _additionalValue;   // 附加值
    
        public AttributeData(float rootValue, float baseValue, float percentageValue, float additionalValue)
        {
            _rootValue = rootValue;
            _baseValue = baseValue;
            _percentageValue = percentageValue;
            _additionalValue = additionalValue;
        }
        
        public AttributeData(float rootValue)
        {
            _rootValue = rootValue;
            _baseValue = 0f;
            _percentageValue = 0f;
            _additionalValue = 0f;
        }
    
        public void Reset()
        {
            _baseValue = 0f;
            _percentageValue = 0f;
            _additionalValue = 0f;
        }
        
        /// <summary>
        /// 计算最终属性值
        /// </summary>
        /// <returns>最终属性值</returns>
        public float CalculateFinalValue()
        {
            // 公式：最终值 = (根值 + 基础值 + 附加值) * (1 + 百分比值)
            return (_rootValue + _baseValue + _additionalValue) * (1f + _percentageValue);
        }
    }
    
    public struct AttributeModifyData
    {
        public AttributeSource attributeSource; // 属性来源
        public AttributeLayerType attributeLayerType; // 属性层级类型
        public DataModify dataModify; // 数据修改
    }
#endregion

/// <summary>
/// 属性系统主类，支持精确的修改器管理和数据回滚
/// </summary>
public class AttributeSystem
{
    #region 数据成员
        private AttributeLayer _baseLayer = new AttributeLayer();
        private AttributeLayer _equiptmentLayer = new AttributeLayer();
        private AttributeLayer _buffLayer = new AttributeLayer();
    
        //字典里面初始化的值是不允许改变的。计算时用的是临时值
        private Dictionary<AttributeType, AttributeData> _attributeDataDict = new Dictionary<AttributeType, AttributeData>();
        //缓存存储计算后的结果，动态修改，是最终值。
        private Dictionary<AttributeType, float> _attributeDataCache = new Dictionary<AttributeType, float>();
        
        // 修改器ID到层级类型的映射，用于精确回滚
        private Dictionary<string, AttributeLayerType> _modifierIdToLayerMap = new Dictionary<string, AttributeLayerType>();
        
        private bool _isDirty = true;
    
        /// <summary>
        /// 标记数据为脏状态，需要重新计算
        /// </summary>
        private void MarkDirty() => _isDirty = true;
    #endregion

    #region 修改或移除修改器
    /// <summary>
    /// 添加属性修改数据
    /// </summary>
    /// <param name="attributeModifyData">属性修改数据</param>
    /// <returns>是否添加成功</returns>
     public bool AddAttributeModifyData(AttributeModifyData attributeModifyData)
     {
         // 检查ID是否已存在
         if (_modifierIdToLayerMap.ContainsKey(attributeModifyData.dataModify.id))
         {
             Debug.LogWarning($"修改器ID已存在: {attributeModifyData.dataModify.id}");
             return false;
         }
         
         AttributeLayer attributeLayer = GetAttributeLayer(attributeModifyData.attributeLayerType);
         if(attributeLayer == null)
         {
             Debug.LogError($"AttributeLayer is null, attributeLayerType: {attributeModifyData.attributeLayerType}");
             return false;
         }
         
         // 添加修改器到对应层级
         attributeLayer.AddModify(attributeModifyData.dataModify);
         
         // 记录ID到层级的映射
         _modifierIdToLayerMap[attributeModifyData.dataModify.id] = attributeModifyData.attributeLayerType;
         
         MarkDirty();
         return true;
     }
 
     /// <summary>
     /// 移除指定ID的修改器
     /// </summary>
     /// <param name="modifierId">修改器ID</param>
     /// <returns>是否移除成功</returns>
     public bool RemoveModifierById(string modifierId)
     {
         if (!_modifierIdToLayerMap.TryGetValue(modifierId, out var layerType))
         {
             Debug.LogWarning($"未找到修改器ID: {modifierId}");
             return false;
         }
         
         AttributeLayer attributeLayer = GetAttributeLayer(layerType);
         if (attributeLayer == null)
         {
             Debug.LogError($"AttributeLayer is null, attributeLayerType: {layerType}");
             return false;
         }
         
         // 从层级中移除修改器
         bool removed = attributeLayer.RemoveModifierById(modifierId);
         if (removed)
         {
             // 从映射中移除
             _modifierIdToLayerMap.Remove(modifierId);
             MarkDirty();
         }
         
         return removed;
     }
 
     /// <summary>
     /// 移除指定层级类型的所有修改器
     /// </summary>
     /// <param name="layerType">层级类型</param>
     /// <returns>移除的修改器数量</returns>
     public int RemoveAllModifiersByLayer(AttributeLayerType layerType)
     {
         AttributeLayer attributeLayer = GetAttributeLayer(layerType);
         if (attributeLayer == null)
         {
             Debug.LogError($"AttributeLayer is null, attributeLayerType: {layerType}");
             return 0;
         }
         
         // 获取该层级的所有修改器ID
         var modifierIds = attributeLayer.GetAllModifierIds();
         
         // 从映射中移除这些ID
         foreach (var id in modifierIds)
         {
             _modifierIdToLayerMap.Remove(id);
         }
         
         // 清空该层级
         int count = attributeLayer.GetTotalModifierCount();
         attributeLayer.Clear();
         
         MarkDirty();
         return count;
     }
   #endregion

   #region 获取数值
    /// <summary>
    /// 获取指定属性类型的最终值（带缓存）
    /// </summary>
    /// <param name="attributeType">属性类型</param>
    /// <returns>最终属性值</returns>
   
     public float GetFinalAttributeValue(AttributeType attributeType)
     {
         UpdateAttributeData();
         
         if (_attributeDataCache.TryGetValue(attributeType, out float cachedValue))
         {
             return cachedValue;
         }
         
         return 0f;
     }
 
     /// <summary>
     /// 获取指定属性类型的详细数据
     /// </summary>
     /// <param name="attributeType">属性类型</param>
     /// <returns>属性数据</returns>
     public AttributeData GetAttributeData(AttributeType attributeType)
     {
         UpdateAttributeData();
         
         if (_attributeDataDict.TryGetValue(attributeType, out var data))
         {
             return data;
         }
         
         return new AttributeData();
     }
   #endregion

   #region 初始化属性
    /// <summary>
    /// 设置基础属性值
    /// </summary>
    /// <param name="attributeType">属性类型</param>
    /// <param name="rootValue">根值</param>
    /// <param name="baseValue">基础值</param>
     public void SetBaseAttributeValue(AttributeType attributeType, float rootValue)
     {
         if (!_attributeDataDict.ContainsKey(attributeType))
         {
             _attributeDataDict[attributeType] = new AttributeData(rootValue);
         }
         
         
         MarkDirty();
     }
   #endregion

    #region 拆分功能
        /// <summary>
        /// 获取属性层级
        /// </summary>
        /// <param name="attributeLayerType">层级类型</param>
        /// <returns>属性层级</returns>
        private AttributeLayer GetAttributeLayer(AttributeLayerType attributeLayerType)
        {
            switch(attributeLayerType)
            {
                case AttributeLayerType.Base:
                    return _baseLayer;
                case AttributeLayerType.Equiptment:
                    return _equiptmentLayer;
                case AttributeLayerType.Buff:
                    return _buffLayer;
                default:
                    return null;
            }
        }
    
        /// <summary>
        /// 更新属性数据并计算缓存
        /// </summary>
        private void UpdateAttributeData()
        {
            if(!_isDirty) return;
    
            // 清空缓存
            _attributeDataCache.Clear();
            
            // 为每个属性类型计算最终值
            foreach(var kvp in _attributeDataDict)
            {
                var attributeType = kvp.Key;
                var attributeData = kvp.Value;
                AttributeData _attributeData = new AttributeData(attributeData._rootValue);

                // 按层级顺序应用修改器
                CalculateLayerAttributeData(ref _attributeData, AttributeLayerType.Base);
                CalculateLayerAttributeData(ref _attributeData, AttributeLayerType.Equiptment);
                CalculateLayerAttributeData(ref _attributeData, AttributeLayerType.Buff);
                
                // 计算并缓存最终值
                _attributeDataCache[attributeType] = _attributeData._rootValue;
            }
            
            _isDirty = false;
        }

        /// <summary>
        /// 每层修改器作用后，计算最终值，然后清空修改器的影响，保证每层的计算互不影响。（修改器无法修改根值）
        /// </summary>
        /// <param name="attributeData">属性数据</param>
        /// <param name="layerType">层级类型</param>
        private void CalculateLayerAttributeData(ref AttributeData attributeData, AttributeLayerType layerType)
        {
            ApplyLayerModifiers(ref attributeData, layerType);
            attributeData._rootValue = attributeData.CalculateFinalValue();
            attributeData.Reset();
        }
    
        /// <summary>
        /// 应用指定层级的所有修改器
        /// </summary>
        /// <param name="attributeData">属性数据</param>
        /// <param name="layerType">层级类型</param>
        private void ApplyLayerModifiers(ref AttributeData attributeData, AttributeLayerType layerType)
        {
            AttributeLayer layer = GetAttributeLayer(layerType);
            if (layer != null)
            {
                // 对每个属性类型应用修改器
                foreach (var attributeType in System.Enum.GetValues(typeof(AttributeType)))
                {
                    layer.CalculateAttributeData((AttributeType)attributeType, ref attributeData);
                }
            }
        }
    
        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void Clear()
        {
            _baseLayer.Clear();
            _equiptmentLayer.Clear();
            _buffLayer.Clear();
            _attributeDataDict.Clear();
            _attributeDataCache.Clear();
            _modifierIdToLayerMap.Clear();
            _isDirty = true;
        }
    
        /// <summary>
        /// 获取系统状态信息
        /// </summary>
        /// <returns>状态信息字符串</returns>
        public string GetSystemStatus()
        {
            return $"Base Layer: {_baseLayer.GetTotalModifierCount()}, " +
                   $"Equipment Layer: {_equiptmentLayer.GetTotalModifierCount()}, " +
                   $"Buff Layer: {_buffLayer.GetTotalModifierCount()}, " +
                   $"Total Modifiers: {_modifierIdToLayerMap.Count}, " +
                   $"Cached Attributes: {_attributeDataCache.Count}";
        }
    #endregion
}
