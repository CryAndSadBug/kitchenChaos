using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class TrashCounter : BaseCounter
{
    // ֻ����һ�����ó���������staticֻ�Ա��ű�������
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
