using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TerrainDetection : MonoBehaviour
{
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

   private void Awake()
   {
      _characterController = GetComponent<CharacterController>();
      
      _StandDirection =  _DefaultNormalDirection;
      _Bounds /= 2f;
   }
   
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
   
   private void OnDrawGizmos()
   {
      Gizmos.DrawWireCube(transform.position - _Center, _Bounds);
   }
}
