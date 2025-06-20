using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime;
using UnityGameFramework.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TaskDescription("Move to player")]
[TaskCategory("Custom/Enemy/Action")]
public class MoveToPlayer : Action
{
    public TimelineAsset _timeline;
    private PlayableDirector _playableDirector;
    public SharedGameObject _player;
    public SharedGameObject _enemy;

    public override void OnAwake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public override TaskStatus OnUpdate()
    {
        _playableDirector.Play(_timeline);
        if (Vector2.Distance(_enemy.Value.transform.position, _player.Value.transform.position) < 6f)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
