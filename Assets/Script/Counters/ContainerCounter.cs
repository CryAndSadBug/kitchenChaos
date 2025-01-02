using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    // ����һ�ȡ����ʱ�������¼�
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private kitchenObjectSO kitchenObjectSo;

    public override void Interact(Player player)
    {
        if (!player.GetKitchenObject()) {
            // player is not carring anything

            KitchenObject.SpawnKitchenObject(kitchenObjectSo, player);

            // �ж��¼��Ƿ�Ϊnull ��Ϊnull  �����첽����.Invoke(�����ߣ��ղ�)
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
