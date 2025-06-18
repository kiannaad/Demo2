using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class IdleState : BaseState
{
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
        CanInterruptSwitchWeapon = false;
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
        isToMove = true;
        //player.SetMovementParZero();
    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        Debug.Log($"{nameof(IdleState)} : Update");

        CheckForFalling();
        
        if (player._NeedRefresh)
        {
            ChangeState<IdleState>(player.playerStateFsm);
            player._NeedRefresh = false;
            Debug.Log("Need Refresh");
        }

    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }
    
    protected override void OnDestroy(IFsm<PlayerState> fsm)
    {
        base.OnDestroy(fsm);
    }

    public override void SwitchToCrouchIdle()
    {
        base.SwitchToCrouchIdle();
        PlayDirector(player.locomotionData._startActionData._StandToCrouch);
    }

    private bool isToMove;
    
   

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);
        
        player.SetRootMotionOffsetZero();
        if (player.HorizontalInput == Vector3.zero)
        {
            if (director.playableAsset != player.locomotionData._loopActionData._IdleTimeline)
            {
                player.SetMovementParZero();
                PlayDirector(player.locomotionData._loopActionData._IdleTimeline);
            }
        }
        else
        {
            SwitchToMove();
        }
    }

    public override void SwitchToAttack()
    {
        base.SwitchToAttack();
        PlayDirector(player.GetCurrentLightComboTimeline());
    }

    #region 过渡方法

    public override void MovePerformed(InputAction.CallbackContext context)
    {
        base.MovePerformed(context);
        if (isToMove)
            SwitchToMove();
        else
        {
            SwitchToRush();
        }
    }
    
    public override void LeftShiftPerformed(InputAction.CallbackContext context)
    {
        base.LeftShiftPerformed(context);
        isToMove = false;
    }

    public override void LeftShiftCanceled(InputAction.CallbackContext context)
    {
        base.LeftShiftCanceled(context);
        isToMove = true;
    }

    public override void CrouchPerformed(InputAction.CallbackContext context)
    {
        base.CrouchPerformed(context);
        SwitchToCrouchIdle();
    }

    public override void SpacePerformed(InputAction.CallbackContext context)
    {
        SwitchToDodgeBack();
    }

    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        base.AttackPerformed(context);
        SwitchToAttack();
    }

    public override void HeavyAttackPerformed(InputAction.CallbackContext context)
    {
        base.HeavyAttackPerformed(context);
        ChangeState<HeavyAttackState>(player.playerStateFsm);
        PlayDirector(player.HeavyAttackData.HeavyAttack_Charge);
    }

    public override void SkillPerformed(InputAction.CallbackContext context)
    {
        base.SkillPerformed(context);
        SwitchToSkill();
    }

    #endregion
}
