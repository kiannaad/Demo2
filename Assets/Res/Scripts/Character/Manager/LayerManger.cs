using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

[Serializable]
public class LayerManger
{
    [HideInInspector]
    public AnimancerLayer _BaseLayer;
    [HideInInspector]
    public AnimancerLayer _BeHalfLayer;
    [HideInInspector]
    public AnimancerLayer _FrHalfLayer;

    public AvatarMask _BodyMask;
    public AvatarMask _HeadMask;
    public AvatarMask _LegMask;
    public AvatarMask _ChestMask;

    public void Init(AnimancerComponent animancer)
    {
            _BaseLayer = animancer.Layers[0];
            _BeHalfLayer = animancer.Layers[1];
            _FrHalfLayer = animancer.Layers[2];
    
            _BaseLayer.Mask = _BodyMask;
            _BeHalfLayer.Mask = _LegMask;
    }
}
