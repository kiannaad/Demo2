using System.Collections;
using System.Collections.Generic;
using System.Threading;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public enum WeaponType
{
    Null,
    Common,
    Katana,
    GreatWord
}

public class WeaponManager
{
    private WeaponsData _weaponsData;
    private Dictionary<WeaponType, WeaponData> _weaponDic;
    
    private GameObject _bindingPosition;

    private List<WeaponType> _WeaponList;
    public int _currentIndex = 0;
    private int _MaxWeaponCapsity;

    private PlayableDirector _director;
    private PlayerState _playerState;

    public bool _isSwitching;

    #region 初始化

    public WeaponManager(PlayerState playerState, int MaxWeaponCapsity, List<WeaponType> _weaponList)
    {
        this._bindingPosition = playerState._BindingWeaponPos;
        this._weaponsData = playerState.weaponsData;
        this._MaxWeaponCapsity = MaxWeaponCapsity;
        this._director = playerState.playableDirector;
        this._playerState = playerState;
        this._WeaponList = _weaponList;
        Init();
    }

    private void Init()
    {
        _weaponDic = new Dictionary<WeaponType, WeaponData>();
        
        _currentIndex = 0;
        
        foreach (var weaponData in _weaponsData._Weapons)
        {
           // Debug.Log(weaponData._WeaponType);
            _weaponDic.TryAdd(weaponData._WeaponType, weaponData);
        }

        if (GameEntry.Event ==  null) Debug.Log($"Event is null");
        GameEntry.Event.Subscribe(ChangeStateEventArg.EventId, ConnectToEquip);
        GameEntry.Event.Subscribe(ChangeStateEventArg.EventId, ConnectToCurrentAni);

        if (_playerState == null) Debug.LogError($"Input action is null");
        _playerState.inputAction.SwitchWeapon.performed += SwitchToNextSweapon;

        var weapon = GetCurrentWeaponData();
        _playerState.InstantiateSweapon(weapon._WeaponPrefab, weapon._LocalPosition, weapon._LocalRotation, weapon._LocalScale);
    }

    #endregion

    public WeaponData GetWeaponData(WeaponType _weaponType)
    {
         _weaponDic.TryGetValue(_weaponType, out var data);
         
         if (data == null)
         {
             Debug.LogError("GetWeaponData Error : data is null");
             return null;
         }
         
         return data;
    }

    public WeaponData GetCurrentWeaponData() => _weaponDic[_WeaponList[_currentIndex]];

    public void CustomlizeWeaponList(List<WeaponType> weaponList) => _WeaponList = weaponList;

    public void SwitchToNextSweapon(InputAction.CallbackContext context)
    {
        if (_isSwitching) return;
        
        var curWeapon = GetWeaponData(_WeaponList[_currentIndex]);
        
        _director.Play(curWeapon._UnArmedTimeline);
        _isSwitching = true;

    }

    private void ConnectToEquip(object sender, GameEventArgs args)
    {
        var arg = (ChangeStateEventArg)args;
        if (arg == null || arg.targetStateName != "UnArmed") return;

        if (arg.isEnd)
        {
            //Debug.Log("ConnectToEquip End");
            var nextWeapon = CheckForNextWeapon();
            _director.Play(nextWeapon._EquipTimeline);
        }
    }

    private void ConnectToCurrentAni(object sender, GameEventArgs args)
    {
        var arg = (ChangeStateEventArg)args;
        if (arg == null || arg.targetStateName != "Equip") return;

        if (!arg.isEnd)
        {
            var weapon = GetCurrentWeaponData();
            _playerState.InstantiateSweapon(weapon._WeaponPrefab, weapon._LocalPosition, weapon._LocalRotation, weapon._LocalScale);
        }

        if (arg.isEnd)
        {
            _playerState._NeedRefresh = true;
            _isSwitching = false;
        }
    }
    

    private WeaponData CheckForNextWeapon()
    {
        IncreaseCurrentIndex();
        while (_WeaponList[_currentIndex] == WeaponType.Null)
        {
            IncreaseCurrentIndex();
        }
        return  GetWeaponData(_WeaponList[_currentIndex]);
    }
    
    private void IncreaseCurrentIndex() => _currentIndex = (_currentIndex + 1) % _WeaponList.Count;
}
