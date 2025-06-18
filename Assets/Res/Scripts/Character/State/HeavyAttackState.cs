using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class HeavyAttackState : BaseState
{
    private bool _isInHeavyAttacking;
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
        _isInHeavyAttacking = false;
        CanLockRotate = false;
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
        GameEntry.Event.Subscribe(ChangeStateEventArg.EventId, HandleContinueAttack);
    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        Debug.Log($"{nameof(HeavyAttackState)}: OnUpdate");
       // Debug.Log($"TheFirst : {theFirst}");
       
       Debug.Log($"_targetTimelineAsset : {_targetTimelineAsset != null}");
        
    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        GameEntry.Event.Unsubscribe(ChangeStateEventArg.EventId, HandleContinueAttack);
    }
    
    protected override void OnDestroy(IFsm<PlayerState> fsm)
    {
        base.OnDestroy(fsm);
    }
    
    public override void SwitchToIdle()
    {
        base.SwitchToIdle();
        PlayDirector(player.HeavyAttackData.HeavyAttack_End);
    }

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
      
        base.ConnectedToLoopingAni(director);
        if (!_isInHeavyAttacking)
        {
            PlayDirector(player.HeavyAttackData.HeavyAttack);
            _isInHeavyAttacking = true;
            ChangeState<HeavyAttackState>(player.playerStateFsm);
        }
        else
        {
            if (player.HorizontalInput == Vector3.zero)
            {
                SwitchToIdle();
            }
            else
            {
                SwitchToMove();
            }

            _isInHeavyAttacking = false;
        }
    }

    public override void HeavyAttackCanceled(InputAction.CallbackContext context)
    {
        base.HeavyAttackCanceled(context);

        if (!_isInHeavyAttacking)
        {
            PlayDirector(player.HeavyAttackData.HeavyAttack);
            _isInHeavyAttacking = true;
            ChangeState<HeavyAttackState>(player.playerStateFsm);
        }
    }

    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        if (!CanAttack || _targetTimelineAsset == null) return;
        
        base.AttackPerformed(context);
        SwitchToAttack();
        PlayDirector(_targetTimelineAsset);
    }

    private bool CanAttack = false;
    private TimelineAsset _targetTimelineAsset;
    
    private void HandleContinueAttack(object sender, GameEventArgs e)
    {
        var arg = (ChangeStateEventArg)e;
        
        if (arg == null || arg.targetStateName != "ContinueAttack") return;
        
        
        if (!arg.isEnd)
        {
            _targetTimelineAsset = arg._targetTimeline;
            Debug.Log(arg._targetTimeline.name);
            CanAttack = true;
            
        }
        else if (arg.isEnd)
        {
            _targetTimelineAsset = null;
            CanAttack = false;
        }
    }
}
