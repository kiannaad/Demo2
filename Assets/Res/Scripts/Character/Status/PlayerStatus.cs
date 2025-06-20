using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
   [SerializeField] private PlayerStatusData playerStatusData;

   private AttributeSystem _attributeSystem;

   private void Awake()
   {
        _attributeSystem = new AttributeSystem();
   }

   private void Start()
   {
        InitAttributeSystem();
   }

   private void Update()
   {
        Debug.Log(_attributeSystem.GetFinalAttributeValue(AttributeType.CurHp));
   }

   private void InitAttributeSystem()
   {
        _attributeSystem.SetBaseAttributeValue(AttributeType.MaxHp, playerStatusData.MaxHp);
        _attributeSystem.SetBaseAttributeValue(AttributeType.Attack, playerStatusData.Attack);
        _attributeSystem.SetBaseAttributeValue(AttributeType.Defense, playerStatusData.Defense);
        _attributeSystem.SetBaseAttributeValue(AttributeType.CurHp, playerStatusData.MaxHp);
   }
}
