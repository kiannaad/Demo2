using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

public interface IRegisterSwitchHandle
{
   public void RegisterHandle(IFsm<PlayerState> fsm);
   public void UnregisterHandle(IFsm<PlayerState> fsm);
}
