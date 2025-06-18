using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CrouchMoveState : BaseState
{
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
        
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        player.UpdateRotation(player.HorizontalInput, 0.1f,
            5f);
        
        Debug.Log($"{nameof(CrouchMoveState)} : Update");

    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        
    }
    
    protected override void OnDestroy(IFsm<PlayerState> fsm)
    {
        base.OnDestroy(fsm);
    }
    
    public override void SwitchToAttack()
    {
        base.SwitchToAttack();
        PlayDirector(player.HeavyAttackData.HeavyAttack);
    }

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);
        PlayDirector(player.locomotionData._loopActionData._CrouchMoveTimeline);
    }

    public override void MoveCanceled(InputAction.CallbackContext context)
    {
        base.MoveCanceled(context);
        ChangeState<CrouchIdleState>(player.playerStateFsm);
        PlayDirector(player.locomotionData._stopActionData._CrouchMoveStop);
    }

    public override void CrouchPerformed(InputAction.CallbackContext context)
    {
        base.CrouchPerformed(context);
        SwitchToMove();
    }
    
    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        base.AttackPerformed(context);
        SwitchToAttack();
    }

    /*public override void LeftShiftPerformed(InputAction.CallbackContext context)
    {
        base.LeftShiftPerformed(context);
        PlayDirector(player.locomotionData._loopActionData._CrouchRunTimeline._Timeline);
    }*/
}
