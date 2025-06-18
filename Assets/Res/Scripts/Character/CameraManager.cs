using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSigleton<CameraManager>
{
    [SerializeField] private GameObject _PlayerCameraPrefab;
    [SerializeField] private GameObject _ResearchCameraPrefab;

    public CinemachineVirtualCamera GetPlayerCamera() 
    {
        var playerCamera = Instantiate(_PlayerCameraPrefab, transform);
        return playerCamera.GetComponent<CinemachineVirtualCamera>();
    }

    public CinemachineVirtualCamera GetResearchCamera()
    {
        var researchCamera = Instantiate(_ResearchCameraPrefab, transform);
        return researchCamera.GetComponent<CinemachineVirtualCamera>();
    }

    
}
