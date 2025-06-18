using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationPlayableMixerBehaviour : PlayableBehaviour
{
    private AnimancerComponent animancerComponent;
    private TransitionAsset _transitionAsset;
    private int layerIndex;
    private AnimancerLayer animancerLayer;
    
    private float _time;

    public void Init(AnimancerComponent animancerComponent, TransitionAsset _transitionAsset, int layerIndex)
    {
        this.animancerComponent = animancerComponent;
        this._transitionAsset = _transitionAsset;
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
    }

    public void UpdateClip(TransitionAsset clipTransition)
    {
        this._transitionAsset = clipTransition;
        
        //animancerLayer.CurrentState.Clip = _transitionAsset.Transition.;
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        
        animancerLayer.Play(_transitionAsset, 0.25f);
        

        if (Application.isEditor && !Application.isPlaying && animancerLayer.CurrentState != null)
        {
            animancerLayer.CurrentState.Time = _time;
        }
        
    }
    
   
    public override void PrepareFrame(Playable playable, FrameData info)
    {
            /*animancerLayer.Play(_transitionAsset);
            animancerLayer.Weight = 1f; // 强制权重为 1*/
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
