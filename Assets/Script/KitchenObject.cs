using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    // ������Unityҳ���ϸ���Ӧ�� perfab ���϶�Ӧ�� kitchenObjectSO
    [SerializeField] private kitchenObjectSO kitchenObjectSO;


    // ���������
    private IKitchenObjectParent KitchenObjectParent;


    // �˷��� ������� Prefab ����Ӧ�� kitchenObjectSO
    public kitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    #region ����� Prefab ֪���Լ�λ���ĸ�������
    // �˷��� ������� Prefab ����Ӧ�� ����
    public void SetKitchenObjectParent(IKitchenObjectParent iKitchenObjectParent)
    {
        if (this.KitchenObjectParent != null)
        {
            this.KitchenObjectParent.ClearKitchenObject();
        }

        this.KitchenObjectParent = iKitchenObjectParent;

        if (iKitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent always has a KitchenObject!");
        }

        iKitchenObjectParent.SetKitchenObject(this);

        // �ѵ�ǰ����� ���Ϸ� ����Ϊ �����������ص����Ϸ�
        // GetKitchenObjectFollwTransform() ���ص������Ϸ�λ��
        transform.parent = iKitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }


    // �˷��� ��ȡ��� Prefab ����Ӧ�� ����
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return KitchenObjectParent;
    }
    #endregion
    
    // ���������Լ��ķ���
    public void DestroySelf()
    {
        KitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            // this as PlateKitchenObject;  ��˼�ǰѵ�ǰ�Ķ���ת��Ϊ PlateKitchenObject����
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    // ����Ԥ����
    public static KitchenObject SpawnKitchenObject(kitchenObjectSO kitchenObjectSO, IKitchenObjectParent IkitchenObjectParent)
    {
        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectParent(IkitchenObjectParent);

        return kitchenObject;
    }
}
