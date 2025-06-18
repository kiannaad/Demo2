using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class RushState: BaseState
{
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
        CanRotate = true;
        CanInterruptSwitchWeapon = false;
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
        player.UpdateRotationgPar(player.movementData.rushData.RotateSmoothTime, player.movementData.rushData.RotateSpeed);

    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        Debug.Log($"{nameof(RushState)} : Update");

        CheckForFalling();
        player.UpdateMovementPar(player.movementData.rushData.TargetSpeed, player.movementData.rushData.SpeedSmoothTime);
        
        if (player._NeedRefresh)
        {
            ChangeState<IdleState>(player.playerStateFsm);
            player._NeedRefresh = false;
        }
    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        
    }
    
    public override void SwitchToIdle()
    {
        base.SwitchToIdle();
        PlayDirector(player.locomotionData._stopActionData._RushStop);
    }

    public override void MoveCanceled(InputAction.CallbackContext context)
    {
        base.MoveCanceled(context);
        SwitchToIdle();
    }

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);
        ConnectPlayDirector(player.locomotionData._loopActionData.MoveMixerTimeline);
    }

    public override void JumpPerformed(InputAction.CallbackContext context)
    {
        base.JumpPerformed(context);
        player.SetRootMotionOffset("QuickJump");
    }
}
