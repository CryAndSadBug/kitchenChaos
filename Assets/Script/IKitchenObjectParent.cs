using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ӿ�
public interface IKitchenObjectParent
{

    // ���� ��������Ϸ�λ�� 
    public Transform GetKitchenObjectFollowTransform();
    #region ���ã���ȡ����գ��жϴ��� (kitchenObject)
    // ���� kitchenObject
    public void SetKitchenObject(KitchenObject kitchenObject);

    // ��ȡ kitchenObject
    public KitchenObject GetKitchenObject();

    // ��� KitchenObject
    public void ClearKitchenObject();

    // ����һ������ֵ �����ж� kitchenObject �Ƿ����
    public bool HasKitchenObject();
    #endregion 
}
