using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Cinemachine;
using GameFramework.Fsm;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityGameFramework.Runtime;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhysicsDetection))]
public class PlayerState : EntityLogic
{
    #region 组件
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera cam;
    public CharacterController _CharacterController;
    private PhysicsDetection _terrainDetection;

    #endregion
    
    public IFsm<PlayerState> playerStateFsm { get; private set; }

    #region 动画相关字段
    
    public LayerManger layerManger;
    
    private Animator animator;
    
    public AnimancerComponent animancer { get; set; }
    
    [SerializeField] private AniDataSO aniData;

    [SerializeField] private StringAsset _movementString;
    
    public LocomotionData locomotionData => _weaponManager.GetCurrentWeaponData()._MoveInfo._locomotionData;
    public MovementData movementData => _weaponManager.GetCurrentWeaponData()._MoveInfo._movementData;
    
    public WeaponsData  weaponsData => aniData._weaponsData;

    public float MovementParameter
    {
        get => animancer.Parameters.GetOrCreate<float>(_movementString);
        set => animancer.Parameters.SetValue(_movementString, value);
    }

    [HideInInspector] public bool _NeedRefresh;
    #endregion

    public Transform _LeftAngle;
    public Transform _RightAngle;
    public GameObject _BindingWeaponPos;
    
    [HideInInspector] public bool _isInAirAttacking = false;

    public List<WeaponType> _customsizeSweaponList;
    
    public bool isGrounded => _terrainDetection.IsGrounded;

    [HideInInspector] public float curSmoothTime;
    [HideInInspector] public float curRotateSpeed;
    [HideInInspector] public bool CanUseRootMotion = true;

    public WeaponManager _weaponManager;
    public CameraInternalMan _cameraInternalMan;
    public PhysicsManager _physicsManager;

    #region 输入模块设置

    public PlayerInputManager inputAction { get; private set; }
    public Vector3 HorizontalInput => new Vector3(inputAction.MoveAction.ReadValue<Vector2>().x, 0f, inputAction.MoveAction.ReadValue<Vector2>().y);

    #endregion

    #region 根位移设置

    
    public void SetRootMotionOffsetZero()
    {
        _physicsManager.ExternalVelocity = Vector3.zero;
    }
    
    private void OnAnimatorMove()
    {
        if (!CanUseRootMotion) return;

        _physicsManager.RootMotionVelocity = animator.velocity;

        transform.rotation *= animator.deltaRotation;
    }
    
    public void SetRootMotionOffset(string _StateName)
    {
        _physicsManager.ExternalVelocity = _StateName switch
        {
            "RunJump" => movementData.RunJumpRootOffset * transform.forward + new Vector3(0f, movementData.JumpHightRootOffset, 0f),
            "QuickJump" => movementData.QuickRunJumpRootOffset * transform.forward + new Vector3(0f, movementData.JumpHightRootOffset, 0f),
            "Fall" => new Vector3(_physicsManager.CalculateVelocity().x, movementData.FallSpeedRootOffset, _physicsManager.CalculateVelocity().z),
            _ => Vector3.zero
        };
    }

    #endregion

    #region 生命周期函数

    private void Awake()
    {
        inputAction = new PlayerInputManager();
        playableDirector = GetComponent<PlayableDirector>();
        animancer = GetComponent<AnimancerComponent>();
        animator = GetComponent<Animator>();
        layerManger.Init(animancer);
        _CharacterController = GetComponent<CharacterController>();
        _terrainDetection = GetComponent<PhysicsDetection>();
        cam = CameraManager.Instance.GetPlayerCamera();
        _cameraInternalMan = new CameraInternalMan(cam, CameraManager.Instance.GetResearchCamera(), this);
        _physicsManager = new PhysicsManager(_CharacterController, false);
    }
    
    private void Start()
    {
        _weaponManager = new WeaponManager(this,  3, 
            _customsizeSweaponList);
        
        if (GameEntry.Fsm == null) Debug.LogError("GameEntry.Fsm is null.");
        playerStateFsm = GameEntry.Fsm.CreateFsm<PlayerState>("PlayerFsm", this,
            new List<FsmState<PlayerState>>{new IdleState(), new MoveState(), new RushState(), 
                new CrouchIdleState(), new CrouchMoveState(), new DodgeState(), new JumpState(), new FallState(), new AttackState()
                ,new HeavyAttackState(), new SkillState()
            });
        playerStateFsm.Start<IdleState>();

        inputAction.Search.performed += Search;
        /*inputAction.HeavyAttack.started += context => Debug.Log("HeavyAttack Started"); 
        inputAction.HeavyAttack.performed += context => Debug.Log("HeavyAttack Performed");
        inputAction.HeavyAttack.canceled += context => Debug.Log("HeavyAttack Canceled");*/
    }

