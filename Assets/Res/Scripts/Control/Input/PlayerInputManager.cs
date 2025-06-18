using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager
{
    private Player playerInput;
    public Player.PlayerCtxActions inputAction;
    
    public PlayerInputManager()
    {
        playerInput = new Player();
        inputAction = playerInput.PlayerCtx;
        EnableInput();
    }
    
    public void EnableInput() => inputAction.Enable();
    public void DisableInput() => inputAction.Disable();

    public InputAction MoveAction => inputAction.Move;
    public InputAction Space => inputAction.Space;
    public InputAction LeftMouse => inputAction.LeftMouse;
    public InputAction LeftShift => inputAction.LeftShift;
    public InputAction Crouch => inputAction.Crouch;
    public InputAction Jump => inputAction.Jump;
    public InputAction Attack => inputAction.Attack;
    public InputAction SwitchWeapon => inputAction.SwitchWeapon;
    public InputAction HeavyAttack => inputAction.HeavyAttack;
    public InputAction Skill => inputAction.Skill;
    public InputAction Search => inputAction.Search;

}
