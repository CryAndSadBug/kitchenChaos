using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 接口
public interface IKitchenObjectParent
{

    // 返回 橱柜的最上方位置 
    public Transform GetKitchenObjectFollowTransform();
    #region 设置，获取，清空，判断存在 (kitchenObject)
    // 设置 kitchenObject
    public void SetKitchenObject(KitchenObject kitchenObject);

    // 获取 kitchenObject
    public KitchenObject GetKitchenObject();

    // 清空 KitchenObject
    public void ClearKitchenObject();

    // 返回一个布尔值 用于判断 kitchenObject 是否存在
    public bool HasKitchenObject();
    #endregion 
}
