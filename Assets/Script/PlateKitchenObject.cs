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

    // ���������ʳ��
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
            // û������Ĳ���
            return false;
        }
        // kitchenObjecySOList.Contains(kitchenObjectSO)   [��� kitchenObjecySOList ���Ƿ��� kitchenObjectSO������]
        // �� �б����Ѿ������� Tamato �� kitchenObjectSO  , �´ζ�Tamato ���л���ʱ�򲻻����
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