    private void Update()
    { 
        //Debug.Log($"Velocity: {_physicsManager.CalculateVelocity()}");
        //Debug.Log($"HorizontalInput: {HorizontalInput}");
        //Debug.Log($"_lockTarget: {_lockTarget != null}");
        //Debug.Log($"Skill Button : {inputAction.Skill.ReadValue<float>()}");
        //Debug.Log($"_isInAirAttacking: {_isInAirAttacking}");
        //Debug.Log($"isGrounded : {isGrounded}");
        // Debug.Log($"duration : {playableDirector.duration}");
        // Debug.Log($"currentTime : {playableDirector.time}");
        //Debug.Log($"YPosition{transform.position.y}");
        //Debug.Log($"velocity : {_CharacterController.velocity}");
        //Debug.Log($"MovementPar : {MovementParameter}");
    }

    #endregion

     #region Rotation
     /// <summary>
    /// 更新人物角度
    /// </summary>
    /// 旋转的时间
    /// <param name="smoothTime"></param>
    public void UpdateRotation(Vector3 input, float smoothTime, float rotateVelocity)
    {
        if (input == Vector3.zero) return;
        
        var Targetrotation = GetInputRotation(input);
        
        float RotateToTarget = Mathf.SmoothDampAngle(transform.eulerAngles.y, Targetrotation, ref rotateVelocity, smoothTime);
        
        transform.rotation = Quaternion.Euler(0, RotateToTarget, 0);
    }

    public void UpdateRotationToward(float smoothTime, float rotateVelocity, GameObject towardObject)
    {
        Vector3 direction = towardObject.transform.position - transform.position;
        direction.y = 0f; // 保持水平旋转
        direction.Normalize(); // 标准化方向向量
            
        var Targetrotation = GetInputRotation(direction, true);

        var RotateToTarget = Mathf.SmoothDampAngle(transform.eulerAngles.y, Targetrotation, ref rotateVelocity, smoothTime);
        
        transform.rotation = Quaternion.Euler(0, RotateToTarget, 0);
    }

    public void UpdateRotationImmediately(Vector3 input, bool isToward = false)
    {
        float Targetrotation = GetInputRotation(input, isToward);
        transform.rotation = Quaternion.Euler(0, Targetrotation, 0);
    }

    /// <summary>
    /// 更新输入后需要旋转的角度
    /// </summary>
    /// 输入值
    /// <param name="input"></param>
    /// 是否是面向某个物体
    /// <param name="isToward"></param>
    /// <returns></returns>
    private float GetInputRotation(Vector3 input, bool isToward = false)
    {
        float rotation = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;

        if (rotation < 0)
        {
            rotation += 360;
        }
        
        if (!isToward)
            AddCameraRotation(ref rotation);
        
        return rotation;
    }

