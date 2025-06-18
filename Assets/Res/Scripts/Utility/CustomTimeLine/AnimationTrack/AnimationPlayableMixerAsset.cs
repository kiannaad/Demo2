using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

public class AnimationPlayableMixerAsset : PlayableAsset
{
    [SerializeReference] public TransitionAsset _MixerTransition;
    [Range(0, 10)] public int LayerIndex;
    
    private AnimationPlayableMixerBehaviour playableBehaviour;
    private AnimancerComponent animancerComponent;
    private PlayableDirector director;
    
    public DirectorWrapMode directorWrapMode;
    public bool ApplyFootIK;
    public bool ApplyRootMotion;
    
    public override double duration
    {
        get
        {
            // 检查 _AnimationClip 和其 Clip 是否有效
            if (_MixerTransition == null || _MixerTransition.Transition == null)
            {
                Debug.LogError("AnimationPlayableAsset 配置错误：_AnimationClip 或 Clip 未赋值！");
                return 0; // 返回默认时长（例如 0 秒）
            }

            return _MixerTransition.MaximumDuration;
        }
    }
    public ClipCaps clipCaps => ClipCaps.Blending;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        if (_MixerTransition == null || _MixerTransition.Transition == null)
        {
            Debug.LogError("clipTransition <UNK>_AnimationClip <UNK> Clip <UNK>");
            return Playable.Null;
        }
        
        var scriptPlyable = ScriptPlayable<AnimationPlayableMixerBehaviour>.Create(graph);
         playableBehaviour = scriptPlyable.GetBehaviour();
         
         animancerComponent = owner.GetComponent<AnimancerComponent>();
         director = owner.GetComponent<PlayableDirector>();

         director.extrapolationMode = directorWrapMode;
         
         animancerComponent.Animator.applyRootMotion = ApplyRootMotion;
         animancerComponent.Layers[LayerIndex].ApplyFootIK = ApplyFootIK;
         
         playableBehaviour.Init(animancerComponent, _MixerTransition, LayerIndex);
       
        return scriptPlyable;
    }

    public void OnValidate()
    {
        if (Application.isPlaying) return;

        EditorApplication.delayCall += () =>
        {
            if (playableBehaviour != null)
            {
                //Debug.Log("OnValidate");
                //playableBehaviour.UpdateClip(_MixerTransition);

                director.RebuildGraph();
                director.Evaluate();
            }
        };

    }
}
