using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraInternalMan
{
    private const int _startCameraPriority = 10;
    private const int _closeCameraPriority = 0;
    public CinemachineVirtualCamera _playerCamera;
    public CinemachineVirtualCamera _researchCamera;
    private CinemachineVirtualCamera _currentCamera;
    private PlayerState _playerState;

    private CinemachineTargetGroup _targetGroup;

    public CameraInternalMan(CinemachineVirtualCamera playerCamera, CinemachineVirtualCamera researchCamera, PlayerState playerState)
    {
        _playerCamera = playerCamera;
        _researchCamera = researchCamera;
        _playerState = playerState;
       
        InitCamera();
    }

    private void InitCamera()
    {
        _playerCamera.Priority = _closeCameraPriority;
        _researchCamera.Priority = _closeCameraPriority;
        _targetGroup = _researchCamera.GetComponentInChildren<CinemachineTargetGroup>();

        SetLookAtANDAim(_playerCamera, _playerState._PlayerLockTarget, _playerState._PlayerLockTarget);
        _playerCamera.Priority = _startCameraPriority;
        _currentCamera = _playerCamera;
    }

    public void TransitionCamera(CinemachineVirtualCamera camera)
    {
        _currentCamera.Priority = _closeCameraPriority;
        camera.Priority = _startCameraPriority;
        _currentCamera = camera;
    }

    public void TransitionCamera(CinemachineVirtualCamera camera, GameObject lookAt, GameObject aim)
    {
        _currentCamera.Priority = _closeCameraPriority;
        camera.Priority = _startCameraPriority;
        if (camera == _researchCamera)
        {
            SetResearchTarget(lookAt, aim);
        }
        else
        {
            SetLookAtANDAim(camera, lookAt, aim);
        }
        _currentCamera = camera;
    }

    private void SetLookAtANDAim(CinemachineVirtualCamera camera, GameObject lookAt, GameObject aim)
    {
        camera.LookAt = lookAt.transform;
        camera.Follow = aim.transform;
    }

    private void SetResearchTarget(GameObject lookat, GameObject aim)
    {
        if (_targetGroup == null || _targetGroup.m_Targets == null || _targetGroup.m_Targets.Length < 2)
            return;

        if (_targetGroup.m_Targets[0].target == null || _targetGroup.m_Targets[0].target != lookat.transform)
        {
            _targetGroup.m_Targets[0].target = lookat.transform;
        }
        if (_targetGroup.m_Targets[1].target == null || _targetGroup.m_Targets[1].target != aim.transform)
        {
            _targetGroup.m_Targets[1].target = aim.transform;
        }
        _researchCamera.LookAt = lookat.transform;
        _researchCamera.Follow = _targetGroup.transform;
    }
}