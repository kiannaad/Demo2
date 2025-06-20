using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor;
using UnityEngine;


public enum JudgeDistanceType
{
    Greater,
    Smaller,
    Equal,
}

[TaskCategory("Custom/Enemy/Conditional")]
public class JudgeDistance : Conditional
{
    public SharedGameObject _player;
    public SharedGameObject _enemy;
    public JudgeDistanceType _judgeDistanceType;
    public float _TargetDistance;


    public override TaskStatus OnUpdate()
    {
        if (_player == null || _enemy == null) return TaskStatus.Failure;

        float distance = Vector3.Distance(_player.Value.transform.position, _enemy.Value.transform.position);

        switch (_judgeDistanceType)
        {
            case JudgeDistanceType.Greater:
                return distance > _TargetDistance ? TaskStatus.Success : TaskStatus.Failure;
            case JudgeDistanceType.Smaller:
                return distance < _TargetDistance ? TaskStatus.Success : TaskStatus.Failure;
            case JudgeDistanceType.Equal:
                return distance == _TargetDistance ? TaskStatus.Success : TaskStatus.Failure;
            default : return TaskStatus.Failure;
        }
    }
}

// [CustomPropertyDrawer(typeof(JudgeDistance))]
// public class JudgeDistanceProperty : PropertyDrawer
// {

//     private SerializedProperty _judgeDistanceType;
//     private SerializedProperty _maxDistance;
//     private SerializedProperty _minDistance;
//     private SerializedProperty _TargetDistance;

//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         GetProperty(property);
//     if ((JudgeDistanceType)_judgeDistanceType.enumValueIndex == JudgeDistanceType.Interval)
//         return EditorGUIUtility.singleLineHeight * 3; // 第一行 + 两行（最大和最小）
//     else
//         return EditorGUIUtility.singleLineHeight * 2; // 第一行 + 一行（目标距离）
//     }

//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         GetProperty(property);

//         EditorGUI.BeginProperty(position, label, property);

//         EditorGUI.PropertyField(position, _judgeDistanceType, new GUIContent("Judge Distance Type"));
//         if ((JudgeDistanceType)_judgeDistanceType.enumValueIndex == JudgeDistanceType.Interval)
//         {
//             DrawInterval(position, position.y + EditorGUIUtility.singleLineHeight);
//         }
//         else
//         {
//            DrawGreater(position, position.y + EditorGUIUtility.singleLineHeight);
//         }

//         EditorGUI.EndProperty();
//     }

//     private void DrawInterval(Rect position, float y)
//     {
//         Rect maxDistanceRect = new Rect(position.x, y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
//         Rect minDistanceRect = new Rect(position.x, y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight);

//         EditorGUI.PropertyField(maxDistanceRect, _maxDistance, new GUIContent("Max Distance"));
//         EditorGUI.PropertyField(minDistanceRect, _minDistance, new GUIContent("Min Distance"));
//     }

//     private void DrawGreater(Rect position, float y)
//     {
//         Rect targetDistanceRect = new Rect(position.x, y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
//         EditorGUI.PropertyField(targetDistanceRect, _TargetDistance, new GUIContent("Target Distance"));
//     }

//     private void GetProperty(SerializedProperty property)
//     {
//         _judgeDistanceType = property.FindPropertyRelative("_judgeDistanceType");
//         _maxDistance = property.FindPropertyRelative("_maxDistance");
//         _minDistance = property.FindPropertyRelative("_minDistance");
//         _TargetDistance = property.FindPropertyRelative("_TargetDistance");
//     }
// }
