using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class JumpState : BaseState
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
        Debug.Log($"{nameof(JumpState)}: OnUpdate");
    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }
    
    protected override void OnDestroy(IFsm<PlayerState> fsm)
    {
        base.OnDestroy(fsm);
    }

    public override void SwitchToFall()
    {
        ChangeState<FallState>(player.playerStateFsm);
        
        if (player._isInAirAttacking)
        {
            PlayDirector(player.locomotionData._loopActionData._AirAttackDownTimeline);
            return;
        }
      
        PlayDirector(player.locomotionData._loopActionData._FallTimeline);
    }

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);
        SwitchToFall();
    }

    public override void JumpPerformed(InputAction.CallbackContext context)
    {
        
    }

    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        base.AttackPerformed(context);
        if (player._isInAirAttacking) return;
        PlayDirector(player.locomotionData._startActionData._AirAttackDownStart);
        player._isInAirAttacking = true;
    }
}
