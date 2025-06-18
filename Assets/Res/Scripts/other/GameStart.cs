using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject m_Prefab;

    private void Start()
    {
        Instantiate(m_Prefab);
    }
}
