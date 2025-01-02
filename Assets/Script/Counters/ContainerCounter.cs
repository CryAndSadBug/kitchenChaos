using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    // 当玩家获取对象时触发的事件
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private kitchenObjectSO kitchenObjectSo;

    public override void Interact(Player player)
    {
        if (!player.GetKitchenObject()) {
            // player is not carring anything

            KitchenObject.SpawnKitchenObject(kitchenObjectSo, player);

            // 判断事件是否为null 不为null  调用异步请求.Invoke(发送者，空参)
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
