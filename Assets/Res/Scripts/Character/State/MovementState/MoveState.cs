using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MoveState : BaseState
{

    private Vector3 _preInput;

    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
        CanRotate = true;
        CanInterruptSwitchWeapon = false;
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
       player.UpdateRotationgPar(player.movementData.moveData.RotateSmoothTime, player.movementData.moveData.RotateSpeed);
       _preInput = player.HorizontalInput;
    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        Debug.Log($"{nameof(MoveState)} : Update");

        CheckForFalling();
        player.UpdateMovementPar(player.movementData.moveData.TargetSpeed, player.movementData.moveData.SpeedSmoothTime);
        
        if (player._NeedRefresh)
        {
            ChangeState<IdleState>(player.playerStateFsm);
            player._NeedRefresh = false;
        }

        if (_preInput != player.HorizontalInput && player.CheckLockTarget())
        {
            _preInput = player.HorizontalInput;
            PlayDirector(player.GetResearchMoveTimeline());
        }
        
    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }

    public override void SwitchToIdle()
    {
        //Debug.Log($"Move SwitchToIdle");
        base.SwitchToIdle();
        if (!player.CheckLockTarget())
        {
            PlayDirector(player.locomotionData._stopActionData._MoveStop);
        }
        else
        {
            PlayDirector(player.locomotionData._loopActionData._IdleTimeline);
        }
    }
    
    public override void SwitchToAttack()
    {
        base.SwitchToAttack();
        PlayDirector(player.GetCurrentLightComboTimeline());
    }
    
    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);
        if (player.CheckLockTarget())
        {
            ConnectPlayDirector(player.GetResearchMoveTimeline());
        }
        else
        {
            ConnectPlayDirector(player.locomotionData._loopActionData.MoveMixerTimeline);
        }
    }

    public override void MoveCanceled(InputAction.CallbackContext context)
    {
        base.MoveCanceled(context);
        SwitchToIdle();
    }
    
    public override void LeftShiftPerformed(InputAction.CallbackContext context)
    {
        base.LeftShiftPerformed(context);
        SwitchToRush();
    }

    public override void CrouchPerformed(InputAction.CallbackContext context)
    {
        base.CrouchPerformed(context);
        SwitchToCrouchMove();
    }

    public override void JumpPerformed(InputAction.CallbackContext context)
    {
        base.JumpPerformed(context);
        player.SetRootMotionOffset("RunJump");
    }
    
    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        base.AttackPerformed(context);
        SwitchToAttack();
    }
}
