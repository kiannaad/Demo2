using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime;
using UnityGameFramework.Runtime;

public class Enemy : EntityLogic
{
    private CharacterController _characterController;
    private Animator _animator;
    private PhysicsManager _physicsManager;
    private BehaviorTree _behaviourTree;
    private GameObject _player;
    private bool _canLookPlayer = true;
    [SerializeField] private float _lookSpeed = 0.1f;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _physicsManager = new PhysicsManager(_characterController);
        _behaviourTree = GetComponent<BehaviorTree>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private float _lookVelocity;
    void OnAnimatorMove()
    {
        _physicsManager.RootMotionVelocity = _animator.velocity;

        transform.rotation *= _animator.deltaRotation;

        if (_canLookPlayer)
        {
            float angle = Mathf.SmoothDampAngle(GetCurrentAngle(), GetPlayerRotation(), ref _lookVelocity, _lookSpeed);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
        }
    }

    private float GetCurrentAngle() 
    {
        float angle = transform.rotation.eulerAngles.y;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    private Vector3 GetPlayerDirection() => (_player.transform.position - transform.position).normalized;
    private float GetPlayerRotation()
    {
        float angle = Quaternion.LookRotation(GetPlayerDirection()).eulerAngles.y;
        if (angle < 0f) angle += 360f;
        return angle;
    }
}
