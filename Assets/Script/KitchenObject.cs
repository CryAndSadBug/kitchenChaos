using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    // 用于在Unity页面上给对应的 perfab 加上对应的 kitchenObjectSO
    [SerializeField] private kitchenObjectSO kitchenObjectSO;


    // 橱柜的引用
    private IKitchenObjectParent KitchenObjectParent;


    // 此方法 返回这个 Prefab 所对应的 kitchenObjectSO
    public kitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    #region 让这个 Prefab 知道自己位于哪个橱柜上
    // 此方法 设置这个 Prefab 所对应的 橱柜
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

        // 把当前橱柜的 最上方 设置为 方法参数传回的最上方
        // GetKitchenObjectFollwTransform() 返回的是最上方位置
        transform.parent = iKitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }


    // 此方法 获取这个 Prefab 所对应的 橱柜
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return KitchenObjectParent;
    }
    #endregion
    
    // 对象销毁自己的方法
    public void DestroySelf()
    {
        KitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            // this as PlateKitchenObject;  意思是把当前的对象转换为 PlateKitchenObject类型
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    // 生成预制体
    public static KitchenObject SpawnKitchenObject(kitchenObjectSO kitchenObjectSO, IKitchenObjectParent IkitchenObjectParent)
    {
        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectParent(IkitchenObjectParent);

        return kitchenObject;
    }
}
