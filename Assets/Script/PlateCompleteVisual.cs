using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static PlateCompleteVisual;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchObject_GameObject
    {
        public kitchenObjectSO kitchenObjectSO;

        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKichenObject;

    [SerializeField] private List<KitchObject_GameObject> kitchObjectGameObjectList;

    private void Start()
    {
        plateKichenObject.OnIngredientAdded += PlateKichenObject_OnIngredientAdded;

        foreach (KitchObject_GameObject kitchObjectGameObject in kitchObjectGameObjectList)
        {
            kitchObjectGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKichenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchObject_GameObject kitchObjectGameObject in kitchObjectGameObjectList)
        {
            if (kitchObjectGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchObjectGameObject.gameObject.SetActive(true);
            }
        }
    }
}
