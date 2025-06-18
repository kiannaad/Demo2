using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLookTarget : MonoBehaviour
{
    [SerializeField] private GameObject _LookTarget;
    public GameObject GetEnemyLookTarget() => _LookTarget;
}
