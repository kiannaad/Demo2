using System.Collections;
using System.Collections.Generic;
using Animancer;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class FallState : BaseState
{
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
     
        player.SetRootMotionOffset("Fall");
    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        Debug.Log($"{nameof(FallState)}: OnUpdate");
        CheckForLanding();
    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
       
    }
    
    protected override void OnDestroy(IFsm<PlayerState> fsm)
    {
        base.OnDestroy(fsm);
    }

    public override void SwitchToIdle()
    {
        base.SwitchToIdle();
        if (player._isInAirAttacking)
        {
            PlayDirector(player.locomotionData._stopActionData._AirAttackDownLand);
            player._isInAirAttacking = false;
        }
        else
        {
            PlayDirector(player.locomotionData._stopActionData._Land);
        }
    }

    public override void SwitchToMove()
    {
        ChangeState<MoveState>(player.playerStateFsm);
        if (player._isInAirAttacking)
        {
            PlayDirector(player.locomotionData._stopActionData._AirAttackDownLand);
            player._isInAirAttacking = false;
        }
        else
        {
            PlayDirector(player.locomotionData._loopActionData.MoveMixerTimeline);
        }
    }

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);
        if (player._isInAirAttacking)
        {
            PlayDirector(player.locomotionData._loopActionData._AirAttackDownTimeline);
            return;
        }
      
        PlayDirector(player.locomotionData._loopActionData._FallTimeline);
    }

    private void CheckForLanding()
    {
        if (player.isGrounded)
        {
            if (player.HorizontalInput == Vector3.zero)
            {
                SwitchToIdle();
            }
            else if (player.HorizontalInput != Vector3.zero)
            {
                SwitchToMove();
            }
        }
    }

    public override void JumpPerformed(InputAction.CallbackContext context)
    {
        //置空，无法跳跃
    }

    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        if (player._isInAirAttacking) return;
        
        base.AttackPerformed(context);
        player._isInAirAttacking = true; 
        ChangeState<FallState>(player.playerStateFsm);
        PlayDirector(player.locomotionData._startActionData._AirAttackDownStart);
    }
}
