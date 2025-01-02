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

    // ���� ��������Ϸ�λ�� 
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    #region ͨ���ӿ�ʵ�ֵ� ���ã���ȡ����գ��жϴ��� (kitchenObject)
    // ��� KitchenObject
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // ��ȡ kitchenObject
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // ����һ������ֵ �����ж� kitchenObject �Ƿ����
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    // ���� kitchenObject
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
