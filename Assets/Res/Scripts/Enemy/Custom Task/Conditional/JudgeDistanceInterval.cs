using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Enemy/Conditional")]
public class JudgeDistanceInterval : Conditional
{
    public SharedGameObject _player;
    public SharedGameObject _enemy;
    public float _maxDistance;
    public float _minDistance;


    public override TaskStatus OnUpdate()
    {
        if (_player == null || _enemy == null) return TaskStatus.Failure;

        float distance = Vector3.Distance(_player.Value.transform.position, _enemy.Value.transform.position);

        if (distance >= _maxDistance || distance <= _minDistance) return TaskStatus.Failure;

        return TaskStatus.Success;
    }
}