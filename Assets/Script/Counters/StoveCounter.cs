using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;
using System;
using static UnityEngine.CullingGroup;

public class StoveCounter : BaseCounter, IHasProgress
{

    // 更改进度条UI事件
    public event EventHandler<IHasProgress.OnPregressChangedEventArgs> OnPregressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;

    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private State state;

    public enum State
    {
        Idel,
        frying,
        fried,
        Burned
    }

    private void Start()
    {
        state = State.Idel;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            //状态机
            switch (state)
                {
                   case State.Idel:
                       break;
                   case State.frying:
                        fryingTimer += Time.deltaTime;

                        OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                        });

                        if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                        {
                            // Fried
                            fryingTimer = 0f;
                            
                            // 销毁自身
                            GetKitchenObject().DestroySelf();
                            // 生成煎熟的
                            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                            state = State.fried;

                            burningTimer = 0f;

                            burningRecipeSO = GetBorningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = state
                            });
                        }
                    break;
                   case State.fried:
                       burningTimer += Time.deltaTime;

                       OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                       {
                           progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                       });

                       if (burningTimer >= burningRecipeSO.burningTimerMax)
                       {
                           // 销毁自身
                           GetKitchenObject().DestroySelf();
                           // 生成煎熟的
                           KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                           state = State.Burned;

                           OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                           {
                               state = state
                           });

                           OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                           {
                               progressNormalized = 0f
                           });
                       }   
                   break;
                   case State.Burned:
                   break;
                }
        }
    }

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
                // 判断玩家手上的食材是否可以进行煎炸
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // 可以煎炸则放在橱柜上
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
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

                state = State.Idel;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            } else
            {
                // player is not carring anything
                GetKitchenObject().SetKitchenObjectParent(player);

                // 把物体给到玩家之后让状态机进入待机状态
                state = State.Idel;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs 
                { 
                    state = state
                });

                OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    // 遍历菜谱菜单FryingRecipeSO 判断放在桌上的食材 是否在菜谱(input)中  存在则返回对应的菜谱
    private FryingRecipeSO GetFryingRecipeSOWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // 循环遍历 FryingRecipeSO 中的 所有 菜谱
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            // 如果遍到 和 放上去的预制体 一样的则 返回其 加工之后的样子
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        // 遍历结束代表没有符合
        return null;
    }

    // 遍历菜谱菜单BurningRecipeSO 判断放在桌上的食材 是否在菜谱(input)中  存在则返回对应的菜谱
    private BurningRecipeSO GetBorningRecipeSOWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // 循环遍历 FryingRecipeSO 中的 所有 菜谱
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            // 如果遍到 和 放上去的预制体 一样的则 返回其 加工之后的样子
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        // 遍历结束代表没有符合
        return null;
    }

    // 根据菜谱返回切割后的食材
    private kitchenObjectSO GetOutputForInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // 定义 FryingRecipeSO 类型变量 接收用 GetFryingRecipeSOWithInput() 返回的菜谱
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        // 如果返回的菜谱不为空 则返回 菜谱的 输出
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        } else
        {
            return null;
        }
    }

    // 检测菜谱是否为空 不为空则代表有符合的菜谱(true) 反之没有符合的(false)
    private bool HasRecipeWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingrecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingrecipeSO != null;
    }

    public bool IsFried()
    {
        return state == State.fried;
    }
}
