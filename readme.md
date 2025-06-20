# OpenWorld 项目文档

## 项目概述
这是一个基于Unity开发的动作类游戏项目，使用现代Unity技术栈实现复杂的角色控制系统和AI行为系统。

## 技术栈
- Unity 2022.3 LTS
- Behavior Designer (行为树插件)
- Cinemachine (相机系统)
- Animancer (动画系统)
- New Input System (输入系统)

## 项目结构

### 1. 角色系统 (Assets/Res/Scripts/Character/)
```
Character/
├── PlayerState.cs          # 玩家状态管理
├── State/
│   ├── BaseState.cs       # 状态基类
│   ├── CameraInternalMan.cs # 相机管理
│   └── [其他状态类]        # 具体状态实现
```

#### 核心功能
- 状态机管理（FSM）
- 相机系统（Cinemachine）
- 输入系统（New Input System）
- 动画系统（Animancer）

### 2. 敌人AI系统 (Assets/Res/Scripts/Enemy/)
```
Enemy/
├── Custom Task/           # 自定义行为树节点
│   ├── Composite/        # 组合节点
│   │   └── WeightSelector.cs # 权重选择器
│   └── [其他节点类型]     # 其他行为树节点
```

#### 核心功能
- 行为树系统（Behavior Designer）
- 自定义节点扩展
- 权重选择系统

### 3. 属性系统 (Assets/Res/Scripts/Utility/)
```
Utility/
├── AttributeSystem.cs     # 属性系统主类
├── AttributeLayer.cs      # 属性层级管理器
└── [其他工具类]           # 其他工具类
```

#### 核心功能
- **属性数据管理**：管理角色的各种属性（生命值、攻击力、防御力等）
- **层级化修改器**：支持不同来源的属性修改（装备、技能、buff等）
- **高性能计算**：使用引用传递和内联优化，确保计算效率

#### 属性系统详细说明

##### AttributeLayer 类
`AttributeLayer` 是属性系统的核心组件，负责管理特定来源的属性修改器。

**主要功能：**
- `AddModify(DataModify dataModify)` - 添加属性修改器
- `RemoveModify(AttributeType attributeType)` - 移除指定类型的修改器（返回是否成功）
- `RemoveModifierById(string modifierId)` - 根据ID精确移除修改器
- `GetAllModifierIds()` - 获取所有修改器ID列表
- `Clear()` - 清空所有修改器
- `CalculateAttributeData(AttributeType, ref AttributeData)` - 计算最终属性值
- `HasModifiers(AttributeType attributeType)` - 检查是否存在指定类型的修改器
- `GetModifierCount(AttributeType attributeType)` - 获取指定类型的修改器数量
- `GetTotalModifierCount()` - 获取所有修改器的总数量

**优化特性：**
1. **高性能设计**：
   - 使用 `TryGetValue` 替代 `ContainsKey` + 索引访问
   - 添加 `[MethodImpl(MethodImplOptions.AggressiveInlining)]` 内联优化
   - 使用引用传递避免不必要的值拷贝

2. **精确的数据回滚**：
   - 每个修改器都有唯一ID，支持精确移除
   - 支持按层级批量移除修改器
   - 维护ID到层级的映射关系

3. **优雅的代码结构**：
   - 将复杂的计算逻辑拆分为多个小方法
   - 使用清晰的命名和详细的注释
   - 支持所有操作类型（加法、设置、百分比等）

4. **类型安全**：
   - 使用枚举确保类型安全
   - 支持扩展新的属性类型和操作类型

##### AttributeSystem 类
`AttributeSystem` 是属性系统的主控制器，管理多个属性层级和缓存机制。

**核心功能：**
- **精确的修改器管理**：
  - `AddAttributeModifyData()` - 添加修改器（带ID检查）
  - `RemoveModifierById()` - 根据ID精确移除修改器
  - `RemoveAllModifiersByLayer()` - 移除指定层级的所有修改器

- **智能缓存系统**：
  - 自动缓存计算结果，避免重复计算
  - 脏标记机制，只在数据变化时重新计算
  - `GetFinalAttributeValue()` - 获取缓存的最终属性值

- **数据回滚支持**：
  - 支持单个修改器的精确回滚
  - 支持按层级批量回滚
  - 维护完整的修改器ID映射关系

**使用方法：**
```csharp
// 创建属性系统
var attributeSystem = new AttributeSystem();

// 设置基础属性值
attributeSystem.SetBaseAttributeValue(AttributeType.Attack, 100f, 0f);

// 添加装备修改器
var weaponModifier = new DataModify(
    "weapon_sword_001",           // 唯一ID
    AttributeType.Attack,
    DataModifyType.AdditionalValue,
    50f,
    DataOperateType.Addition
);

var modifyData = new AttributeModifyData
{
    attributeSource = AttributeSource.Equipment,
    attributeLayerType = AttributeLayerType.Equiptment,
    dataModify = weaponModifier
};

attributeSystem.AddAttributeModifyData(modifyData);

// 获取最终属性值（带缓存）
float finalAttack = attributeSystem.GetFinalAttributeValue(AttributeType.Attack);

// 精确回滚 - 移除特定修改器
attributeSystem.RemoveModifierById("weapon_sword_001");

// 批量回滚 - 移除所有装备修改器
int removedCount = attributeSystem.RemoveAllModifiersByLayer(AttributeLayerType.Equiptment);
```

