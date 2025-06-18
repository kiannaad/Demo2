using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using UnityEngine.Timeline;

public class test : MonoBehaviour 
{
    public AnimationClip clip;
    private AnimancerComponent animancer;

    private void Awake()
    {
        animancer = GetComponent<AnimancerComponent>();
    }

    private void Start()
    {
        animancer.Play(clip);
    }
}
