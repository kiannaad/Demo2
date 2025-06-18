using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.Playables;

public class ConditionalTrackBehaviour : PlayableBehaviour
{
   private ChangeStateEventArg changeStateEventArg;
   private bool _NeedTriggerInterrupt;

   public void Init(ChangeStateEventArg changeStateEventArg, bool needTriggerInterrupt)
   {
      this.changeStateEventArg = changeStateEventArg;
      _NeedTriggerInterrupt = needTriggerInterrupt;
   }
   
   public override void OnBehaviourPlay(Playable playable, FrameData info)
   {
      base.OnBehaviourPlay(playable, info);
      
      if (changeStateEventArg != null)
      {
         var e = ChangeStateEventArg.Create(changeStateEventArg.targetStateName, changeStateEventArg._targetTimeline);
         e.isEnd = false;
         
         GameEntry.Event?.FireNow(this, e);
      }
   }

   private bool _completed; // 确保只触发一次
   
   public override void OnBehaviourPause(Playable playable, FrameData info)
   {
     
      base.OnBehaviourPause(playable, info);
      
      if (_completed) return; // 防止多次调用
        
      // 检测是否是自然结束
      double duration = playable.GetDuration();
      double current = playable.GetTime();
      double timeEpsilon = 0.05; // 0.05秒容差
        
      bool isNaturalEnd = (duration - current) <= timeEpsilon;

      if (_NeedTriggerInterrupt) isNaturalEnd = true;
      
      if (isNaturalEnd )
      {
          Debug.Log("OnBehaviourPause");
         _completed = true;
         if (changeStateEventArg != null)
         {
            var e = ChangeStateEventArg.Create(changeStateEventArg.targetStateName,  changeStateEventArg._targetTimeline);
            e.isEnd = true;
            GameEntry.Event?.FireNow(this, e);
         }
      }
     
   }
}
