using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{

    // TextMeshPro��3D����е� TextMeshProUGUI ��Canvas�е� 
    [SerializeField] private TextMeshProUGUI recipeNameText;

    [SerializeField] private Transform IconContainer;

    [SerializeField] private Transform IconTemplate;

    private void Awake()
    {
        IconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;

        foreach (Transform child in IconContainer)
        {
            if (child == IconTemplate) continue;
            Destroy(child.gameObject); 
        }

        foreach (kitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {

            Transform IconTransform = Instantiate(IconTemplate, IconContainer);
            IconTransform.gameObject.SetActive(true);
            IconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
