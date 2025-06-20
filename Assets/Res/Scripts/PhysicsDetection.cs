using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PhysicsDetection : MonoBehaviour
{
   #region 地面检测
      [Header("地面盒型射线检测设置")]
      [SerializeField] private Vector3 _Bounds;
      [SerializeField] private Vector3 _Center;
      [SerializeField] private Vector3 _StandDirection;
      [SerializeField] private float _RayMaxDistance;
      [SerializeField] private Vector3 _RayDectedDirection = Vector3.down;
      [SerializeField] private float _CanStandAgree;
      
      private CharacterController _characterController;
      
      [SerializeField] LayerMask _GroundMask;
      
      private Vector3 _DefaultNormalDirection = Vector3.up;
   
      private float Height_Difference;
   
      private float height = 0;
   
      private float _curHeight
      {
         get => height;
         set
         {
            if (height - value <= 1 || height - value >= -1)
            {
               Height_Difference = 0f;
               return;
            }
   
            Height_Difference = height - value;
            height = value;
         }
      }
      
      public float GetHeightOffset() => Height_Difference;
   
      public bool IsGrounded => CheckIsGrounded();
   #endregion

   #region 武器攻击检测
      // 武器胶囊检测相关字段
      [SerializeField] private GameObject _weaponObject;
      private Vector3 _capsulePoint1, _capsulePoint2;
      [SerializeField] private float _capsuleRadius;
      private Action<Collider> _onWeaponHit;
      [SerializeField] private LayerMask _WeaponHitMask;
   
      private void Awake()
      {
         _characterController = GetComponent<CharacterController>();
         
         _StandDirection =  _DefaultNormalDirection;
         _Bounds /= 2f;
      }
   
      public void SetWeaponObject(GameObject weaponObject) => _weaponObject = weaponObject;
   
       private bool CheckIsGrounded()
      {
         RaycastHit[] hits = Physics.BoxCastAll(transform.position - _Center, _Bounds, _RayDectedDirection, Quaternion.identity, _RayMaxDistance,
            _GroundMask);
   
         foreach (RaycastHit hit in hits)
         {
            if (Vector3.Angle(_StandDirection, hit.normal) < _CanStandAgree)
            {
               _curHeight = hit.point.y;
               return true;
            }
         }
         
         return false;
      }
      
      // 注册武器命中回调
      public void RegisterWeaponHitCallback(Action<Collider> callback)
      {
         _onWeaponHit = callback;
      }
   
      // 计算并更新胶囊体参数（包裹武器，动态，使用世界坐标）
      private bool UpdateWeaponCapsule()
      {
         if (_weaponObject == null) {
            //Debug.LogError("武器对象为空");
            return false;
         }
         var meshRenderer = _weaponObject.GetComponentInChildren<MeshRenderer>();
         if (meshRenderer == null) {
            Debug.LogError("武器对象没有MeshRenderer组件");
            return false;
         }
         Bounds bounds = meshRenderer.bounds; // 世界坐标
         Vector3 center = bounds.center;
         Vector3 extents = bounds.extents;
         Vector3 up = _weaponObject.transform.up; // 武器本地y轴在世界坐标下的方向
         _capsulePoint1 = center + up * extents.y;
         _capsulePoint2 = center - up * extents.y;
         //_capsuleRadius = 0.1f;//Mathf.Max(extents.x, extents.z);
         if (_capsuleRadius < 0.001f) _capsuleRadius = 0.01f;
         return true;
      }
   
      // 外部调用：进行胶囊检测（动态）
      public void DetectWeaponCapsule()
      {
         if (!UpdateWeaponCapsule()) return;
         Debug.Log("检测武器胶囊");
         Collider[] hits = Physics.OverlapCapsule(_capsulePoint1, _capsulePoint2, _capsuleRadius, _WeaponHitMask);
         foreach (var hit in hits)
         {
            _onWeaponHit?.Invoke(hit);
         }
      }
      
     #region Gizmos
       private void OnDrawGizmos()
       {
          Gizmos.DrawWireCube(transform.position - _Center, _Bounds);
          // 动态画出武器胶囊
          if (UpdateWeaponCapsule())
          {
             Gizmos.color = Color.red;
             DrawWireCapsule(_capsulePoint1, _capsulePoint2, _capsuleRadius);
             Gizmos.color = Color.white;
          }
       }
    
       // Gizmos辅助方法：画胶囊（世界坐标）
       private void DrawWireCapsule(Vector3 p1, Vector3 p2, float radius)
       {
          // 只做简单可视化，Unity没有内置Gizmos画胶囊，采用球+线段近似
          Gizmos.DrawWireSphere(p1, radius);
          Gizmos.DrawWireSphere(p2, radius);
          Gizmos.DrawLine(p1 + Vector3.forward * radius, p2 + Vector3.forward * radius);
          Gizmos.DrawLine(p1 - Vector3.forward * radius, p2 - Vector3.forward * radius);
          Gizmos.DrawLine(p1 + Vector3.right * radius, p2 + Vector3.right * radius);
          Gizmos.DrawLine(p1 - Vector3.right * radius, p2 - Vector3.right * radius);
       }
     #endregion
   #endregion
}
