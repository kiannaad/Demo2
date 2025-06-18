using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class AnimationPlayableAsset : PlayableAsset, ITimelineClipAsset
{
    [SerializeReference] public ClipTransition clipTransition;
    [Range(0, 10)] public int LayerIndex;
    
    private AnimationPlayableBehaviour playableBehaviour;
    private AnimancerComponent animancerComponent;
    private PlayableDirector director;

    public float FadeTime = 0.25f;
    public FadeMode FadeMode;
    public DirectorWrapMode directorWrapMode;
    public bool ApplyFootIK;
    
    public override double duration
    {
        get
        {
            // 检查 _AnimationClip 和其 Clip 是否有效
            if (clipTransition == null || clipTransition.Clip == null)
            {
                Debug.LogError("AnimationPlayableAsset 配置错误：_AnimationClip 或 Clip 未赋值！");
                return 0; // 返回默认时长（例如 0 秒）
            }

            return clipTransition.Clip.length;
        }
    }
    public ClipCaps clipCaps => ClipCaps.Blending;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        if (clipTransition == null || clipTransition.Clip == null)
        {
            Debug.LogError($"{owner.GetComponent<PlayableDirector>().playableAsset.name} clipTransition <UNK>_AnimationClip <UNK> Clip <UNK>");
            return Playable.Null;
        }
        
        var scriptPlyable = ScriptPlayable<AnimationPlayableBehaviour>.Create(graph);
         playableBehaviour = scriptPlyable.GetBehaviour();
         
         animancerComponent = owner.GetComponent<AnimancerComponent>();
         director = owner.GetComponent<PlayableDirector>();

         
         director.extrapolationMode = directorWrapMode;
          
         
         animancerComponent.Animator.applyRootMotion = true;
         animancerComponent.Layers[LayerIndex].ApplyFootIK = ApplyFootIK;
         
         playableBehaviour.Init(animancerComponent, clipTransition, LayerIndex, FadeTime, FadeMode);
       
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
                playableBehaviour.UpdateClip(clipTransition);

                director.RebuildGraph();
                director.Evaluate();
            }
        };

    }
}
