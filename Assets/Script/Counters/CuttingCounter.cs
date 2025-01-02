using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    public static event EventHandler OnAnyCut;

    new public static void RestStaticData()
    {
        OnAnyCut = null;
    }

    // ���ڲ����в˶������¼�
    public event EventHandler OnCut;

    // ���Ľ�����UI�¼�
    public event EventHandler <IHasProgress.OnPregressChangedEventArgs> OnPregressChanged;

    [SerializeField] private CuttingRecipeSO[] cutKitchenObjectSoArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        // �жϳ������Ƿ�����Ʒ
        if (!HasKitchenObject())
        {
            // ������û����Ʒ�����ж���������Ƿ�����Ʒ
            // There is no KitchenObject
            if (player.HasKitchenObject())
            {
                // player is carring sthing 
                // �ж�������ϵ�ʳ���Ƿ���Խ����и�
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // �����и�����ڳ�����
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO =  GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            } else
            {
                // player not carring anything
            }
        } else
        {
            // ����������Ʒ�����ж���������Ƿ�����Ʒ
            // There is a KitchenObject
            if (player.HasKitchenObject())
            {
                // player is carring somthing
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a Plate

                    // �ѳ����ϵ���Ʒ��ӽ� plateKitchenObject
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // �ж��Ƿ���ӳɹ� �����ӳɹ������ٳ����ϵ���Ʒ
                        GetKitchenObject().DestroySelf();
                    }
                }
            } else
            {
                // player is not carring anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    // �в˵Ļ���
    public override void InteractAlternate(Player player)
    {
        // There is a KitchenObject here? And it can be cut?
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a KitchenObject here
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            OnAnyCut?.Invoke(this, EventArgs.Empty);
            Debug.Log(OnAnyCut.GetInvocationList().Length);
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
            {
                progressNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            // �ж��и�����Ƿ�����
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // �����������и���Ԥ����
                kitchenObjectSO outputKitchObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchObjectSO, this);
            }
        }
    }

    // �������ײ˵�CuttingRecipeSO �жϷ������ϵ�ʳ�� �Ƿ��ڲ���(input)��  �����򷵻ض�Ӧ�Ĳ���
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // ѭ������ CuttingRecipeSO �е� ���� �и�����
        foreach (CuttingRecipeSO cuttingRecipeSO in cutKitchenObjectSoArray)
        {
            // ����鵽 �� ����ȥ��Ԥ���� һ������ ������ �ӹ�֮�������
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        // ������������û�з���
        return null;
    }

    // ���ݲ��׷����и���ʳ��
    private kitchenObjectSO GetOutputForInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // ���� CuttingRecipeSO ���ͱ��� ������ GetCuttingRecipeSOWithInput() ���صĲ���
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        // ������صĲ��ײ�Ϊ�� �򷵻� ���׵� ���
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }else
        {
            return null;
        }
    }

    // �������Ƿ�Ϊ�� ��Ϊ��������з��ϵĲ���(true) ��֮û�з��ϵ�(false)
    private bool HasRecipeWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    } 
}
