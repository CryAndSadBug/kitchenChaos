using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{
    // 食谱生成事件
    public event EventHandler OnRecipeSpawned;
    // 食谱完成事件
    public event EventHandler OnRecipeCompleted;

    public event EventHandler OnRecipeSuccess;

    public event EventHandler OnRecipeFailed;

    // 1.制作为单例
    public static DeliveryManager Instance { get; private set; }

    // 所有食谱的引用
    [SerializeField] private RicipeListSO ricipeListSO;

    // 待做食谱列表
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;

    private float spawnRecipeTimerMax = 4f;

    private int waitingRicipesMax = 4;

    private int successfulRecipeAmount;


    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>();

        // 2.制作为单例
        Instance = this;
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRicipesMax)
            {
                // 如果待做食谱列表小于 最大食谱列表 就继续生成食谱
                RecipeSO waitingRecipeSO = ricipeListSO.recipeSOList[UnityEngine.Random.Range(0, ricipeListSO.recipeSOList.Count)];
                
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliveryRicipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetPlateKitchenObjectList().Count)
            {
                // Has the same number of ingredients
                // 盘子内容物与食谱相匹配
                bool plateContentsMatchesRecipe = true;
                foreach (kitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // 循环浏览食谱中的所有内容
                    // Cycing through all indredients in the Recipe
                    bool ingredientFound = false;
                    foreach (kitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetPlateKitchenObjectList())
                    {
                        // Cycing through all indredients in the Recipe
                        // 循环浏览食谱中的所有内容

                        // 遍历出食谱中的所有内容进行比较是否一致
                        if (recipeKitchenObjectSO == plateKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This Rcipe ingredient(组成成分) was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe! (玩家提供了正确的食谱！)
                    successfulRecipeAmount++;
                    
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

                    // 食谱成功事件
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }

        // 食谱失败事件
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipeAmount;
    }

}
