using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform IconTemPlate;


    private void Awake()
    {
        IconTemPlate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    // 更新视觉效果
    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == IconTemPlate) continue;

            Destroy(child.gameObject);
        }

        foreach (kitchenObjectSO kitchenObjectSO in plateKitchenObject.GetPlateKitchenObjectList())
        {
                Transform IconTransform = Instantiate(IconTemPlate, transform);

                IconTransform.gameObject.SetActive(true);
            
                IconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
