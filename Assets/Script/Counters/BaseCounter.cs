using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    public static event EventHandler OnAnyObjectPlaceHere;

    public static void RestStaticData()
    {
        OnAnyObjectPlaceHere = null;
    }

    [SerializeField]private Transform counterTopPoint;  

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate();");
    }

    // 返回 橱柜的最上方位置 
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    #region 通过接口实现的 设置，获取，清空，判断存在 (kitchenObject)
    // 清空 KitchenObject
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // 获取 kitchenObject
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // 返回一个布尔值 用于判断 kitchenObject 是否存在
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    // 设置 kitchenObject
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyObjectPlaceHere?.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion
}
