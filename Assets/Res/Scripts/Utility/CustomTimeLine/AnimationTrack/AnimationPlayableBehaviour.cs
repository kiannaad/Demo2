using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Animancer;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AnimationPlayableBehaviour : PlayableBehaviour
{
    private AnimancerComponent animancerComponent;
    private ClipTransition clip;
    private int layerIndex;
    private AnimancerLayer animancerLayer;
    
    private float _time;

    private float _fadeTime;
    private FadeMode _fadeMode;

    public void Init(AnimancerComponent animancerComponent, ClipTransition clip, int layerIndex, float fadeTime, FadeMode fadeMode)
    {
        this.animancerComponent = animancerComponent;
        this.clip = clip;
        this.layerIndex = layerIndex;
        
        if (animancerComponent == null)
        {
            Debug.LogError("animancerComponent is null");
            return;
        }

        if (animancerComponent.Layers[layerIndex] == null)
        {
            Debug.LogError("animancerComponent.Layers[layerIndex] is null");
            return;
        }
        
        animancerLayer = animancerComponent.Layers[layerIndex];
        
        _fadeMode = fadeMode;
        _fadeTime = fadeTime;
        
    }

    public void UpdateClip(ClipTransition clipTransition)
    {
        this.clip = clipTransition;
       
        animancerLayer.Stop();
        var state = animancerLayer.Play(clip, _fadeTime, _fadeMode);
        state.Time = 0f;
        animancerLayer.CurrentState.Clip = clip.Clip;
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        
        //Debug.Log("OnBehaviourPlay");
        var state = animancerLayer.Play(clip, _fadeTime, _fadeMode);
        state.Time = 0f;
        state.Events(nameof(PlayerState)).Clear();
        state.Events(nameof(PlayerState)).OnEnd = () =>
        {
            if (animancerLayer != animancerComponent.Layers[0])
            {
                animancerLayer.StartFade(0, AnimancerGraph.DefaultFadeDuration);
            }
        };
        
        if (Application.isEditor && !Application.isPlaying && animancerLayer.CurrentState != null)
        {
            animancerLayer.CurrentState.Time = _time;
        }
        
    }
    
   
    public override void PrepareFrame(Playable playable, FrameData info)
    {
            // animancerLayer.Play(clip, fadeDuration: 0.25f);
            // animancerLayer.Weight = 1f; // 强制权重为 1
           // Debug.Log(animancerLayer.CurrentState.Clip.name);
        
        if (Application.isEditor && !Application.isPlaying)
        {
            base.PrepareFrame(playable, info);
            //Debug.Log(animancerLayer.CurrentState.Clip.name);
        
            if (animancerLayer.CurrentState != null)
            {
                animancerLayer.CurrentState.Speed = 0f;
                _time = (float)playable.GetTime();
                animancerLayer.CurrentState.MoveTime(_time, false);
            }
        
            animancerComponent.Evaluate(); // 确保编辑器刷新动画
        }
    }
    
}
