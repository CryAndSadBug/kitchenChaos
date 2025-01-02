using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 创建菜单
[CreateAssetMenu()]

public class CuttingRecipeSO : ScriptableObject
{

    // 输入
    public kitchenObjectSO input;

    // 输出
    public kitchenObjectSO output;

    // 切割的进度
    public int cuttingProgressMax;
}
