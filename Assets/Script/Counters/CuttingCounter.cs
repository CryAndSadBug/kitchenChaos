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

    // 用于播放切菜动画的事件
    public event EventHandler OnCut;

    // 更改进度条UI事件
    public event EventHandler <IHasProgress.OnPregressChangedEventArgs> OnPregressChanged;

    [SerializeField] private CuttingRecipeSO[] cutKitchenObjectSoArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        // 判断橱柜上是否有物品
        if (!HasKitchenObject())
        {
            // 橱柜上没有物品，则判断玩家手上是否有物品
            // There is no KitchenObject
            if (player.HasKitchenObject())
            {
                // player is carring sthing 
                // 判断玩家手上的食材是否可以进行切割
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // 可以切割则放在橱柜上
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
            // 橱柜上有物品，则判断玩家手里是否有物品
            // There is a KitchenObject
            if (player.HasKitchenObject())
            {
                // player is carring somthing
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a Plate

                    // 把橱柜上的物品添加进 plateKitchenObject
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // 判断是否添加成功 如果添加成功再销毁橱柜上的物品
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

    // 切菜的互动
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

            // 判断切割进度是否满了
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // 满了则生成切割后的预制体
                kitchenObjectSO outputKitchObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchObjectSO, this);
            }
        }
    }

    // 遍历菜谱菜单CuttingRecipeSO 判断放在桌上的食材 是否在菜谱(input)中  存在则返回对应的菜谱
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // 循环遍历 CuttingRecipeSO 中的 所有 切割种类
        foreach (CuttingRecipeSO cuttingRecipeSO in cutKitchenObjectSoArray)
        {
            // 如果遍到 和 放上去的预制体 一样的则 返回其 加工之后的样子
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        // 遍历结束代表没有符合
        return null;
    }

    // 根据菜谱返回切割后的食材
    private kitchenObjectSO GetOutputForInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // 定义 CuttingRecipeSO 类型变量 接收用 GetCuttingRecipeSOWithInput() 返回的菜谱
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        // 如果返回的菜谱不为空 则返回 菜谱的 输出
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }else
        {
            return null;
        }
    }

    // 检测菜谱是否为空 不为空则代表有符合的菜谱(true) 反之没有符合的(false)
    private bool HasRecipeWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    } 
}
