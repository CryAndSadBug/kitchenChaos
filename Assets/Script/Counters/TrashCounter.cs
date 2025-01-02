using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class TrashCounter : BaseCounter
{
    // 只有这一个运用场景所以用static只对本脚本内有用
    public static event EventHandler OnAnyObjectTrashed;
    new public static void RestStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
