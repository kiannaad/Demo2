using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhysicsType
{
   OnlyRootMotion,
   OnlyExternal,
   Insert
}

public class PhysicsManager
{
    private CharacterController _characterController;
    public bool _useSimpleMove;

    public PhysicsManager(CharacterController characterController, bool useSimpleMove = true)
    {
        _characterController = characterController;
        _rootMotionVelocity = Vector3.zero;
        _externalVelocity = Vector3.zero;
        _physicsType = PhysicsType.Insert;
        _useSimpleMove = useSimpleMove;
    }

    private Vector3 _rootMotionVelocity;
    public Vector3 RootMotionVelocity
    {
        get => _rootMotionVelocity;
        set
        {
            if (value == _rootMotionVelocity) return;
            _rootMotionVelocity = value;
            SetVelocity();
        }
    }

    private Vector3 _externalVelocity;
    public Vector3 ExternalVelocity
    {
        get => _externalVelocity;
        set
        {
            if (value == _externalVelocity) return;
            _externalVelocity = value;
            SetVelocity();
        }
    }

    private PhysicsType _physicsType;
    public void SetPhysicsType(PhysicsType physicsType) => _physicsType = physicsType;
    
    private void SetVelocity()
    {
        if (_useSimpleMove)
        {
            _characterController.SimpleMove(CalculateVelocity());
        }
        else
        {
            _characterController.Move(CalculateVelocity() * Time.deltaTime);
        }
    }


    public Vector3 CalculateVelocity()
    {
        switch (_physicsType)
        {
            case PhysicsType.OnlyRootMotion:
                return RootMotionVelocity;
            case PhysicsType.OnlyExternal:
                return ExternalVelocity;
            case PhysicsType.Insert:
                return RootMotionVelocity + ExternalVelocity;
            default:
                return Vector3.zero;
        }
    }

}
