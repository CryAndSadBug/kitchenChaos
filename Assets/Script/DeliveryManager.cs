using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{
    // ʳ�������¼�
    public event EventHandler OnRecipeSpawned;
    // ʳ������¼�
    public event EventHandler OnRecipeCompleted;

    public event EventHandler OnRecipeSuccess;

    public event EventHandler OnRecipeFailed;

    // 1.����Ϊ����
    public static DeliveryManager Instance { get; private set; }

    // ����ʳ�׵�����
    [SerializeField] private RicipeListSO ricipeListSO;

    // ����ʳ���б�
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;

    private float spawnRecipeTimerMax = 4f;

    private int waitingRicipesMax = 4;

    private int successfulRecipeAmount;


    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>();

        // 2.����Ϊ����
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
                // �������ʳ���б�С�� ���ʳ���б� �ͼ�������ʳ��
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
                // ������������ʳ����ƥ��
                bool plateContentsMatchesRecipe = true;
                foreach (kitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // ѭ�����ʳ���е���������
                    // Cycing through all indredients in the Recipe
                    bool ingredientFound = false;
                    foreach (kitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetPlateKitchenObjectList())
                    {
                        // Cycing through all indredients in the Recipe
                        // ѭ�����ʳ���е���������

                        // ������ʳ���е��������ݽ��бȽ��Ƿ�һ��
                        if (recipeKitchenObjectSO == plateKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This Rcipe ingredient(��ɳɷ�) was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe! (����ṩ����ȷ��ʳ�ף�)
                    successfulRecipeAmount++;
                    
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

                    // ʳ�׳ɹ��¼�
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }

        // ʳ��ʧ���¼�
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
