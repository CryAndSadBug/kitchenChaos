using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    // [SerializeField] private GameObject tomatoPrefab ���Ժ� [SerializeField] private Transform tomatoPrefab ����ת��
    [SerializeField] private kitchenObjectSO kitchenObjectSo;


    public override void Interact(Player player)
    {
        // �жϳ������Ƿ�����Ʒ
        if (!HasKitchenObject())
        {
            // ������û����Ʒ�����ж���������Ƿ�����Ʒ
            // There is no KitchenObject
            if (player.HasKitchenObject())
            {
                // ������ڳ�����
                // player is carring sthing 
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else
            {
                // player not carring anything
            }
        }else
        {
            // ����������Ʒ,���ж���������Ƿ�����Ʒ
            // There is a KitchenObject
            if (player.HasKitchenObject())
            {
                // player is carring somthing
                // �ж�������ϵĶ����Ƿ�Ϊ PlateKitchenObject ����
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a Plate

                    // �ѳ����ϵ���Ʒ��ӽ� plateKitchenObject
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // �ж��Ƿ���ӳɹ� �����ӳɹ������ٳ����ϵ���Ʒ
                        GetKitchenObject().DestroySelf();
                    }  
                }else
                {
                    // ����õĲ������Ӷ��Ǳ�Ķ���
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