    private void UpdateRotationInResearch(Vector2 input)
    {
        float rotation = GetInputRotationInResearch(input);
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    private float GetInputRotationInResearch(Vector2 input)
    {
        if (!CheckLockTarget())
        {
            // 非索敌状态下的普通旋转计算
            Vector3 worldInput = new Vector3(input.x, 0, input.y);
            float rotation = Mathf.Atan2(worldInput.x, worldInput.z) * Mathf.Rad2Deg;
            
            if (rotation < 0)
            {
                rotation += 360;
            }
            
            rotation += cam.transform.eulerAngles.y;
            
            if (rotation > 360)
            {
                rotation -= 360;
            }
            
            return rotation;
        }
        else
        {
            // 索敌状态下的旋转计算
            Vector3 toEnemy = _lockTarget.transform.position - transform.position;
            toEnemy.y = 0;
            toEnemy.Normalize();
            
            // 计算敌人方向的角度
            float enemyAngle = Mathf.Atan2(toEnemy.x, toEnemy.z) * Mathf.Rad2Deg;
            if (enemyAngle < 0)
            {
                enemyAngle += 360;
            }
            
            // 根据输入方向调整角度
            if (input.x != 0 || input.y != 0)
            {
                // 计算输入方向相对于敌人方向的角度
                float inputAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
                if (inputAngle < 0)
                {
                    inputAngle += 360;
                }
                
                // 将输入角度添加到敌人角度上
                enemyAngle += inputAngle;
                
                // 确保角度在0-360范围内
                if (enemyAngle > 360)
                {
                    enemyAngle -= 360;
                }
            }
            
            return enemyAngle;
        }
    }

    /// <summary>
    /// 添加摄像机的旋转
    /// </summary>
    /// <param name="rotation"></param>
    private void AddCameraRotation(ref float rotation)
    {
        float cameraRotation = cam.transform.eulerAngles.y;
        rotation += cameraRotation;
        if (rotation > 360)
        {
            rotation -= 360;
        }
    }

    private Vector3 GetInputDirection()
    {
        Quaternion rotation = Quaternion.Euler(0, GetInputRotation(HorizontalInput), 0);
        return  rotation * Vector3.forward;
    }
    

    #endregion

    #region TimeLine播放设置以及相关动画参数调整

    public bool isLeftObjAhead(Vector3 leftObj, Vector3 rightObj, Transform character)
    {
        // 获取角色面朝方向的单位向量
        Vector3 forward = character.forward;
        
        // 计算两脚相对于角色位置的向量
        Vector3 leftRelative = leftObj - character.position;
        Vector3 rightRelative = rightObj - character.position;

        // 计算脚向量在角色前方向上的投影长度（点积）
        float leftProjection = Vector3.Dot(leftRelative, forward);
        float rightProjection = Vector3.Dot(rightRelative, forward);

        // 比较投影长度，值大的更靠前
        return (leftProjection > rightProjection) ? true : false;
    }

    public void PD(TimelineAsset timeline) => playableDirector.Play(timeline);

    public void UpdateRotationgPar(float smoothTime, float rotateSpeed)
    {
        curRotateSpeed =  rotateSpeed;
        curSmoothTime = smoothTime;
    }

    [SerializeField] private float currentVelocity;
    public void UpdateMovementPar(float targetSpeed, float smoothTime)
    {
        var curSpeed = MovementParameter;
        var nextSpeed = Mathf.SmoothDamp(curSpeed, targetSpeed, ref currentVelocity, smoothTime);
        MovementParameter = nextSpeed;
    }

    public void SetMovementParZero() => MovementParameter = 0f;

    public TimelineAsset GetResearchMoveTimeline()
    {
        var input = GetNormalizeInput();

        if (input == new Vector2(0, 1))
            return locomotionData._MoveSearchTimeline[0];
        else if (input == new Vector2(0, -1))
            return locomotionData._MoveSearchTimeline[1];
        else if (input == new Vector2(-1, 0))
            return locomotionData._MoveSearchTimeline[2];
        else if (input == new Vector2(1, 0))
            return locomotionData._MoveSearchTimeline[3];
        else if (input == new Vector2(-1, 1))
            return locomotionData._MoveSearchTimeline[4];
        else if (input == new Vector2(1, 1))
            return locomotionData._MoveSearchTimeline[5];
        else if (input == new Vector2(-1, -1))
            return locomotionData._MoveSearchTimeline[6];
        else if (input == new Vector2(1, -1))
            return locomotionData._MoveSearchTimeline[7];
        else
            return null;
    }

    private Vector2 GetNormalizeInput()
    {
        float xinput = HorizontalInput.x;
        if (xinput > 0) xinput = 1;
        else if (xinput < 0) xinput = -1;
        else xinput = 0;

        float yinput = HorizontalInput.z;
        if (yinput > 0) yinput = 1;
        else if (yinput < 0) yinput = -1;
        else yinput = 0;

        return new Vector2(xinput, yinput);
    }

    public TimelineAsset GetResearchDodgeRollTimeline()
    {
        var input = GetNormalizeInput();

        if (input == new Vector2(0, 1))
            return locomotionData._DodgeRollSearchTimeline[0];
        else if (input == new Vector2(0, -1))
            return locomotionData._DodgeRollSearchTimeline[1];
        else if (input == new Vector2(-1, 0))
            return locomotionData._DodgeRollSearchTimeline[2];
        else if (input == new Vector2(1, 0))
            return locomotionData._DodgeRollSearchTimeline[3];
        else if (input == new Vector2(-1, 1))
        {
            UpdateRotationInResearch(new Vector2(-1, 1));
            return locomotionData._DodgeRollSearchTimeline[0];
        }
        else if (input == new Vector2(1, 1))
        {
            UpdateRotationInResearch(new Vector2(1, 1));
            return locomotionData._DodgeRollSearchTimeline[0];
        }
        else if (input == new Vector2(1, -1))
        {
            UpdateRotationInResearch(new Vector2(-1, 1)); 
            return locomotionData._DodgeRollSearchTimeline[1];
        }
        else if (input == new Vector2(-1, -1))
        {
            UpdateRotationInResearch(new Vector2(1, 1));
            return locomotionData._DodgeRollSearchTimeline[1];
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region 连招Timeline的获取

    private int comboCount = 0;

    public List<WeaponComboData> ComboData => _weaponManager.GetCurrentWeaponData()._ComboInfo._ComboData;

    public HeavyAttackData HeavyAttackData =>
        _weaponManager.GetCurrentWeaponData()._ComboInfo._HeavyAttackData;

    public SkillData SkillData => _weaponManager.GetCurrentWeaponData()._ComboInfo._skillData;

    public TimelineAsset GetCurrentLightComboTimeline()
    {
        if (comboCount < 0 || comboCount >= ComboData.Count)
        {
            Debug.LogError("comboCount is too high or too low.");
            return null;
        }
        
        var targetTimeline =  ComboData[comboCount]._Timeline;
        IncreaseComboCount();

        if (targetTimeline == null)
        {
            Debug.LogError("targetTimeline is null");
            return null;
        }
        
        return targetTimeline;
    }
    
    private void IncreaseComboCount() => comboCount =  (comboCount + 1) % ComboData.Count;
    
    public void ResetComboCount() => comboCount = 0;

    public void InstantiateSweapon(GameObject _weaponPrefab, Vector3 _LocalPosition, Vector3 _LocalRotation, Vector3 _LocalScale)
    {
        var transforms = _BindingWeaponPos.GetComponentsInChildren<Transform>();
        foreach (var t in transforms)
        {
            if (t == _BindingWeaponPos.transform) continue;
            Destroy(t.gameObject);
            
        }
        
        var InsWeapon = Instantiate(_weaponPrefab, _BindingWeaponPos.transform);
        InsWeapon.transform.localPosition = _LocalPosition;
        InsWeapon.transform.localRotation = Quaternion.Euler(_LocalRotation);
        if (_LocalScale == Vector3.zero) _LocalScale = Vector3.one;
        InsWeapon.transform.localScale = _LocalScale;
    }

    #endregion

    #region 索敌功能

    // 锁定相关字段
     private GameObject _lockTarget;
     public float lockMaxDistance = 20f; // 最大锁定距离
     public float lockScreenRange = 0.2f; // 屏幕中心范围（归一化，0.2约为20%屏幕宽高）
     private bool _isLocking = false;
     public GameObject _PlayerLockTarget;
     public bool CheckLockTarget() => _isLocking && _lockTarget != null;
     public bool CheckLocking() => _isLocking;

        private void Search(InputAction.CallbackContext context)
        {
            if (_isLocking)
            {
                // 再次点击中键取消锁定
                CancelLock();
            }
            else
            {
                TryLockTarget();
            }
        }
        
        // 锁定目标检测
        private void TryLockTarget()
        {
            Camera mainCam = Camera.main;
            if (mainCam == null) return;
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = mainCam.ScreenPointToRay(screenCenter);
            Collider[] hits = Physics.OverlapSphere(transform.position, lockMaxDistance);
            GameObject bestTarget = null;
            float bestScore = float.MaxValue;
            
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    SearchEnemy(mainCam, ref bestTarget, ref bestScore, hit);
                }
            }
            if (bestTarget != null)
            {
                if (bestTarget.GetComponent<GetLookTarget>() == null)
                {
                    _lockTarget = bestTarget;
                    Debug.Log("No GetLookTarget Component : " + _lockTarget.name);
                }
                else
                {
                    _lockTarget = bestTarget.GetComponent<GetLookTarget>().GetEnemyLookTarget();
                }
                _isLocking = true;
            }
            else
            {
                CancelLock();
            }
        }
    
        private void SearchEnemy(Camera mainCam, ref GameObject bestTarget, ref float bestScore, Collider hit)
        {
            Vector3 enemyScreenPos = mainCam.WorldToViewportPoint(hit.transform.position);
            float centerDist = Vector2.Distance(new Vector2(0.5f, 0.5f), new Vector2(enemyScreenPos.x, enemyScreenPos.y));
            float worldDist = Vector3.Distance(transform.position, hit.transform.position);
            if (enemyScreenPos.z > 0 && centerDist < lockScreenRange && worldDist < lockMaxDistance)
            {
                float score = centerDist * 0.7f + worldDist / lockMaxDistance * 0.3f; // 屏幕中心优先，距离次之
                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = hit.gameObject;
                }
            }
        }
    
        // 人物和摄像机朝向锁定目标
        public void LookAtLockTarget()
        {
            //Debug.Log("LookAtLockTarget");
            if (_lockTarget == null || !_isLocking)
            {
                CancelLock();
                return;
            }
    
            // 人物旋转
            UpdateRotationToward(0.1f, 0f, _lockTarget);
    
            // 摄像机旋转
            SetCameraLookAt(_lockTarget);
        
            // 若目标死亡或超出距离，自动解锁
            float dist = Vector3.Distance(transform.position, _lockTarget.transform.position);
            if (dist > lockMaxDistance || !_lockTarget.activeInHierarchy)
            {
                CancelLock();
            }
        }
    
        private void SetCameraLookAt(GameObject target)
        {
           _cameraInternalMan.TransitionCamera(_cameraInternalMan._researchCamera, target, gameObject);
        }
    
        private void SetCameraLookAt() 
        {
            _cameraInternalMan.TransitionCamera(_cameraInternalMan._playerCamera, _PlayerLockTarget, _PlayerLockTarget);
        }
    
        private void CancelLock()
        {
            _isLocking = false;
            _lockTarget = null;
            SetCameraLookAt();
        }
    #endregion
}
