using System.Collections;
using System.Collections.Generic;
using Animancer;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SkillState : BaseState
{
    private SkillType _skillType;
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
        CanLockRotate = false;
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
        
        _skillType = player.SkillData._skillType;
        
        if (_skillType == SkillType.Charge)
        {
            CanRotate = true;
            PlayDirector(player.SkillData._ToDefenseTimeline);
        }
        else if (_skillType == SkillType.Instant)
        {
            CanRotate = false;
            PlayDirector(player.SkillData._InstantTimeline);
        }
        
        GameEntry.Event.Subscribe(ChangeStateEventArg.EventId, HandleContinueAttack);
    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        Debug.Log($"{nameof(SkillState)} : Update");
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
        if (_skillType == SkillType.Charge)
        {
            PlayDirector(player.SkillData._ExitDefenseTimeline);
        }
        else if (_skillType == SkillType.Instant)
        {
            PlayDirector(player.locomotionData._loopActionData._IdleTimeline);
        }
    }

    public override void SwitchToAttack()
    {
        base.SwitchToAttack();
        PlayDirector(player.SkillData._DefenseAttackTimeline);
    }

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);
        if (_skillType == SkillType.Charge)
        {
            PlayDirector(player.SkillData._DefenseIdleTimeline);
        }
        else if (_skillType == SkillType.Instant)
        {
            SwitchToIdle();
        }
    }

    public override void MovePerformed(InputAction.CallbackContext context)
    {
        base.MovePerformed(context);
        if (_skillType == SkillType.Charge)
        {
            PlayDirector(player.SkillData._DefenseWalkFTimeline);
        }
    }

    public override void MoveCanceled(InputAction.CallbackContext context)
    {
        base.MoveCanceled(context);
        if (_skillType == SkillType.Charge)
        {
            PlayDirector(player.SkillData._DefenseIdleTimeline);
        }
    }

    public override void Skillcanceled(InputAction.CallbackContext context)
    {
        base.Skillcanceled(context);
        if (_skillType == SkillType.Charge)
        {
           SwitchToIdle();
        }
    }

    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        base.AttackPerformed(context);
        if (_skillType == SkillType.Charge)
        {
            SwitchToAttack();
            return;
        }

        if (CanAttack && _targetTimelineAsset != null)
        {
            ChangeState<AttackState>(player.playerStateFsm);
            PlayDirector(_targetTimelineAsset);
        }
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
