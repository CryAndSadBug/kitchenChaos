using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{

    [SerializeField] private Transform Container;

    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        // 食谱生成事件
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;

        // 食谱完成事件
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // 循环遍历出Container的所有子组件
        foreach (Transform child in Container)
        {
            // 判断子组件是否为 recipeTemplate 是就跳过
            if (child == recipeTemplate) continue;
            // 销毁除了 recipeTemplate 外的所有子组件
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform =  Instantiate(recipeTemplate, Container);
            recipeTransform.gameObject.SetActive(true);

            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }

    }

}
