using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;



[TaskCategory("Custom/Enemy/Conditional")]
public class InitTree : Conditional
{
    public SharedString _playerTag;
    public SharedGameObject _player;
  
    public override TaskStatus OnUpdate()
    {
        if (_player.Value == null) 
        {
            _player.Value = GameObject.FindGameObjectWithTag(_playerTag.Value);
        }

        return _player.Value == null ? TaskStatus.Failure : TaskStatus.Success;
    }
}


