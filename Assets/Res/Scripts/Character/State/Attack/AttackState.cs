using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AttackState : BaseState
{
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        base.OnInit(fsm);
        CanLockRotate = false;
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        base.OnEnter(fsm);
        GameEntry.Event.Subscribe(ChangeStateEventArg.EventId, HandleAttackDecision);
        GameEntry.Event.Subscribe(ChangeStateEventArg.EventId, HandleCanInterrupted);
        GameEntry.Event.Subscribe(ChangeStateEventArg.EventId, HandleContinueAttack);
        
        InitPar();
    }
    
    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        Debug.Log($"{nameof(AttackState)} : Update");
       // Debug.Log($"{CanMoveInterrupt}");
        
    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        GameEntry.Event.Unsubscribe(ChangeStateEventArg.EventId, HandleAttackDecision);
        GameEntry.Event.Unsubscribe(ChangeStateEventArg.EventId, HandleCanInterrupted);
        GameEntry.Event.Unsubscribe(ChangeStateEventArg.EventId, HandleContinueAttack);
    }

    public override void SwitchToIdle()
    {
        base.SwitchToIdle();
        PlayDirector(player.locomotionData._loopActionData._IdleTimeline);
        player.ResetComboCount();
    }

    public override void SwitchToAttack()
    {
        if (CanContinueAttack && _targetTimelineAsset != null)
        {
            ChangeState<AttackState>(player.playerStateFsm);
            PlayDirector(_targetTimelineAsset);
        }
        else
        {
            if (NeedReset) player.ResetComboCount();
            base.SwitchToAttack();
            PlayDirector(player.GetCurrentLightComboTimeline());
        }
    }

    public override void SwitchToDodgeBack()
    {
        base.SwitchToDodgeBack();
        player.ResetComboCount();
    }

    public override void SwitchToMove()
    {
        base.SwitchToMove();
        player.ResetComboCount();
    }

    public override void SwitchToDodgeRoll()
    {
        base.SwitchToDodgeRoll();
        player.ResetComboCount();
    }

    public override void SwitchToJump()
    {
        base.SwitchToJump();
        player.ResetComboCount();
    }

    public override void ConnectedToLoopingAni(PlayableDirector director)
    {
        base.ConnectedToLoopingAni(director);

        
        if (player.SkillData._skillType == SkillType.Charge && player.inputAction.Skill.ReadValue<float>() == 1f)
        { 
            SwitchToSkill();
            return;
        }
        
        if (player.HorizontalInput == Vector3.zero)
        {
            SwitchToIdle();
            player.SetMovementParZero();
            PlayDirector(player.locomotionData._loopActionData._IdleTimeline);
        }
        else
        { 
            SwitchToMove();
        }
        
       
    }

    public override void AttackPerformed(InputAction.CallbackContext context)
    {
        if (!CanAttack) return;
        
        base.AttackPerformed(context);
        SwitchToAttack();
       
    }

    public override void SpacePerformed(InputAction.CallbackContext context)
    {
        if (!CanMoveInterrupt) return;
        
        base.SpacePerformed(context);
    }

    public override void MovePerformed(InputAction.CallbackContext context)
    {
        if (!CanMoveInterrupt) return;
        
        base.MovePerformed(context);
        SwitchToMove();
    }

    public override void JumpPerformed(InputAction.CallbackContext context)
    {
        if (!CanMoveInterrupt) return;
        
        base.JumpPerformed(context);
    }

    private bool CanAttack;
    private bool CanMoveInterrupt;

    private bool NeedReset;

    private void InitPar()
    {
        CanAttack = false;
        CanMoveInterrupt = false;
        NeedReset = false;
        CanContinueAttack = false;
    }
    
    private void HandleAttackDecision(object sender, GameEventArgs args)
    {
        var arg = args as ChangeStateEventArg;
        
        if (arg == null || arg.targetStateName != "Attack Decision") return;

        if (arg.isEnd == false)
        {
            CanAttack = true;
        }
        else if (arg.isEnd == true)
        {
            NeedReset = true;
        }
    }
    
    private void HandleCanInterrupted(object sender, GameEventArgs args)
    {
        var arg = args as ChangeStateEventArg;
        
        if (arg == null || arg.targetStateName != "Can Interrupted") return;

        if (arg.isEnd == false)
        {
        
            CanMoveInterrupt = true;
        }
    }

    private TimelineAsset _targetTimelineAsset;
    private bool CanContinueAttack = false;
    private void HandleContinueAttack(object sender, GameEventArgs e)
    {
        var arg = (ChangeStateEventArg)e;
        
        if (arg == null || arg.targetStateName != "ContinueAttack") return;

        if (!arg.isEnd)
        {
            _targetTimelineAsset = arg._targetTimeline;
            CanAttack = true;
            CanContinueAttack = true;
        }
        else if (arg.isEnd)
        {
            _targetTimelineAsset = null;
            CanContinueAttack = false;
        }
    }
    
    
}
