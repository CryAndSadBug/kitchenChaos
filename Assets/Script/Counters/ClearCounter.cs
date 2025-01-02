using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    // [SerializeField] private GameObject tomatoPrefab 可以和 [SerializeField] private Transform tomatoPrefab 互相转换
    [SerializeField] private kitchenObjectSO kitchenObjectSo;


    public override void Interact(Player player)
    {
        // 判断橱柜上是否有物品
        if (!HasKitchenObject())
        {
            // 橱柜上没有物品，则判断玩家手上是否有物品
            // There is no KitchenObject
            if (player.HasKitchenObject())
            {
                // 有则放在橱柜上
                // player is carring sthing 
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else
            {
                // player not carring anything
            }
        }else
        {
            // 橱柜上有物品,则判断玩家手里是否有物品
            // There is a KitchenObject
            if (player.HasKitchenObject())
            {
                // player is carring somthing
                // 判断玩家手上的东西是否为 PlateKitchenObject 类型
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a Plate

                    // 把橱柜上的物品添加进 plateKitchenObject
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // 判断是否添加成功 如果添加成功再销毁橱柜上的物品
                        GetKitchenObject().DestroySelf();
                    }  
                }else
                {
                    // 玩家拿的不是盘子而是别的东西
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // this is a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }

            } else
            {
                // player is not carring anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
