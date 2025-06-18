using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.Playables;

public class DodgeState : BaseState
{
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
        CanLockRotate = false;
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
       
    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        Debug.Log($"{nameof(DodgeState)} : Update");
        
    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }

    public override void SwitchToIdle()
    {
        base.SwitchToIdle();
        PlayDirector(player.locomotionData._loopActionData._IdleTimeline);
    }

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);
        if (player.HorizontalInput == Vector3.zero)
        {
            if (director.playableAsset != player.locomotionData._loopActionData._IdleTimeline)
            {
                SwitchToIdle();
            }
        }
        else
        {
            SwitchToMove();
        }
    }
}
