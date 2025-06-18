using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BaseState : FsmState<PlayerState>
{
    protected PlayerState player;
    protected bool theFirst;
    protected bool CanRotate = false;
    protected bool CanLockRotate = true;    
    protected bool CanInterruptSwitchWeapon;
    
    
    protected override void OnInit(IFsm<PlayerState> fsm)
    {
        player = fsm.Owner;
        CanInterruptSwitchWeapon = true;
    }

    protected override void OnEnter(IFsm<PlayerState> fsm)
    {
        RegisterEvent();
        theFirst = true;
        if (CanInterruptSwitchWeapon)
            InterruptSwitchWeapon();
    }

    protected override void OnUpdate(IFsm<PlayerState> fsm, float elapseSeconds, float realElapseSeconds)
    {
        if (player.playableDirector.extrapolationMode != DirectorWrapMode.Loop && 
            player.playableDirector.duration - 0.1f <= player.playableDirector.time && theFirst)
        {
            //Debug.Log("ConnectToAni");
            theFirst = false;
            ConnectedToLoopingAni(player.playableDirector);
        }

        if (CanRotate && !player.CheckLocking())
        {
            player.UpdateRotation(player.HorizontalInput, player.curSmoothTime, 
                player.curRotateSpeed);
        }
        else if (player.CheckLockTarget() && CanLockRotate)
        {
            // 持续朝向锁定目标
            player.LookAtLockTarget();
        }
        
    }

    protected override void OnLeave(IFsm<PlayerState> fsm, bool isShutdown)
    {
        UnRegisterEvent();
        player.SetRootMotionOffset("null");
    }
    
    protected override void OnDestroy(IFsm<PlayerState> fsm)
    {
    }


    public void CheckForFalling()
    {
        if (!player.isGrounded)
        {
            SwitchToFall();
        }
    }
    
    /// <summary>
    /// 注册事件函数，统一管理事件的订阅和注销，使得生命周期简洁易于管理。
    /// </summary>
    private void RegisterEvent()
    {
        player.inputAction.MoveAction.performed += MovePerformed;
        player.inputAction.MoveAction.canceled += MoveCanceled;
        
        player.inputAction.LeftShift.performed += LeftShiftPerformed;
        player.inputAction.LeftShift.canceled += LeftShiftCanceled;
        
        player.inputAction.Crouch.performed += CrouchPerformed;
        
        player.inputAction.Space.performed += SpacePerformed;
        
        player.inputAction.Jump.performed += JumpPerformed;

        player.inputAction.Attack.performed += AttackPerformed;

        player.inputAction.HeavyAttack.performed += HeavyAttackPerformed;

        player.inputAction.HeavyAttack.canceled += HeavyAttackCanceled;

        player.inputAction.Skill.performed += SkillPerformed;

        player.inputAction.Skill.canceled += Skillcanceled;

        // player.playableDirector.stopped += PDStopped;
    }

    private void UnRegisterEvent()
    {
        player.inputAction.MoveAction.performed -= MovePerformed;
        player.inputAction.MoveAction.canceled -= MoveCanceled;
        
        player.inputAction.LeftShift.performed -= LeftShiftPerformed;
        player.inputAction.LeftShift.canceled -= LeftShiftCanceled;
        
        player.inputAction.Crouch.performed -= CrouchPerformed;
        
        player.inputAction.Space.performed -= SpacePerformed;
        
        player.inputAction.Jump.performed -= JumpPerformed;

        player.inputAction.Attack.performed -= AttackPerformed;

        player.inputAction.HeavyAttack.performed -= HeavyAttackPerformed;

        player.inputAction.HeavyAttack.canceled -= HeavyAttackCanceled;

        player.inputAction.Skill.performed -= SkillPerformed;
        
        player.inputAction.Skill.canceled -= SkillPerformed;
        // player.playableDirector.stopped -= PDStopped;
    }
    
    //重写按键虚方法来让具体子状态决定可以被哪个操作中断，以及自定义中断操作。

    public virtual void MovePerformed(InputAction.CallbackContext context)
    {
    }

    public virtual void MoveCanceled(InputAction.CallbackContext context)
    {
    }

    public virtual void LeftShiftPerformed(InputAction.CallbackContext context)
    {
    }

    public virtual void LeftShiftCanceled(InputAction.CallbackContext context)
    {
    }

    public virtual void CrouchPerformed(InputAction.CallbackContext context)
    {
    }

    public virtual void SpacePerformed(InputAction.CallbackContext context)
    {
        SwitchToDodgeRoll();
    }

    public virtual void JumpPerformed(InputAction.CallbackContext context)
    {
        if (player.isGrounded)
            SwitchToJump();
    }

    public virtual void AttackPerformed(InputAction.CallbackContext context)
    {
        
    }

    public virtual void HeavyAttackPerformed(InputAction.CallbackContext context)
    {
        
    }

    public virtual void HeavyAttackCanceled(InputAction.CallbackContext context)
    {
        
    }

    public virtual void SkillPerformed(InputAction.CallbackContext context)
    {

    }

    public virtual void Skillcanceled(InputAction.CallbackContext context)
    {
        
    }
    
    /// <summary>
    /// 这个函数较为特殊，实质是PlayableDirector的stopped回调函数，用于连接过渡动作到本状态的循环动作。一般自定义都是本状态的对应动画播放。
    /// </summary>
    /// <param name="director"></param>
    public virtual void ConnectedToLoopingAni(PlayableDirector director)
    {
        //Debug.Log("Connected to looping ani");
    }

    
    //常用的切换函数，方便使用。可以自定义用于确定需要播放哪些Stopped动画。
    
    public virtual void SwitchToMove()
    {
        ChangeState<MoveState>(player.playerStateFsm);
        if (player.CheckLockTarget())
        {
            PlayDirector(player.GetResearchMoveTimeline());
        }
        else
        {
            PlayDirector(player.locomotionData._loopActionData.MoveMixerTimeline);
        }
    }
    
    public virtual void SwitchToIdle()
    {
        ChangeState<IdleState>(player.playerStateFsm);
        //怎么切换自定义
    }

    public virtual void SwitchToRush()
    {
        ChangeState<RushState>(player.playerStateFsm);;
    }

    public virtual void SwitchToCrouchMove()
    {
        ChangeState<CrouchMoveState>(player.playerStateFsm);
        PlayDirector(player.locomotionData._loopActionData._CrouchMoveTimeline);
    }

    public virtual void SwitchToCrouchIdle()
    {
        ChangeState<CrouchIdleState>(player.playerStateFsm);
    }

    public virtual void SwitchToDodgeRoll()
    {
        ChangeState<DodgeState>(player.playerStateFsm);
        if (player.CheckLockTarget())
        {
            PlayDirector(player.GetResearchDodgeRollTimeline());
        }
        else
        {
            player.UpdateRotationImmediately(player.HorizontalInput);
            PlayDirector(player.locomotionData._startActionData._DodgeRoll);
        }
    }

    public virtual void SwitchToDodgeBack()
    {
        ChangeState<DodgeState>(player.playerStateFsm);
        if (player.CheckLockTarget())
        {
            PlayDirector(player.GetResearchDodgeRollTimeline());
        }
        else
        {
            PlayDirector(player.locomotionData._startActionData._DodgeBack);
        }
    }

    public virtual void SwitchToJump()
    {
        ChangeState<JumpState>(player.playerStateFsm);

        if (player.HorizontalInput != Vector3.zero)
        {
            player.UpdateRotationImmediately(player.HorizontalInput);
        }
        PlayDirector(player.locomotionData._loopActionData._JumpTimeline);
    }

    public virtual void SwitchToFall()
    {
        ChangeState<FallState>(player.playerStateFsm);
        PlayDirector(player.locomotionData._loopActionData._FallTimeline);
    }

    public virtual void SwitchToAttack()
    {
        ChangeState<AttackState>(player.playerStateFsm);
    }

    public virtual void SwitchToSkill()
    {
        ChangeState<SkillState>(player.playerStateFsm);
    }

    public virtual void InterruptSwitchWeapon()
    {
    //    Debug.Log("InterruptSwitchWeapon");
        player._weaponManager._isSwitching = false;
        player.layerManger._BeHalfLayer.Stop();
        player.layerManger._BeHalfLayer.StartFade(0, 0.1f);
    }

    #region 播放方式

    public void PlayLeftOrRight(TimelineAsset left, TimelineAsset right)
    {
        if (player.isLeftObjAhead(player._LeftAngle.transform.position, player._RightAngle.transform.position, player.transform))
        {
            PlayDirector(left);
        }
        else
        {
            PlayDirector(right);
        }
    }
    
    public void PlayDirector(TimelineAsset timeline) => player.PD(timeline);

    public void ConnectPlayDirector(TimelineAsset timeline)
    {
        if (player.playableDirector.playableAsset != timeline)
            PlayDirector(timeline);
    }

    #endregion
}
