using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CrouchIdleState : BaseState
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
        
        Debug.Log($"{nameof(CrouchIdleState)} : Update");

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
        PlayDirector(player.locomotionData._loopActionData._CrouchIdleTimeline);
    }

    public override void CrouchPerformed(InputAction.CallbackContext context)
    {
        base.CrouchPerformed(context);
        ChangeState<IdleState>(player.playerStateFsm);
        PlayDirector(player.locomotionData._startActionData._CrouchToStand);
    }

    public override void MovePerformed(InputAction.CallbackContext context)
    {
        base.MovePerformed(context);
        ChangeState<CrouchMoveState>(player.playerStateFsm);
        PlayDirector(player.locomotionData._startActionData._CrouchMoveStart);
    }

    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        base.AttackPerformed(context);
        SwitchToAttack();
    }
}