##### 数据回滚功能详解

**1. 精确回滚**
```csharp
// 每个修改器都有唯一ID
var modifier = new DataModify("unique_id_123", ...);

// 添加修改器
attributeSystem.AddAttributeModifyData(modifyData);

// 精确移除
bool removed = attributeSystem.RemoveModifierById("unique_id_123");
```

**2. 层级回滚**
```csharp
// 移除指定层级的所有修改器
int count = attributeSystem.RemoveAllModifiersByLayer(AttributeLayerType.Buff);
Debug.Log($"移除了 {count} 个Buff修改器");
```

**3. 缓存机制**
```csharp
// 第一次获取会计算并缓存
float value1 = attributeSystem.GetFinalAttributeValue(AttributeType.Attack);

// 第二次获取直接从缓存读取（如果数据未变化）
float value2 = attributeSystem.GetFinalAttributeValue(AttributeType.Attack);
```

##### 属性数据结构
```csharp
public struct AttributeData
{
    public float _rootValue;         // 根值（基础属性）
    public float _baseValue;         // 基础值
    public float _percentageValue;   // 百分比加成
    public float _additionalValue;   // 附加值
    
    // 计算最终值：最终值 = (根值 + 基础值 + 附加值) * (1 + 百分比值)
    public float CalculateFinalValue();
}
```

##### 修改器类型
```csharp
public enum DataModifyType
{
    BaseValue,           // 基础值
    AdditionalValue,     // 附加值
    PercentageValue,     // 百分比
}

public enum DataOperateType
{
    Addition,            // 加法
    Set,                 // 设置
    Percentage,          // 百分比（被施法者）
    PercentageTrigger,   // 百分比（施法者）
}
```

##### 性能优化特性

**1. 缓存机制**
- 自动缓存计算结果
- 脏标记避免不必要的重新计算
- 支持高频属性值查询

**2. 内存优化**
- 使用引用传递减少值拷贝
- 内联优化减少函数调用开销
- 高效的数据结构设计

**3. 批量操作**
- 支持批量添加和移除修改器
- 层级操作减少循环次数
- 智能的脏标记管理

## 设计模式
1. 状态模式（State Pattern）
   - 用于管理角色状态转换
   - 实现于BaseState及其子类

2. 组合模式（Composite Pattern）
   - 用于行为树节点组织
   - 实现于WeightSelector等组合节点

3. 观察者模式（Observer Pattern）
   - 用于事件系统
   - 实现于输入系统和状态转换

4. 策略模式（Strategy Pattern）
   - 用于AI决策
   - 实现于行为树节点

5. **装饰器模式（Decorator Pattern）**
   - 用于属性修改器系统
   - 实现于AttributeLayer类

## 已知问题
1. 相机系统
   - 索敌状态下的相机切换可能不够平滑
   - 需要优化相机过渡效果

2. 状态系统
   - 部分状态转换可能不够流畅
   - 需要优化状态切换逻辑

3. AI系统
   - 权重选择器可能需要更多调试选项
   - 需要添加更多行为节点类型

## 开发建议
1. 代码质量
   - 添加单元测试
   - 实现更完善的错误处理
   - 添加性能监控

2. 功能扩展
   - 添加更多AI行为节点
   - 优化相机系统
   - 增加更多角色状态

3. 工具支持
   - 添加调试工具
   - 完善编辑器扩展
   - 添加性能分析工具

4. 文档完善
   - 添加API文档
   - 完善使用说明
   - 添加开发指南

## 开发规范
1. 代码风格
   - 使用C#命名规范
   - 添加适当的注释
   - 保持代码简洁

2. 版本控制
   - 使用Git Flow工作流
   - 保持提交信息清晰
   - 定期代码审查

3. 资源管理
   - 遵循Unity资源命名规范
   - 合理组织资源目录
   - 注意资源优化

## 后续计划
1. 短期目标
   - 优化相机系统
   - 完善AI行为
   - 添加更多角色状态

2. 中期目标
   - 添加单元测试
   - 实现性能监控
   - 完善调试工具

3. 长期目标
   - 优化整体架构
   - 添加更多游戏特性
   - 提升游戏性能

## 贡献指南
1. 代码提交
   - 遵循Git Flow工作流
   - 提交前进行代码审查
   - 保持提交信息清晰

2. 问题报告
   - 使用Issue模板
   - 提供详细的问题描述
   - 附上复现步骤

3. 功能请求
   - 说明功能用途
   - 提供实现建议
   - 考虑兼容性

## 许可证
[待定]

## 联系方式
[待定]