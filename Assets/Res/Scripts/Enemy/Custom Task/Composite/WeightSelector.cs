using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom/Enemy/Composite")]
public class WeightSelector : Composite
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("每个子节点的权重列表")]
    public List<float> weights = new List<float>();
    
    private float totalWeight = 0f;

    private TaskStatus executionStatus = TaskStatus.Inactive;
    private int abortIndex = -1;


    public override void OnAwake()
    {
        base.OnAwake();
        totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }
    }

    public override int CurrentChildIndex()
    {
        return abortIndex == -1 ? SelectRandomChild() : abortIndex;
    }

    public override bool CanExecute()
    {
        return executionStatus != TaskStatus.Success;
    }

    public override void OnChildExecuted(TaskStatus childStatus)
    {
        executionStatus = childStatus;
    }

    public override void OnConditionalAbort(int childIndex)
    {
        executionStatus = TaskStatus.Inactive;
        abortIndex = childIndex;
    }

    public override void OnEnd()
    {
        executionStatus = TaskStatus.Inactive;
        abortIndex = -1;
    }

    public override void OnReset()
    {
        base.OnReset();
        executionStatus = TaskStatus.Inactive;
        abortIndex = -1;
    }
    

    private int SelectRandomChild()
    {
        if (children.Count == 0 || totalWeight <= 0)
            return -1;

        float random = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        for (int i = 0; i < children.Count; i++)
        {
            currentWeight += weights[i];
            if (random <= currentWeight)
            {
                return i;
            }
        }

        return children.Count - 1; // 防止浮点数精度问题
    }

}
