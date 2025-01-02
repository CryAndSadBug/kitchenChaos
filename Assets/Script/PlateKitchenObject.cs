using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public kitchenObjectSO kitchenObjectSO;
    }

    // 经过处理的食材
    [SerializeField] private List<kitchenObjectSO> vaildKitchenObjectSOList;

    private List<kitchenObjectSO> kitchenObjecySOList;

    private void Awake()
    {
        kitchenObjecySOList = new List<kitchenObjectSO>();
    }

    public bool TryAddIngredient(kitchenObjectSO kitchenObjectSO)
    {

        if (!vaildKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // 没处理过的材料
            return false;
        }
        // kitchenObjecySOList.Contains(kitchenObjectSO)   [检查 kitchenObjecySOList 下是否含有 kitchenObjectSO的类型]
        // 如 列表中已经含有了 Tamato 的 kitchenObjectSO  , 下次对Tamato 进行互动时则不会添加
        if (kitchenObjecySOList.Contains(kitchenObjectSO))
        {
            return false;
        }else
        {
            kitchenObjecySOList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            return true;
        }
    }

    public List<kitchenObjectSO> GetPlateKitchenObjectList()
    {
        return kitchenObjecySOList;
    }
}
